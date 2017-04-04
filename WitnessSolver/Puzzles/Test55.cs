using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WitnessSolver
{
    static partial class Puzzles
    {
        public static Puzzle Test55()
        {
            RectanglePuzzle puzzle = new RectanglePuzzle("Test 5x5", 5, 5, new Point(0, 5), new Point(5, 0));

            return puzzle;
        }
    }
}
