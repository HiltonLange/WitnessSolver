namespace WitnessSolver
{
    public static partial class Puzzles
    {
        [PuzzleDefinition(PuzzleCategory.Trivial, expectedSolutions: 1)]
        public static Puzzle StartShed()
        {
            var puzzle = new RectanglePuzzle("Start shed", 3, 3, new Point(3, 0), new Point(0, 1));
            puzzle.Points[0, 2].MustTraverse = true;
            puzzle.Points[1, 2].MustTraverse = true;
            puzzle.Points[1, 0].MustTraverse = true;
            puzzle.Points[2, 1].MustTraverse = true;
            puzzle.Points[3, 1].MustTraverse = true;
            puzzle.Points[3, 3].MustTraverse = true;

            puzzle.Points[0, 0].OutEdges[puzzle.Points[1, 0]].MayTraverse = false;
            puzzle.Points[0, 0].OutEdges[puzzle.Points[0, 1]].MayTraverse = false;
            puzzle.Points[2, 1].OutEdges[puzzle.Points[3, 1]].MayTraverse = false;
            puzzle.Points[2, 2].OutEdges[puzzle.Points[2, 3]].MayTraverse = false;

            return puzzle;
        }
    }
}




