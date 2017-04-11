using System.Collections.Generic;

namespace WitnessSolver
{
    class Section
    {
        public readonly HashSet<Cell> Cells;
        public bool Checked;
        public bool Correct;
        private readonly bool[] containsCell;

        public Section(IEnumerable<Cell> cells, int puzzleCellCount)
        {
            this.Cells = new HashSet<Cell>(cells);
            this.Checked = false;
            this.containsCell = new bool[puzzleCellCount];
            this.CalculateContainedCells();
        }

        public Section(HashSet<Cell> cells, int puzzleCellCount)
        {
            this.Cells = cells;
            this.Checked = false;
            this.containsCell = new bool[puzzleCellCount];
            this.CalculateContainedCells();
        }

        public void UnionWith(IEnumerable<Cell> cells)
        {
            foreach (var cell in cells)
            {
                this.Cells.Add(cell);
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
    }
}
