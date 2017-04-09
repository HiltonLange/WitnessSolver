namespace WitnessSolver
{
    static partial class Puzzles
    {
        public static Puzzle Test55()
        {
            var puzzle = new RectanglePuzzle("Test 5x5", 5, 5, new Point(0, 5), new Point(5, 0));

            puzzle.Points[3, 3].MustTraverse = true;
            puzzle.Points[3, 4].MustTraverse = true;
            puzzle.Points[1, 0].OutEdges[puzzle.Points[1, 1]].MustTraverse = true;
            //puzzle.Points[0, 4].OutEdges[puzzle.Points[1, 4]].MustTraverse = true;
            //puzzle.Points[3, 0].OutEdges[puzzle.Points[3, 1]].MustTraverse = true;
            puzzle.Cell[0, 0].StarColorLetter = 'W';
            puzzle.Cell[4, 4].StarColorLetter = 'W';
            puzzle.ExpectedSolutions = 46204;

            return puzzle;
        }
    }
}
