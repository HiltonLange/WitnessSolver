using System.Collections.Generic;
using System.Linq;

namespace WitnessSolver
{
    class Section
    {
        public readonly HashSet<Cell> Cells;
        public bool Checked;
        public bool Correct;
        private readonly bool[] containsCell;
        internal readonly int cellMax;

        public Section(IEnumerable<Cell> cells, int cellMax)
        {
            this.Cells = new HashSet<Cell>(cells);
            this.Checked = false;
            this.containsCell = new bool[cellMax];
            this.cellMax = cellMax;
            this.CalculateContainedCells();
        }

        public Section(HashSet<Cell> cells, int cellMax)
        {
            this.Cells = cells;
            this.Checked = false;
            this.containsCell = new bool[cellMax];
            this.cellMax = cellMax;
            this.CalculateContainedCells();
        }

        public void UnionWith(Section section)
        {
            this.Cells.UnionWith(section.Cells);
            foreach (var cell in section.Cells)
            {
                this.containsCell[cell.CellIndex] = true;
            }
            this.Checked = false;
        }

        public bool ContainsCell(Cell cell)
        {
            if (cell == null)
            {
                return false;
            }

            return this.containsCell[cell.CellIndex];
        }

        public void CalculateContainedCells()
        {
            foreach (var cell in this.Cells)
            {
                this.containsCell[cell.CellIndex] = true;
            }
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
