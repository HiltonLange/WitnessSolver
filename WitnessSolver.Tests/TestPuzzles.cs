namespace WitnessSolver
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class TestPuzzles
    {
        [TestMethod]
        public void TestFlashing()
        {
            TestPuzzle(Puzzles.Flashing());
        }

        [TestMethod]
        public void TestMiddleChurch()
        {
            TestPuzzle(Puzzles.MiddleChurch());
        }

        [TestMethod]
        public void TestNewPuzzle()
        {
            TestPuzzle(Puzzles.NewPuzzle());
        }

        [TestMethod]
        public void TestOverlays()
        {
            TestPuzzle(Puzzles.Overlays());
        }

        [TestMethod]
        public void TestSamplePuzzle()
        {
            TestPuzzle(Puzzles.SamplePuzzle());
        }

        [TestMethod]
        public void TestStartShed()
        {
            TestPuzzle(Puzzles.StartShed());
        }

        [TestMethod]
        public void TestTest55()
        {
            TestPuzzle(Puzzles.Test55());
        }

        [TestMethod]
        public void TestDistortedColors()
        {
            TestPuzzle(Puzzles.DistortedColors());
        }

        [TestMethod]
        public void TestTriangle1()
        {
            TestPuzzle(Puzzles.Triangle1());
        }

        [TestMethod]
        public void TestTriangle2()
        {
            TestPuzzle(Puzzles.Triangle2());
        }

        [TestMethod]
        public void TestTriangleOptimization()
        {
            TestPuzzle(Puzzles.TriangleOptimization());
        }

        [TestMethod]
        public void TestTriangleSectionTrap()
        {
            TestPuzzle(Puzzles.TriangleSectionTrap());
        }

        [TestMethod]
        public void TestWrapBasic()
        {
            TestPuzzle(Puzzles.WrapBasic());
        }

        [TestMethod]
        public void TestTetrisSimple()
        {
            TestPuzzle(Puzzles.TetrisSimple());
        }

        [TestMethod]
        public void TestTetrisCombine()
        {
            TestPuzzle(Puzzles.TetrisCombine());
        }

        [TestMethod]
        public void TestTetrisComplex()
        {
            TestPuzzle(Puzzles.TetrisComplex());
        }

        [TestMethod]
        public void TestTetrisBasicNegative()
        {
            TestPuzzle(Puzzles.TetrisBasicNegative());
        }

        [TestMethod]
        public void TestTetrisBasicNegative2()
        {
            TestPuzzle(Puzzles.TetrisBasicNegative2());
        }

        [TestMethod]
        public void TestTunnelPuzzle()
        {
            TestPuzzle(Puzzles.TunnelPuzzle());
        }

        [TestMethod]
        public void TestTetrisRotation()
        {
            TestPuzzle(Puzzles.TetrisRotationPuzzle());
        }

        [TestMethod]
        public void TestTetris3And3()
        {
            TestPuzzle(Puzzles.Tetris3And3());
        }

        [TestMethod]
        public void TestTunnelTetris()
        {
            TestPuzzle(Puzzles.TunnelTetrisPuzzle());
        }

        [TestMethod]
        public void TestTetrisCross()
        {
            TestPuzzle(Puzzles.TetrisCross());
        }

        [TestMethod]
        [TestCategory("Long")]
        public void TestWrapLarge()
        {
            TestPuzzle(Puzzles.WrapLarge());
        }

        [TestMethod]
        [TestCategory("Long")]
        public void TestComplexBeginning()
        {
            TestPuzzle(Puzzles.ComplexBeginning());
        }

        private void TestPuzzle(Puzzle puzzle)
        {
            var graph = SolverGraph.Compile(puzzle);
            var solver = new PuzzleSolver(graph);
            int solutions = solver.Solve();
            Assert.AreEqual(puzzle.ExpectedSolutions, solutions);
        }
    }
}
