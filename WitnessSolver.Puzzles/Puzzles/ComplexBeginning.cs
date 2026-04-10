namespace WitnessSolver
{
    public static partial class Puzzles
    {
        [PuzzleDefinition(PuzzleCategory.Long, expectedSolutions: 321102)]
        public static Puzzle ComplexBeginning()
        {
            Point[] starts =
            {
                new Point(0,7),
                new Point(2,5),
                new Point(4,2),
                new Point(6,4),
            };

            Point[] ends =
            {
                new Point(0, 0),
                new Point(7, 0),
                new Point(7, 7),
            };

            var puzzle = new RectanglePuzzle("Complex beginning", 7, 7, starts[3], ends[0]);
            puzzle.Cell[0, 0].SquareColorLetter = 'L';
            puzzle.Cell[0, 1].SquareColorLetter = 'W';
            puzzle.Cell[1, 0].SquareColorLetter = 'W';
            puzzle.Cell[2, 4].SquareColorLetter = 'L';
            puzzle.Cell[2, 5].SquareColorLetter = 'W';
            puzzle.Cell[3, 1].SquareColorLetter = 'L';
            puzzle.Cell[3, 2].SquareColorLetter = 'W';
            puzzle.Cell[4, 1].SquareColorLetter = 'L';
            puzzle.Cell[4, 2].SquareColorLetter = 'W';
            puzzle.Cell[5, 0].SquareColorLetter = 'L';
            puzzle.Cell[5, 6].SquareColorLetter = 'W';
            puzzle.Cell[6, 0].SquareColorLetter = 'W';
            puzzle.Cell[6, 5].SquareColorLetter = 'L';
            puzzle.Cell[6, 6].SquareColorLetter = 'L';

            puzzle.Points[0, 7].OutEdges[puzzle.Points[0, 6]].MustTraverse = true;
            puzzle.Points[0, 7].OutEdges[puzzle.Points[1, 7]].MustTraverse = true;
            puzzle.Points[1, 5].OutEdges[puzzle.Points[2, 5]].MustTraverse = true;
            puzzle.Points[5, 7].OutEdges[puzzle.Points[6, 7]].MustTraverse = true;
            puzzle.Points[7, 0].OutEdges[puzzle.Points[6, 0]].MustTraverse = true;
            puzzle.Points[7, 0].OutEdges[puzzle.Points[7, 1]].MustTraverse = true;
            puzzle.Points[7, 4].OutEdges[puzzle.Points[6, 4]].MustTraverse = true;
            puzzle.Points[7, 4].OutEdges[puzzle.Points[7, 3]].MustTraverse = true;

            puzzle.Points[ends[1].X, ends[1].Y].End = true;
            puzzle.Points[ends[2].X, ends[2].Y].End = true;

            //puzzle.Cell[0, 2].StarColorLetter = 'R';

            return puzzle;
        }

    }
}




