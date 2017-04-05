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
                Flashing(),
                MiddleChurch(),
                NewPuzzle(),
                Overlays(),
                SamplePuzzle(),
                StartShed(),
                Test55(),
            };
        }
    }
}
