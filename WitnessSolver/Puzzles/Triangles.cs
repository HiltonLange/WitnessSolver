namespace WitnessSolver
{
    static partial class Puzzles
    {
        public static Puzzle Triangle1()
        {
            var puzzle = new RectanglePuzzle("Triangle1", 1, 2, new Point(0, 2), new Point(1, 0));
            puzzle.ExpectedSolutions = 1;

            puzzle.Cell[0, 1].TriangleCount = 1;

            return puzzle;
        }

        public static Puzzle Triangle2()
        {
            var puzzle = new RectanglePuzzle("Triangle2", 1, 2, new Point(0, 2), new Point(1, 0));
            puzzle.ExpectedSolutions = 2;

            puzzle.Cell[0, 1].TriangleCount = 2;

            return puzzle;
        }
    }
}
