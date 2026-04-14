using System.Collections.Generic;
using System.Linq;

namespace WitnessSolver
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class TestPuzzles
    {
        [TestMethod]
        [DynamicData(nameof(TrivialAndShortPuzzles))]
        public void SolvePuzzle(string name, PuzzleEntry entry)
        {
            var puzzle = entry.Factory();
            var graph = SolverGraph.Compile(puzzle);
            var solver = new PuzzleSolver(graph);
            int solutions = solver.Solve();

            if (entry.HasExpectedSolutions)
                Assert.AreEqual(entry.ExpectedSolutions, solutions, $"Puzzle '{name}' solution count mismatch");
            else
                Assert.IsTrue(solutions >= 0, $"Puzzle '{name}' returned negative solution count");
        }

        public static IEnumerable<object[]> TrivialAndShortPuzzles
            => PuzzleCatalog.Below(PuzzleCategory.Medium).Select(e => new object[] { e.Name, e });
    }
}
