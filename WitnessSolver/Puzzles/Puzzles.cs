using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace WitnessSolver
{
    static partial class Puzzles
    {
        public static List<Puzzle> AllPuzzles()
        {
            return new List<Puzzle>
            {
                Puzzles.AnotherPuzzle(),
                Puzzles.ComplexBeginning(),
                Puzzles.Flashing(),
                Puzzles.MiddleChurch(),
                Puzzles.NewPuzzle(),
                Puzzles.Overlays(),
                Puzzles.SamplePuzzle(),
                Puzzles.StartShed(),
                Puzzles.Test55(),
            };
        }
    }
}
