using System.Collections.Generic;
using System.Linq;

namespace WitnessSolver
{
    class Section
    {
        public readonly HashSet<Cell> Cells;
        public bool Checked;
        public bool Correct;
        internal readonly int cellMax;

        public Section(IEnumerable<Cell> cells, int cellMax)
            : this(cellMax)
        {
            this.Cells = new HashSet<Cell>(cells);
            foreach (var cell in cells)
            {
                cell.Section = this;
            }
        }

        public Section(HashSet<Cell> cells, int cellMax)
            : this(cellMax)
        {
            this.Cells = cells;
            foreach (var cell in cells)
            {
                cell.Section = this;
            }
        }

        private Section(int cellMax)
        {
            this.cellMax = cellMax;
            this.Checked = false;
        }

        public void UnionWith(Section section)
        {
            this.Cells.UnionWith(section.Cells);
            foreach (var cell in section.Cells)
            {
                cell.Section = this;
            }
            this.Checked = false;
        }

        public bool CheckSection()
        {
            if (this.Checked)
            {
                return this.Correct;
            }

            var good = true;

            var squareLetters = new HashSet<char>();
            var starLetters = new HashSet<char>();
            var colorLetterCount = new Dictionary<char, int>();
            var tetrisList = new List<Tetris>();
            foreach (var cell in this.Cells)
            {
                // Check for square
                if (cell.SquareColorLetter != ' ')
                {
                    squareLetters.Add(cell.SquareColorLetter);
                    if (!colorLetterCount.ContainsKey(cell.SquareColorLetter))
                    {
                        colorLetterCount[cell.SquareColorLetter] = 0;
                    }

                    colorLetterCount[cell.SquareColorLetter]++;
                }

                // Check for star
                if (cell.StarColorLetter != ' ')
                {
                    starLetters.Add(cell.StarColorLetter);
                    if (!colorLetterCount.ContainsKey(cell.StarColorLetter))
                    {
                        colorLetterCount[cell.StarColorLetter] = 0;
                    }

                    colorLetterCount[cell.StarColorLetter]++;
                }

                // Check for triangle
                if (cell.TriangleCount.HasValue && cell.TriangleCount.Value != cell.UsedEdgeCount)
                {
                    good = false;
                    break;
                }

                // Check for tetris
                if (cell.Tetris != null)
                {
                    tetrisList.Add(cell.Tetris);

                    if (cell.Tetris.ColorLetter != ' ')
                    {
                        if (!colorLetterCount.ContainsKey(cell.Tetris.ColorLetter))
                        {
                            colorLetterCount[cell.Tetris.ColorLetter] = 0;
                        }

                        colorLetterCount[cell.Tetris.ColorLetter]++;
                    }
                }
            }

            // Only 1 square of a color allowed
            if (squareLetters.Count > 1)
            {
                good = false;
            }

            // A star indicates exactly 2 of that color
            foreach (var starLetter in starLetters)
            {
                if (colorLetterCount[starLetter] != 2)
                {
                    good = false;
                }
            }

            // Check tetris
            if (good && tetrisList.Count > 0)
            {
                good &= SectionTetrisChecker.CanContainExactly(this, tetrisList);
            }

            this.Checked = true;
            this.Correct = good;
            return good;
        }

        public List<Section> FindSubSections()
        {
            var sections = new List<Section>();

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
            {
                // Section is intact
                return null;
            }

            sections.Add(new Section(cells, this.cellMax));
            sections.Add(new Section(this.Cells.Where(cell => !visited[cell.CellIndex]), this.cellMax));

            return sections;
        }
    }
}
