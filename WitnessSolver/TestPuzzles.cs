namespace WitnessSolver
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class TestPuzzles
    {

        [TestMethod]
        private void TestAnotherPuzzle()
        {
            var puzzle = Puzzles.AnotherPuzzle();
            PuzzleSolver solver = new PuzzleSolver()
            {
                Puzzle = puzzle,
            };

            int solutions = solver.Solve();
            Assert.AreEqual(puzzle.ExpectedSolutions, solutions);
        }

        [TestMethod]
        public void TestFlashing()
        {
            var puzzle = Puzzles.Flashing();
            PuzzleSolver solver = new PuzzleSolver()
            {
                Puzzle = puzzle,
            };

            int solutions = solver.Solve();
            Assert.AreEqual(puzzle.ExpectedSolutions, solutions);
        }

        [TestMethod]
        public void TestMiddleChurch()
        {
            var puzzle = Puzzles.MiddleChurch();
            PuzzleSolver solver = new PuzzleSolver()
            {
                Puzzle = puzzle,
            };

            int solutions = solver.Solve();
            Assert.AreEqual(puzzle.ExpectedSolutions, solutions);
        }

        [TestMethod]
        public void TestNewPuzzle()
        {
            var puzzle = Puzzles.NewPuzzle();
            PuzzleSolver solver = new PuzzleSolver()
            {
                Puzzle = puzzle,
            };

            int solutions = solver.Solve();
            Assert.AreEqual(puzzle.ExpectedSolutions, solutions);
        }

        [TestMethod]
        public void TestOverlays()
        {
            var puzzle = Puzzles.Overlays();
            PuzzleSolver solver = new PuzzleSolver()
            {
                Puzzle = puzzle,
            };

            int solutions = solver.Solve();
            Assert.AreEqual(puzzle.ExpectedSolutions, solutions);
        }

        [TestMethod]
        public void TestSamplePuzzle()
        {
            var puzzle = Puzzles.SamplePuzzle();
            PuzzleSolver solver = new PuzzleSolver()
            {
                Puzzle = puzzle,
            };

            int solutions = solver.Solve();
            Assert.AreEqual(puzzle.ExpectedSolutions, solutions);
        }

        [TestMethod]
        public void TestStartShed()
        {
            var puzzle = Puzzles.StartShed();
            PuzzleSolver solver = new PuzzleSolver()
            {
                Puzzle = puzzle,
            };

            int solutions = solver.Solve();
            Assert.AreEqual(puzzle.ExpectedSolutions, solutions);
        }

        [TestMethod]
        public void TestTest55()
        {
            var puzzle = Puzzles.Test55();
            PuzzleSolver solver = new PuzzleSolver()
            {
                Puzzle = puzzle,
            };

            int solutions = solver.Solve();
            Assert.AreEqual(puzzle.ExpectedSolutions, solutions);
        }

        [TestMethod]
        [TestCategory("Long")]
        public void TestComplexBeginning()
        {
            var puzzle = Puzzles.ComplexBeginning();
            PuzzleSolver solver = new PuzzleSolver()
            {
                Puzzle = puzzle,
            };

            int solutions = solver.Solve();
            Assert.AreEqual(puzzle.ExpectedSolutions, solutions);
        }
    }
}
