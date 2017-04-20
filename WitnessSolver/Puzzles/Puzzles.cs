using System.Collections.Generic;

namespace WitnessSolver
{
    static partial class Puzzles
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
                TetrisSimple(),
                TetrisCombine(),
                TetrisComplex(),
                TetrisBasicNegative(),
                TetrisBasicNegative2(),
                Triangle1(),
                Triangle2(),
                TriangleOptimization(),
                TriangleSectionTrap(),
                WrapBasic(),
                WrapLarge(),
                TunnelPuzzle(),
                TunnelTetrisPuzzle(),
            };
        }
    }
}
