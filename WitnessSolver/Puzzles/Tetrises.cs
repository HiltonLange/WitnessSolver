using System.Collections.Generic;

namespace WitnessSolver
{
    static partial class Puzzles
    {
        public static Puzzle TetrisSimple()
        {
            var puzzle = new RectanglePuzzle("TetrisSimple", 5, 4, new Point(0, 4), new Point(5, 0));
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

        public static Puzzle TetrisCombine()
        {
            var puzzle = new RectanglePuzzle("TetrisCombine", 4, 5, new Point(0, 5), new Point(4, 0));
            puzzle.ExpectedSolutions = 3;

            puzzle.Cell[1, 3].Tetris = new Tetris(
                new List<TetrisCell>()
                {
                    new TetrisCell(0, 0),
                    new TetrisCell(0, 1),
                    new TetrisCell(0, 2),
                });

            puzzle.Cell[2, 0].Tetris = new Tetris(
                new List<TetrisCell>()
                {
                    new TetrisCell(0, 0),
                    new TetrisCell(0, 1),
                });

            return puzzle;
        }

        public static Puzzle TetrisComplex()
        {
            var puzzle = new RectanglePuzzle("TetrisComplex", 4, 5, new Point(0, 5), new Point(4, 0));
            puzzle.ExpectedSolutions = 246;

            puzzle.Cell[0, 2].Tetris = new Tetris(
                new List<TetrisCell>()
                {
                    new TetrisCell(0, 0),
                    new TetrisCell(0, 1),
                });

            puzzle.Cell[2, 4].Tetris = new Tetris(
                new List<TetrisCell>()
                {
                    new TetrisCell(0, 0),
                    new TetrisCell(1, 0),
                    new TetrisCell(2, 0),
                });

            return puzzle;
        }

        public static Puzzle TetrisBasicNegative()
        {
            var puzzle = new RectanglePuzzle("TetrisBasicNegative", 4, 4, new Point(0, 4), new Point(4, 0));
            puzzle.ExpectedSolutions = 116;

            puzzle.Cell[3, 3].Tetris = new Tetris(
                new List<TetrisCell>()
                {
                    new TetrisCell(2, 1),
                    new TetrisCell(2, 0),
                    new TetrisCell(1, 1),
                    new TetrisCell(0, 1),
                });

            puzzle.Cell[2, 3].Tetris = new Tetris(
                new List<TetrisCell>()
                {
                    new TetrisCell(0, 0, -1),
                });

            return puzzle;
        }

        public static Puzzle TetrisBasicNegative2()
        {
            var puzzle = new RectanglePuzzle("TetrisBasicNegative2", 4, 4, new Point(0, 4), new Point(4, 0));
            puzzle.ExpectedSolutions = 4;

            puzzle.Cell[0, 1].Tetris = new Tetris(
                new List<TetrisCell>()
                {
                    new TetrisCell(0, 0),
                    new TetrisCell(0, 1),
                    new TetrisCell(0, 2),
                });

            puzzle.Cell[2, 3].Tetris = new Tetris(
                new List<TetrisCell>()
                {
                    new TetrisCell(0, 0),
                    new TetrisCell(1, 0),
                    new TetrisCell(2, 0),
                });

            puzzle.Cell[2, 1].Tetris = new Tetris(
                new List<TetrisCell>()
                {
                    new TetrisCell(0, 0, -1),
                });

            return puzzle;
        }
    }
}
