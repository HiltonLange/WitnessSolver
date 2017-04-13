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
                Triangle1(),
                Triangle2(),
                WrapBasic(),
                WrapLarge(),
            };
        }
    }
}
