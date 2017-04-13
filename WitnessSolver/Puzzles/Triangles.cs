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

        public static Puzzle TriangleOptimization()
        {
            var puzzle = new RectanglePuzzle("TriangleOptimization", 6, 5, new Point(0, 5), new Point(6, 0));
            puzzle.ExpectedSolutions = 0;

            puzzle.Cell[0, 4].TriangleCount = 3;
            puzzle.Points[0, 5].OutEdges[puzzle.Points[0, 4]].MustTraverse = true;
            puzzle.Points[0, 4].OutEdges[puzzle.Points[1, 4]].MustTraverse = true;
            puzzle.Points[1, 4].OutEdges[puzzle.Points[2, 4]].MustTraverse = true;
            puzzle.Points[2, 4].OutEdges[puzzle.Points[2, 5]].MustTraverse = true;

            return puzzle;
        }
    }
}
