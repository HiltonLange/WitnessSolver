namespace WitnessSolver
{
    static partial class Puzzles
    {
        public static Puzzle MiddleChurch()
        {
            var puzzle = new RectanglePuzzle("Middle church", 4, 4, new Point(0, 4), new Point(4, 0));
            puzzle.ExpectedSolutions = 5;

            puzzle.Cell[0, 0].StarColorLetter = 'W';
            puzzle.Cell[1, 0].StarColorLetter = 'W';
            puzzle.Cell[2, 0].StarColorLetter = 'W';
            puzzle.Cell[3, 0].StarColorLetter = 'W';

            puzzle.Cell[0, 1].StarColorLetter = 'W';
            puzzle.Cell[1, 1].StarColorLetter = 'W';
            puzzle.Cell[2, 1].StarColorLetter = 'B';
            puzzle.Cell[3, 1].StarColorLetter = 'R';

            puzzle.Cell[0, 2].StarColorLetter = 'B';
            puzzle.Cell[1, 2].StarColorLetter = 'B';
            puzzle.Cell[2, 2].StarColorLetter = 'B';
            puzzle.Cell[3, 2].StarColorLetter = 'R';

            puzzle.Cell[0, 3].StarColorLetter = 'R';
            puzzle.Cell[1, 3].StarColorLetter = 'R';
            puzzle.Cell[2, 3].StarColorLetter = 'R';
            puzzle.Cell[3, 3].StarColorLetter = 'R';

            return puzzle;
        }
    }
}
