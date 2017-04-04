using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WitnessSolver
{
    static partial class Puzzles
    {
        public static Puzzle Flashing()
        {
            RectanglePuzzle puzzle = new RectanglePuzzle("Flashing", 5, 5, new Point(0, 5), new Point(5, 0));

            puzzle.Cell[4, 0].StarColorLetter = 'B';
            puzzle.Cell[0, 4].StarColorLetter = 'Y';

            puzzle.Cell[0, 2].SquareColorLetter = 'Y';
            puzzle.Cell[0, 3].SquareColorLetter = 'B';
            puzzle.Cell[2, 1].SquareColorLetter = 'Y';
            puzzle.Cell[4, 3].SquareColorLetter = 'Y';
            puzzle.Cell[4, 4].SquareColorLetter = 'B';

            return puzzle;
        }
    }
}
