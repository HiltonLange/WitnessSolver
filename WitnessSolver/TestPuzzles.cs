namespace WitnessSolver
{
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class TestPuzzles
    {
        public static IEnumerable<object[]> PuzzlesToTest =>
            new[]
            {
                Puzzles.AnotherPuzzle(),
                Puzzles.DistortedColors(),
                Puzzles.Flashing(),
                Puzzles.HedgeTetris2(),
                Puzzles.MiddleChurch(),
                Puzzles.NewPuzzle(),
                Puzzles.Overlays(),
                Puzzles.SamplePuzzle(),
                Puzzles.StartShed(),
                Puzzles.Test55(),
                Puzzles.Tetris3And3(),
                Puzzles.TetrisBasicNegative(),
                Puzzles.TetrisBasicNegative2(),
                Puzzles.TetrisCombine(),
                Puzzles.TetrisComplex(),
                Puzzles.TetrisCross(),
                Puzzles.TetrisRotationPuzzle(),
                Puzzles.TetrisSimple(),
                Puzzles.Triangle1(),
                Puzzles.Triangle2(),
                Puzzles.TriangleOptimization(),
                Puzzles.TriangleSectionTrap(),
                Puzzles.TunnelPuzzle(),
                Puzzles.TunnelTetrisPuzzle(),
                Puzzles.WrapBasic(),
            }.Select(p => new object[] { p });

        public static IEnumerable<object[]> LongPuzzlesToTest =>
            new[]
            {
                Puzzles.ComplexBeginning(),
                Puzzles.WrapLarge(),
            }.Select(p => new object[] { p });

        [DataTestMethod]
        [DynamicData(nameof(PuzzlesToTest))]
        public void SolvesCorrectly(object puzzleObj)
        {
            var puzzle = (Puzzle)puzzleObj;
            var solver = new PuzzleSolver { Puzzle = puzzle };
            Assert.AreEqual(puzzle.ExpectedSolutions, solver.Solve());
        }

        [DataTestMethod]
        [DynamicData(nameof(LongPuzzlesToTest))]
        [TestCategory("Long")]
        public void SolvesCorrectlyLong(object puzzleObj)
        {
            var puzzle = (Puzzle)puzzleObj;
            var solver = new PuzzleSolver { Puzzle = puzzle };
            Assert.AreEqual(puzzle.ExpectedSolutions, solver.Solve());
        }
    }
}
