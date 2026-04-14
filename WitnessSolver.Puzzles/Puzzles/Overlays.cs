namespace WitnessSolver
{
    public static partial class Puzzles
    {
        [PuzzleDefinition(PuzzleCategory.Trivial, expectedSolutions: 1)]
        public static Puzzle Overlays()
        {
            var puzzle = new RectanglePuzzle("Overlays", 5, 5, new Point(0, 5), new Point(5, 0));
            puzzle.Cell[0, 0].StarColorLetter = 'P';
            puzzle.Cell[1, 0].StarColorLetter = 'G';
            puzzle.Cell[4, 4].StarColorLetter = 'G';
            puzzle.Cell[3, 4].StarColorLetter = 'P';

            puzzle.Cell[0, 0].SquareColorLetter = 'W';
            puzzle.Cell[2, 4].SquareColorLetter = 'B';

            puzzle.Points[0, 1].OutEdges[puzzle.Points[1, 1]].MustTraverse = true;

            // ..
            var y = -1;
            puzzle.Points[0, 2 + y].OutEdges[puzzle.Points[1, 2 + y]].MustTraverse = true;
            puzzle.Points[0, 4 + y].OutEdges[puzzle.Points[1, 4 + y]].MustTraverse = true;
            puzzle.Points[1, 2 + y].OutEdges[puzzle.Points[1, 3 + y]].MustTraverse = true;
            puzzle.Points[1, 3 + y].OutEdges[puzzle.Points[1, 4 + y]].MustTraverse = true;

            // ...
            // . .
            //   .
            //puzzle.Points[5, 4].OutEdges[puzzle.Points[4, 4]].MustTraverse = true;
            //puzzle.Points[4, 4].OutEdges[puzzle.Points[4, 3]].MustTraverse = true;
            //puzzle.Points[4, 3].OutEdges[puzzle.Points[4, 2]].MustTraverse = true;
            //puzzle.Points[4, 2].OutEdges[puzzle.Points[3, 2]].MustTraverse = true;
            //puzzle.Points[3, 2].OutEdges[puzzle.Points[3, 3]].MustTraverse = true;
            //puzzle.Points[3, 3].OutEdges[puzzle.Points[2, 3]].MustTraverse = true;
            //puzzle.Points[2, 3].OutEdges[puzzle.Points[2, 2]].MustTraverse = true;
            //puzzle.Points[2, 2].OutEdges[puzzle.Points[2, 1]].MustTraverse = true;
            //puzzle.Points[2, 1].OutEdges[puzzle.Points[3, 1]].MustTraverse = true;
            //puzzle.Points[3, 1].OutEdges[puzzle.Points[4, 1]].MustTraverse = true;
            //puzzle.Points[4, 1].OutEdges[puzzle.Points[5, 1]].MustTraverse = true;

            // ...
            // ..
            // .
            //puzzle.Points[5, 2].OutEdges[puzzle.Points[4, 2]].MustTraverse = true;
            //puzzle.Points[4, 2].OutEdges[puzzle.Points[4, 3]].MustTraverse = true;
            //puzzle.Points[4, 3].OutEdges[puzzle.Points[3, 3]].MustTraverse = true;
            //puzzle.Points[3, 3].OutEdges[puzzle.Points[3, 4]].MustTraverse = true;
            //puzzle.Points[3, 4].OutEdges[puzzle.Points[2, 4]].MustTraverse = true;
            //puzzle.Points[2, 4].OutEdges[puzzle.Points[2, 3]].MustTraverse = true;
            //puzzle.Points[2, 3].OutEdges[puzzle.Points[2, 2]].MustTraverse = true;
            //puzzle.Points[2, 2].OutEdges[puzzle.Points[2, 1]].MustTraverse = true;
            //puzzle.Points[2, 1].OutEdges[puzzle.Points[3, 1]].MustTraverse = true;
            //puzzle.Points[3, 1].OutEdges[puzzle.Points[4, 1]].MustTraverse = true;
            //puzzle.Points[4, 1].OutEdges[puzzle.Points[5, 1]].MustTraverse = true;

            //   .
            // ...
            // . .
            puzzle.Points[4, 0].OutEdges[puzzle.Points[4, 1]].MustTraverse = true;
            puzzle.Points[4, 1].OutEdges[puzzle.Points[3, 1]].MustTraverse = true;
            puzzle.Points[3, 1].OutEdges[puzzle.Points[2, 1]].MustTraverse = true;
            puzzle.Points[2, 1].OutEdges[puzzle.Points[2, 2]].MustTraverse = true;
            puzzle.Points[2, 2].OutEdges[puzzle.Points[2, 3]].MustTraverse = true;
            puzzle.Points[2, 3].OutEdges[puzzle.Points[3, 3]].MustTraverse = true;
            puzzle.Points[3, 3].OutEdges[puzzle.Points[3, 2]].MustTraverse = true;
            puzzle.Points[3, 2].OutEdges[puzzle.Points[4, 2]].MustTraverse = true;
            puzzle.Points[4, 2].OutEdges[puzzle.Points[4, 3]].MustTraverse = true;
            puzzle.Points[4, 3].OutEdges[puzzle.Points[5, 3]].MustTraverse = true;

            return puzzle;
        }

    }
}




