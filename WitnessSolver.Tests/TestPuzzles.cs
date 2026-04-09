using System.Collections.Generic;
using System.Linq;

namespace WitnessSolver
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class TestPuzzles
    {
        [AssemblyInitialize]
        public static void Init(TestContext _)
        {
            PuzzleLibrary.RegisterAssembly(typeof(Puzzles).Assembly);
        }

        [TestMethod]
        [DynamicData(nameof(NormalAndLongPuzzles))]
        public void SolvePuzzle(string name, PuzzleEntry entry)
        {
            var puzzle = entry.Factory();
            var graph = SolverGraph.Compile(puzzle);
            var solver = new PuzzleSolver(graph);
            int solutions = solver.Solve();
            Assert.AreEqual(puzzle.ExpectedSolutions, solutions, $"Puzzle '{name}' solution count mismatch");
        }

        public static IEnumerable<object[]> NormalAndLongPuzzles
            => PuzzleLibrary.Below(PuzzleCategory.Medium).Select(e => new object[] { e.Name, e });
    }
}
