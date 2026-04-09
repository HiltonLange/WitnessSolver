namespace WitnessSolver
{
    public static partial class Puzzles
    {
        [PuzzleDefinition(PuzzleCategory.Short)]
        public static Puzzle SamplePuzzle()
        {
            var puzzle = new RectanglePuzzle("Sample puzzle", 4, 4, new Point(0, 4), new Point(4, 0));
            puzzle.ExpectedSolutions = 196;

            puzzle.Cell[0, 1].SquareColorLetter = 'R';
            puzzle.Cell[0, 2].SquareColorLetter = 'R';
            puzzle.Cell[0, 3].SquareColorLetter = 'R';
            puzzle.Cell[1, 2].SquareColorLetter = 'R';
            puzzle.Cell[1, 3].SquareColorLetter = 'R';
            puzzle.Cell[2, 3].SquareColorLetter = 'R';

            puzzle.Cell[2, 2].SquareColorLetter = 'R';

            puzzle.Cell[1, 0].SquareColorLetter = 'B';
            puzzle.Cell[2, 0].SquareColorLetter = 'B';
            puzzle.Cell[3, 0].SquareColorLetter = 'B';
            puzzle.Cell[2, 1].SquareColorLetter = 'B';
            puzzle.Cell[3, 1].SquareColorLetter = 'B';
            puzzle.Cell[3, 2].SquareColorLetter = 'B';

            puzzle.Points[3, 1].OutEdges[puzzle.Points[3, 2]].MayTraverse = false;

            return puzzle;
        }
    }
}



