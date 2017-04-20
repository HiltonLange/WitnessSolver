using System.Collections.Generic;

namespace WitnessSolver
{
    static partial class Puzzles
    {
        public static Puzzle Tetris1()
        {
            var puzzle = new RectanglePuzzle("Tetris1", 5, 4, new Point(0, 4), new Point(5, 0));
            puzzle.ExpectedSolutions = 10;

            puzzle.Cell[4, 3].Tetris = new Tetris(
                new List<TetrisCell>()
                {
                    new TetrisCell(1, 0),
                    new TetrisCell(0, 1),
                    new TetrisCell(1, 1),
                });

            puzzle.Cell[0,0].Tetris = new Tetris(
                new List<TetrisCell>()
                {
                    new TetrisCell(0, 0),
                    new TetrisCell(0, 1),
                    new TetrisCell(1, 0),
                });

            return puzzle;
        }
    }
}
