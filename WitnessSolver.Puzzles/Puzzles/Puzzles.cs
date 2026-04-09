using System.Collections.Generic;

namespace WitnessSolver
{
    public static partial class Puzzles
    {
        public static List<Puzzle> AllPuzzles()
        {
            return new List<Puzzle>
            {
                AnotherPuzzle(),
                ComplexBeginning(),
                DistortedColors(),
                Flashing(),
                MiddleChurch(),
                NewPuzzle(),
                Overlays(),
                SamplePuzzle(),
                StartShed(),
                Test55(),
                Tetris3And3(),
                TetrisSimple(),
                TetrisCombine(),
                TetrisComplex(),
                TetrisBasicNegative(),
                TetrisBasicNegative2(),
                TetrisRotationPuzzle(),
                Triangle1(),
                Triangle2(),
                TriangleOptimization(),
                TriangleSectionTrap(),
                WrapBasic(),
                WrapLarge(),
                TunnelPuzzle(),
                TunnelTetrisPuzzle(),
                TetrisCross(),
                SwampyBoots(),
                HedgeTetris2(),
            };
        }
    }
}

