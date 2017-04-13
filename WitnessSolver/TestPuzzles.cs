namespace WitnessSolver
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class TestPuzzles
    {

        [TestMethod]
        private void TestAnotherPuzzle()
        {
            TestPuzzle(Puzzles.AnotherPuzzle());
        }

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
        [TestCategory("Long")]
        public void TestComplexBeginning()
        {
            TestPuzzle(Puzzles.ComplexBeginning());
        }

        private void TestPuzzle(Puzzle puzzle)
        {
            PuzzleSolver solver = new PuzzleSolver()
            {
                Puzzle = puzzle,
            };

            int solutions = solver.Solve();
            Assert.AreEqual(puzzle.ExpectedSolutions, solutions);
        }
    }
}
