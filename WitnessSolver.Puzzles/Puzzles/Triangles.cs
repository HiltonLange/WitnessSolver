namespace WitnessSolver
{
    public static partial class Puzzles
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

        public static Puzzle TriangleSectionTrap()
        {
            var puzzle = new RectanglePuzzle("TriangleSectionTrap", 3, 2, new Point(3, 1), new Point(1, 1));
            puzzle.ExpectedSolutions = 1;

            puzzle.Cell[1, 0].TriangleCount = 1;
            puzzle.Points[0, 1].OutEdges[puzzle.Points[1, 1]].MayTraverse = false;
            puzzle.Points[0, 2].OutEdges[puzzle.Points[1, 2]].MayTraverse = false;
            puzzle.Points[1, 0].OutEdges[puzzle.Points[1, 1]].MayTraverse = false;
            puzzle.Points[2, 0].OutEdges[puzzle.Points[2, 1]].MayTraverse = false;
            puzzle.Points[3, 0].OutEdges[puzzle.Points[3, 1]].MayTraverse = false;
            puzzle.Points[2, 1].OutEdges[puzzle.Points[2, 2]].MayTraverse = false;

            return puzzle;
        }
    }
}

