using System;
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

            // Use stackalloc for color tracking — avoids HashSet/Dictionary allocs
            // Colors: R=0, G=1, B=2, W=3, L=4, Y=5, P=6
            Span<int> colorCounts = stackalloc int[7];
            Span<bool> hasStarColor = stackalloc bool[7];
            int squareColorCount = 0;
            char firstSquareColor = ' ';
            List<Tetris> tetrisList = null;

            if (this._useSolverCells)
            {
                foreach (var cell in this.SolverCells)
                {
                    if (!CheckCell(cell.SquareColor, cell.StarColor, cell.TriangleCount,
                        cell.UsedEdgeCount, cell.Tetris, colorCounts, hasStarColor,
                        ref squareColorCount, ref firstSquareColor, ref tetrisList))
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
                    if (!CheckCell(cell.SquareColorLetter, cell.StarColorLetter, cell.TriangleCount,
                        cell.UsedEdgeCount, cell.Tetris, colorCounts, hasStarColor,
                        ref squareColorCount, ref firstSquareColor, ref tetrisList))
                    {
                        good = false;
                        break;
                    }
                }
            }

            // Star color check: exactly 2 of each star color
            if (good)
            {
                for (int i = 0; i < 7; i++)
                {
                    if (hasStarColor[i] && colorCounts[i] != 2)
                    {
                        good = false;
                        break;
                    }
                }
            }

            if (good && tetrisList != null)
                good &= SectionTetrisChecker.CanContainExactly(this, tetrisList);

            this.Checked = true;
            this.Correct = good;
            return good;
        }

        private static bool CheckCell(char squareColor, char starColor, int? triangleCount,
            int usedEdgeCount, Tetris tetris, Span<int> colorCounts, Span<bool> hasStarColor,
            ref int squareColorCount, ref char firstSquareColor, ref List<Tetris> tetrisList)
        {
            if (squareColor != ' ')
            {
                if (squareColorCount == 0)
                    firstSquareColor = squareColor;
                else if (squareColor != firstSquareColor)
                    return false; // Multiple square colors — fail immediately
                squareColorCount++;
                int idx = ColorIndex(squareColor);
                if (idx >= 0) colorCounts[idx]++;
            }

            if (starColor != ' ')
            {
                int idx = ColorIndex(starColor);
                if (idx >= 0)
                {
                    hasStarColor[idx] = true;
                    colorCounts[idx]++;
                }
            }

            if (triangleCount.HasValue && triangleCount.Value != usedEdgeCount)
                return false;

            if (tetris != null)
            {
                tetrisList ??= new List<Tetris>();
                tetrisList.Add(tetris);
                if (tetris.ColorLetter != ' ')
                {
                    int idx = ColorIndex(tetris.ColorLetter);
                    if (idx >= 0) colorCounts[idx]++;
                }
            }

            return true;
        }

        private static int ColorIndex(char c)
        {
            switch (c)
            {
                case 'R': return 0;
                case 'G': return 1;
                case 'B': return 2;
                case 'W': return 3;
                case 'L': return 4;
                case 'Y': return 5;
                case 'P': return 6;
                default: return -1;
            }
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

