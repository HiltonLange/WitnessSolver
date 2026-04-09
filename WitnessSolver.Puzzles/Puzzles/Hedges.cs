using System.Collections.Generic;

namespace WitnessSolver
{
    public static partial class Puzzles
    {
        public static Puzzle HedgeTetris2()
        {
            var puzzle = new RectanglePuzzle("HedgeTetris2", 4, 4, new Point(4, 4), new Point(0, 0));

            // NE corner: horizontal I-tetromino
            puzzle.Cell[3, 0].Tetris = new Tetris(
                new List<TetrisCell>
                {
                    new TetrisCell(0, 0),
                    new TetrisCell(1, 0),
                    new TetrisCell(2, 0),
                    new TetrisCell(3, 0),
                });

            // Bottom row, 3rd column: L on its back
            puzzle.Cell[2, 3].Tetris = new Tetris(
                new List<TetrisCell>
                {
                    new TetrisCell(0, 0),
                    new TetrisCell(0, 1),
                    new TetrisCell(1, 1),
                    new TetrisCell(2, 1),
                });

            puzzle.Points[3, 1].OutEdges[puzzle.Points[4, 1]].MayTraverse = false;
            puzzle.ExpectedSolutions = 0;

            return puzzle;
        }
    }
}
