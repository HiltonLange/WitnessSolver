using System.Collections.Generic;

namespace WitnessSolver
{
    class Section
    {
        public readonly HashSet<Cell> Cells;
        public bool Checked;
        public bool Correct;
        private bool[] containsCell;

        public Section(IEnumerable<Cell> cells, int puzzleCellCount)
        {
            this.Cells = new HashSet<Cell>(cells);
            this.Checked = false;
            containsCell = new bool[puzzleCellCount];
            this.CalculateContainedCells();
        }

        public Section(HashSet<Cell> cells, int puzzleCellCount)
        {
            this.Cells = cells;
            this.Checked = false;
            containsCell = new bool[puzzleCellCount];
            this.CalculateContainedCells();
        }

        public void UnionWith(IEnumerable<Cell> cells)
        {
            this.Cells.UnionWith(cells);
            foreach (var cell in cells)
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
                containsCell[cell.CellIndex] = true;
            }
        }
    }
}
