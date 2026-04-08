namespace WitnessSolver
{
    public static partial class Puzzles
    {
        public static Puzzle WrapBasic()
        {
            var puzzle = new RectanglePuzzle("WrapBasic", 2, 2, new Point(1, 0), new Point(0, 2));
            puzzle.AddWrap();
            puzzle.ExpectedSolutions = 5;

            puzzle.Cell[2, 0].SquareColorLetter = 'B';
            puzzle.Cell[0, 0].SquareColorLetter = 'B';
            puzzle.Cell[2, 1].TriangleCount = 1;
            puzzle.Points[2, 2].OutEdges[puzzle.Points[0, 2]].MustTraverse = true;

            return puzzle;
        }

        public static Puzzle WrapLarge()
        {
            var puzzle = new RectanglePuzzle("WrapLarge", 5, 6, new Point(0, 6), new Point(0, 0));
            puzzle.ExpectedSolutions = 82;
            puzzle.AddWrap();

            for (int x = 0; x < 6; x++)
            {
                puzzle.Cell[x, 1].TriangleCount = 2;
                puzzle.Cell[x, 3].TriangleCount = 1;
                puzzle.Cell[x, 5].TriangleCount = 2;
            }

            return puzzle;
        }
    }
}

