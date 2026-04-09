using System.Collections.Generic;
using System.Linq;

namespace WitnessSolver
{
    public class Section
    {
        public readonly HashSet<Cell> Cells;
        public readonly HashSet<SolverCell> SolverCells;
        public bool Checked;
        public bool Correct;
        internal readonly int cellMax;
        private readonly bool _useSolverCells;

        // Definition-cell constructors (used by old Puzzle path)
        public Section(IEnumerable<Cell> cells, int cellMax)
            : this(cellMax)
        {
            this.Cells = new HashSet<Cell>(cells);
            foreach (var cell in cells)
                cell.Section = this;
        }

        public Section(HashSet<Cell> cells, int cellMax)
            : this(cellMax)
        {
            this.Cells = cells;
            foreach (var cell in cells)
                cell.Section = this;
        }

        // SolverCell constructors (used by SolverGraph)
        public Section(SolverCell[] cells, int cellMax)
            : this(cellMax)
        {
            this._useSolverCells = true;
            this.SolverCells = new HashSet<SolverCell>(cells);
            foreach (var cell in cells)
                cell.Section = this;
        }

        public Section(HashSet<SolverCell> cells, int cellMax, bool solverMode)
            : this(cellMax)
        {
            this._useSolverCells = true;
            this.SolverCells = cells;
            foreach (var cell in cells)
                cell.Section = this;
        }

        private Section(int cellMax)
        {
            this.cellMax = cellMax;
            this.Checked = false;
        }

        public void UnionWith(Section section)
        {
            if (this._useSolverCells)
            {
                this.SolverCells.UnionWith(section.SolverCells);
                foreach (var cell in section.SolverCells)
                    cell.Section = this;
            }
            else
            {
                this.Cells.UnionWith(section.Cells);
                foreach (var cell in section.Cells)
                    cell.Section = this;
            }
            this.Checked = false;
        }

        public bool CheckSection()
        {
            if (this.Checked)
                return this.Correct;

            var good = true;
            var squareLetters = new HashSet<char>();
            var starLetters = new HashSet<char>();
            var colorLetterCount = new Dictionary<char, int>();
            var tetrisList = new List<Tetris>();

            if (this._useSolverCells)
            {
                foreach (var cell in this.SolverCells)
                {
                    if (!ProcessCellForCheck(cell.SquareColor, cell.StarColor, cell.TriangleCount,
                        cell.UsedEdgeCount, cell.Tetris, squareLetters, starLetters, colorLetterCount, tetrisList))
                    {
                        good = false;
                        break;
                    }
                }
            }
            else
            {
                foreach (var cell in this.Cells)
                {
                    if (!ProcessCellForCheck(cell.SquareColorLetter, cell.StarColorLetter, cell.TriangleCount,
                        cell.UsedEdgeCount, cell.Tetris, squareLetters, starLetters, colorLetterCount, tetrisList))
                    {
                        good = false;
                        break;
                    }
                }
            }

            if (good && squareLetters.Count > 1)
                good = false;

            if (good)
            {
                foreach (var starLetter in starLetters)
                {
                    if (colorLetterCount[starLetter] != 2)
                    {
                        good = false;
                        break;
                    }
                }
            }

            if (good && tetrisList.Count > 0)
                good &= SectionTetrisChecker.CanContainExactly(this, tetrisList);

            this.Checked = true;
            this.Correct = good;
            return good;
        }

        private static bool ProcessCellForCheck(char squareColor, char starColor, int? triangleCount,
            int usedEdgeCount, Tetris tetris, HashSet<char> squareLetters, HashSet<char> starLetters,
            Dictionary<char, int> colorLetterCount, List<Tetris> tetrisList)
        {
            if (squareColor != ' ')
            {
                squareLetters.Add(squareColor);
                if (!colorLetterCount.ContainsKey(squareColor))
                    colorLetterCount[squareColor] = 0;
                colorLetterCount[squareColor]++;
            }

            if (starColor != ' ')
            {
                starLetters.Add(starColor);
                if (!colorLetterCount.ContainsKey(starColor))
                    colorLetterCount[starColor] = 0;
                colorLetterCount[starColor]++;
            }

            if (triangleCount.HasValue && triangleCount.Value != usedEdgeCount)
                return false;

            if (tetris != null)
            {
                tetrisList.Add(tetris);
                if (tetris.ColorLetter != ' ')
                {
                    if (!colorLetterCount.ContainsKey(tetris.ColorLetter))
                        colorLetterCount[tetris.ColorLetter] = 0;
                    colorLetterCount[tetris.ColorLetter]++;
                }
            }

            return true;
        }

        public List<Section> FindSubSections()
        {
            if (this._useSolverCells)
                return FindSubSectionsSolver();
            return FindSubSectionsCells();
        }

        private List<Section> FindSubSectionsCells()
        {
            var visited = new bool[this.cellMax];
            var startCell = this.Cells.First();
            var cells = new HashSet<Cell> { startCell };
            var cellQueue = new Queue<Cell>(cells);
            visited[startCell.CellIndex] = true;

            while (cellQueue.Count > 0)
            {
                var currentCell = cellQueue.Dequeue();
                foreach (var edge in currentCell.EdgeLoopClockwise)
                {
                    if (!edge.Traversed && !edge.ReversedEdge.Traversed && edge.LeftCell != null && !visited[edge.LeftCell.CellIndex])
                    {
                        visited[edge.LeftCell.CellIndex] = true;
                        cellQueue.Enqueue(edge.LeftCell);
                        cells.Add(edge.LeftCell);
                    }
                }
            }

            if (cells.Count == this.Cells.Count)
                return null;

            return new List<Section>
            {
                new Section(cells, this.cellMax),
                new Section(new HashSet<Cell>(this.Cells.Where(c => !visited[c.CellIndex])), this.cellMax),
            };
        }

        private List<Section> FindSubSectionsSolver()
        {
            var visited = new bool[this.cellMax];
            SolverCell startCell = null;
            foreach (var c in this.SolverCells) { startCell = c; break; }

            var cells = new HashSet<SolverCell> { startCell };
            var cellQueue = new Queue<SolverCell>(cells);
            visited[startCell.Index] = true;

            while (cellQueue.Count > 0)
            {
                var currentCell = cellQueue.Dequeue();
                foreach (var edge in currentCell.EdgeLoop)
                {
                    if (!edge.Traversed && !edge.Reverse.Traversed)
                    {
                        // Find the adjacent cell on the left side
                        foreach (var adjCell in edge.AdjacentCells)
                        {
                            if (adjCell != currentCell && this.SolverCells.Contains(adjCell) && !visited[adjCell.Index])
                            {
                                visited[adjCell.Index] = true;
                                cellQueue.Enqueue(adjCell);
                                cells.Add(adjCell);
                            }
                        }
                    }
                }
            }

            if (cells.Count == this.SolverCells.Count)
                return null;

            var remaining = new HashSet<SolverCell>();
            foreach (var c in this.SolverCells)
                if (!visited[c.Index]) remaining.Add(c);

            return new List<Section>
            {
                new Section(cells, this.cellMax, solverMode: true),
                new Section(remaining, this.cellMax, solverMode: true),
            };
        }
    }
}

