namespace WitnessSolver
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class TestPuzzles
    {
        [TestMethod] public void TestFlashing() => Solve(Puzzles.Flashing());
        [TestMethod] public void TestMiddleChurch() => Solve(Puzzles.MiddleChurch());
        [TestMethod] public void TestNewPuzzle() => Solve(Puzzles.NewPuzzle());
        [TestMethod] public void TestOverlays() => Solve(Puzzles.Overlays());
        [TestMethod] public void TestSamplePuzzle() => Solve(Puzzles.SamplePuzzle());
        [TestMethod] public void TestStartShed() => Solve(Puzzles.StartShed());
        [TestMethod] public void TestTest55() => Solve(Puzzles.Test55());
        [TestMethod] public void TestDistortedColors() => Solve(Puzzles.DistortedColors());
        [TestMethod] public void TestTriangle1() => Solve(Puzzles.Triangle1());
        [TestMethod] public void TestTriangle2() => Solve(Puzzles.Triangle2());
        [TestMethod] public void TestTriangleOptimization() => Solve(Puzzles.TriangleOptimization());
        [TestMethod] public void TestTriangleSectionTrap() => Solve(Puzzles.TriangleSectionTrap());
        [TestMethod] public void TestWrapBasic() => Solve(Puzzles.WrapBasic());
        [TestMethod] public void TestTetrisSimple() => Solve(Puzzles.TetrisSimple());
        [TestMethod] public void TestTetrisCombine() => Solve(Puzzles.TetrisCombine());
        [TestMethod] public void TestTetrisComplex() => Solve(Puzzles.TetrisComplex());
        [TestMethod] public void TestTetrisBasicNegative() => Solve(Puzzles.TetrisBasicNegative());
        [TestMethod] public void TestTetrisBasicNegative2() => Solve(Puzzles.TetrisBasicNegative2());
        [TestMethod] public void TestTunnelPuzzle() => Solve(Puzzles.TunnelPuzzle());
        [TestMethod] public void TestTetrisRotation() => Solve(Puzzles.TetrisRotationPuzzle());
        [TestMethod] public void TestTetris3And3() => Solve(Puzzles.Tetris3And3());
        [TestMethod] public void TestTunnelTetris() => Solve(Puzzles.TunnelTetrisPuzzle());
        [TestMethod] public void TestTetrisCross() => Solve(Puzzles.TetrisCross());
        [TestMethod] public void TestHedgeTetris2() => Solve(Puzzles.HedgeTetris2());

        [TestMethod, TestCategory("Long")] public void TestWrapLarge() => Solve(Puzzles.WrapLarge());
        [TestMethod, TestCategory("Long")] public void TestComplexBeginning() => Solve(Puzzles.ComplexBeginning());
        [TestMethod, TestCategory("Long")] public void TestAnotherPuzzle() => Solve(Puzzles.AnotherPuzzle());

        private static void Solve(Puzzle puzzle)
        {
            var graph = SolverGraph.Compile(puzzle);
            var solver = new PuzzleSolver(graph);
            int solutions = solver.Solve();
            Assert.AreEqual(puzzle.ExpectedSolutions, solutions, $"Puzzle '{puzzle.Name}' solution count mismatch");
        }
    }
}
