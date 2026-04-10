using System;

namespace WitnessSolver
{
    public class PuzzleEntry
    {
        public string Name { get; }
        public PuzzleCategory Category { get; }
        public long ExpectedSolutions { get; }
        public Func<Puzzle> Factory { get; }

        public PuzzleEntry(string name, PuzzleCategory category, long expectedSolutions, Func<Puzzle> factory)
        {
            this.Name = name;
            this.Category = category;
            this.ExpectedSolutions = expectedSolutions;
            this.Factory = factory;
        }

        public override string ToString() => this.Name;
    }
}
