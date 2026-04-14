using System;

namespace WitnessSolver
{
    public class PuzzleEntry
    {
        public string Name { get; }
        public PuzzleCategory Category { get; }
        public long ExpectedSolutions { get; }
        public bool HasExpectedSolutions { get; }
        public Func<Puzzle> Factory { get; }

        public PuzzleEntry(string name, PuzzleCategory category, long expectedSolutions, bool hasExpectedSolutions, Func<Puzzle> factory)
        {
            this.Name = name;
            this.Category = category;
            this.ExpectedSolutions = expectedSolutions;
            this.HasExpectedSolutions = hasExpectedSolutions;
            this.Factory = factory;
        }

        public override string ToString() => this.Name;
    }
}
