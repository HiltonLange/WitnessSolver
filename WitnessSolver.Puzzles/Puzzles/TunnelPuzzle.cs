using System.Collections.Generic;

namespace WitnessSolver
{
    public static partial class Puzzles
    {
        [PuzzleDefinition(PuzzleCategory.Medium)]
        public static Puzzle TunnelPuzzle()
        {
            var puzzle = new RectanglePuzzle("Tunnel puzzle", 5, 5, new Point(0, 5), new Point(5, 0));
            puzzle.ExpectedSolutions = 4;

            puzzle.Cell[0, 0].StarColorLetter = 'G';
            puzzle.Cell[4, 0].StarColorLetter = 'G';
            puzzle.Cell[4, 4].StarColorLetter = 'G';
            puzzle.Cell[1, 1].Tetris = new Tetris(
                new List<TetrisCell>
                {
                    new TetrisCell(0,0),
                    new TetrisCell(0,1),
                    new TetrisCell(1,0),
                    new TetrisCell(1,1),
                });
            puzzle.Cell[1, 1].Tetris.ColorLetter = 'G';

            puzzle.Cell[3, 3].Tetris = new Tetris(
                new List<TetrisCell>
                {
                    new TetrisCell(0,0),
                    new TetrisCell(0,1),
                    new TetrisCell(1,0),
                    new TetrisCell(1,1),
                });
            puzzle.Cell[3, 3].Tetris.ColorLetter = 'G';

            puzzle.Cell[1, 3].Tetris = new Tetris(
                new List<TetrisCell>
                {
                    new TetrisCell(0,0),
                    new TetrisCell(0,1),
                    new TetrisCell(1,0),
                    new TetrisCell(1,1),
                });
            puzzle.Cell[1, 3].Tetris.ColorLetter = 'G';

            puzzle.Cell[3, 1].Tetris = new Tetris(
                new List<TetrisCell>
                {
                    new TetrisCell(0,0),
                    new TetrisCell(0,1),
                    new TetrisCell(1,0),
                    new TetrisCell(1,1),
                });
            puzzle.Cell[3, 1].Tetris.ColorLetter = 'G';

            return puzzle;
        }
    }
}



