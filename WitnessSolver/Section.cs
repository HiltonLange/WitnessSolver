using System.Collections.Generic;

namespace WitnessSolver
{
    class Section
    {
        public readonly HashSet<Cell> Cells;
        public bool Checked;
        public bool Correct;

        public Section(IEnumerable<Cell> cells)
        {
            this.Cells = new HashSet<Cell>(cells);
            this.Checked = false;
        }

        public Section(HashSet<Cell> cells)
        {
            this.Cells = cells;
        }
    }
}
