namespace WitnessSolver
{
    public static partial class Puzzles
    {
        public static Puzzle DistortedColors()
        {
            var puzzle = new RectanglePuzzle("Distorted colors", 6, 6, new Point(0, 6), new Point(6, 0));
            puzzle.ExpectedSolutions = 46;
            puzzle.Cell[0, 0].SquareColorLetter = 'Y';
            puzzle.Cell[0, 1].SquareColorLetter = 'Y';
            puzzle.Cell[0, 2].SquareColorLetter = 'Y';
            puzzle.Cell[0, 3].SquareColorLetter = 'Y';
            puzzle.Cell[0, 4].SquareColorLetter = 'Y';
            puzzle.Cell[0, 5].SquareColorLetter = 'Y';

            puzzle.Cell[1, 0].SquareColorLetter = 'B';
            puzzle.Cell[1, 2].SquareColorLetter = 'B';
            puzzle.Cell[1, 3].SquareColorLetter = 'B';
            puzzle.Cell[1, 5].SquareColorLetter = 'B';

            puzzle.Cell[2, 0].SquareColorLetter = 'B';
            puzzle.Cell[2, 2].SquareColorLetter = 'Y';
            puzzle.Cell[2, 3].SquareColorLetter = 'B';
            puzzle.Cell[2, 4].SquareColorLetter = 'Y';
            puzzle.Cell[2, 5].SquareColorLetter = 'B';

            puzzle.Cell[3, 0].SquareColorLetter = 'Y';
            puzzle.Cell[3, 1].SquareColorLetter = 'Y';
            puzzle.Cell[3, 2].SquareColorLetter = 'Y';
            puzzle.Cell[3, 3].SquareColorLetter = 'Y';
            puzzle.Cell[3, 4].SquareColorLetter = 'Y';
            puzzle.Cell[3, 5].SquareColorLetter = 'B';

            puzzle.Cell[4, 0].SquareColorLetter = 'Y';
            puzzle.Cell[4, 2].SquareColorLetter = 'B';
            puzzle.Cell[4, 3].SquareColorLetter = 'Y';
            puzzle.Cell[4, 5].SquareColorLetter = 'B';

            puzzle.Cell[5, 0].SquareColorLetter = 'Y';
            puzzle.Cell[5, 1].SquareColorLetter = 'Y';
            puzzle.Cell[5, 2].SquareColorLetter = 'B';
            puzzle.Cell[5, 3].SquareColorLetter = 'B';
            puzzle.Cell[5, 4].SquareColorLetter = 'B';
            puzzle.Cell[5, 5].SquareColorLetter = 'B';

            return puzzle;
        }
    }
}

