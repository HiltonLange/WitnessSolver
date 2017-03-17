using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WitnessSolver
{
    class Section
    {
        public HashSet<Cell> Cells;
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
