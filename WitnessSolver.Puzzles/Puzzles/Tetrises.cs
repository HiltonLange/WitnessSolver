using System.Collections.Generic;

namespace WitnessSolver
{
    public static partial class Puzzles
    {
        [PuzzleDefinition(PuzzleCategory.Medium, expectedSolutions: 10)]
        public static Puzzle TetrisSimple()
        {
            var puzzle = new RectanglePuzzle("TetrisSimple", 5, 4, new Point(0, 4), new Point(5, 0));
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

        [PuzzleDefinition(PuzzleCategory.Medium, expectedSolutions: 3)]
        public static Puzzle TetrisCombine()
        {
            var puzzle = new RectanglePuzzle("TetrisCombine", 4, 5, new Point(0, 5), new Point(4, 0));
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

        [PuzzleDefinition(PuzzleCategory.Medium, expectedSolutions: 246)]
        public static Puzzle TetrisComplex()
        {
            var puzzle = new RectanglePuzzle("TetrisComplex", 4, 5, new Point(0, 5), new Point(4, 0));
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

        [PuzzleDefinition(PuzzleCategory.Medium, expectedSolutions: 116)]
        public static Puzzle TetrisBasicNegative()
        {
            var puzzle = new RectanglePuzzle("TetrisBasicNegative", 4, 4, new Point(0, 4), new Point(4, 0));
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

        [PuzzleDefinition(PuzzleCategory.Medium, expectedSolutions: 4)]
        public static Puzzle TetrisBasicNegative2()
        {
            var puzzle = new RectanglePuzzle("TetrisBasicNegative2", 4, 4, new Point(0, 4), new Point(4, 0));
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

        [PuzzleDefinition(PuzzleCategory.Medium, expectedSolutions: 88)]
        public static Puzzle TetrisRotationPuzzle()
        {
            var puzzle = new RectanglePuzzle("Tetris rotation", 4, 5, new Point(0, 5), new Point(4, 0));
            puzzle.Cell[3, 4].Tetris = new Tetris(
                new List<TetrisCell>
                {
                    new TetrisCell(0,0),
                    new TetrisCell(0,1),
                    new TetrisCell(1,0),
                },
                anyRotation: true);

            puzzle.Cell[2, 1].Tetris = new Tetris(
                new List<TetrisCell>
                {
                    new TetrisCell(0,0),
                    new TetrisCell(0,1),
                    new TetrisCell(1,0),
                },
                anyRotation: true);

            return puzzle;
        }

        [PuzzleDefinition(PuzzleCategory.Medium, expectedSolutions: 116)]
        public static Puzzle TunnelTetrisPuzzle()
        {
            var puzzle = new RectanglePuzzle("Tunnel tetris puzzle", 5, 5, new Point(0, 5), new Point(5, 0));
            puzzle.Cell[0, 0].Tetris = new Tetris(
                new List<TetrisCell>
                {
                    new TetrisCell(0,0),
                },
                anyRotation: true);

            puzzle.Cell[0, 2].Tetris = new Tetris(
                new List<TetrisCell>
                {
                    new TetrisCell(1,0),
                    new TetrisCell(0,1),
                    new TetrisCell(1,2),
                },
                anyRotation: true);

            puzzle.Cell[4, 2].Tetris = new Tetris(
                new List<TetrisCell>
                {
                    new TetrisCell(1,0),
                    new TetrisCell(0,1),
                    new TetrisCell(1,2),
                },
                anyRotation: true);

            puzzle.Cell[2, 4].Tetris = new Tetris(
                new List<TetrisCell>
                {
                    new TetrisCell(0,0),
                    new TetrisCell(0,1),
                    new TetrisCell(0,2),
                    new TetrisCell(1,2),
                },
                anyRotation: true);

            puzzle.Points[2, 1].OutEdges[puzzle.Points[2, 2]].MayTraverse = false;

            return puzzle;
        }

        [PuzzleDefinition(PuzzleCategory.Short, expectedSolutions: 1)]
        public static Puzzle Tetris3And3()
        {
            var puzzle = new RectanglePuzzle("Tetris3And3", 4, 4, new Point(0, 4), new Point(4, 0));
            puzzle.Cell[0, 1].Tetris = new Tetris(
                new List<TetrisCell>
                {
                    new TetrisCell(0,0),
                    new TetrisCell(0,1),
                    new TetrisCell(0,2),
                },
                anyRotation: true);

            puzzle.Cell[3, 1].Tetris = new Tetris(
                new List<TetrisCell>
                {
                    new TetrisCell(0,0),
                    new TetrisCell(0,1),
                    new TetrisCell(0,2),
                },
                anyRotation: true);

            for (int x = 0; x <= 4; x++)
            {
                for (int y = 0; y <= 4; y++)
                {
                    puzzle.Points[x, y].MustTraverse = true;
                }
            }

            puzzle.Points[0, 4].MustTraverse = false;
            puzzle.Points[4, 0].MustTraverse = false;

            return puzzle;
        }

        [PuzzleDefinition(PuzzleCategory.Medium, expectedSolutions: 126)]
        public static Puzzle TetrisCross()
        {
            var puzzle = new RectanglePuzzle("TetrisCross", 5, 5, new Point(0, 5), new Point(5, 0));
            puzzle.Cell[2, 0].Tetris = new Tetris(
                new List<TetrisCell>
                {
                    new TetrisCell(0,0),
                },
                anyRotation: false);

            puzzle.Cell[0, 2].Tetris = new Tetris(
                new List<TetrisCell>
                {
                    new TetrisCell(1,0),
                    new TetrisCell(0,1),
                    new TetrisCell(1,2),
                },
                anyRotation: true);

            puzzle.Cell[4, 2].Tetris = new Tetris(
                new List<TetrisCell>
                {
                    new TetrisCell(1,0),
                    new TetrisCell(0,1),
                    new TetrisCell(1,2),
                },
                anyRotation: true);

            puzzle.Cell[2, 4].Tetris = new Tetris(
                new List<TetrisCell>
                {
                    new TetrisCell(0,0),
                    new TetrisCell(0,1),
                    new TetrisCell(0,2),
                    new TetrisCell(1,2),
                    new TetrisCell(2,2),
                },
                anyRotation: true);

            return puzzle;
        }

        [PuzzleDefinition(PuzzleCategory.Medium, expectedSolutions: 1)]
        public static Puzzle SwampyBoots()
        {
            var puzzle = new RectanglePuzzle("Swampy Boots", 4, 4, new Point(0, 4), new Point(4, 0));
            puzzle.Cell[0, 0].Tetris = new Tetris(
                new List<TetrisCell>
                {
                    new TetrisCell(0,0),
                    new TetrisCell(1,0),
                    new TetrisCell(2,0),
                    new TetrisCell(2,1),
                    new TetrisCell(2,2),
                },
                anyRotation: false);
            puzzle.Cell[3, 1].Tetris = new Tetris(
                new List<TetrisCell>
                {
                    new TetrisCell(0,0),
                    new TetrisCell(0,1),
                    new TetrisCell(0,2),
                });
            puzzle.Cell[1, 2].Tetris = new Tetris(
                new List<TetrisCell>
                {
                    new TetrisCell(0,0),
                    new TetrisCell(0,1),
                    new TetrisCell(3,0),
                    new TetrisCell(3,1),
                });
            return puzzle;
        }
    }
}





