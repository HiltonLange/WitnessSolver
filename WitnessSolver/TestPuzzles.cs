namespace WitnessSolver
{
    using System;
    using System.Collections.Generic;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class TestPuzzles
    {
        private static readonly Dictionary<string, Func<Puzzle>> PuzzleLookup = new()
        {
            [nameof(Puzzles.DistortedColors)]      = Puzzles.DistortedColors,
            [nameof(Puzzles.Flashing)]             = Puzzles.Flashing,
            [nameof(Puzzles.HedgeTetris2)]         = Puzzles.HedgeTetris2,
            [nameof(Puzzles.MiddleChurch)]         = Puzzles.MiddleChurch,
            [nameof(Puzzles.NewPuzzle)]            = Puzzles.NewPuzzle,
            [nameof(Puzzles.Overlays)]             = Puzzles.Overlays,
            [nameof(Puzzles.SamplePuzzle)]         = Puzzles.SamplePuzzle,
            [nameof(Puzzles.StartShed)]            = Puzzles.StartShed,
            [nameof(Puzzles.Test55)]               = Puzzles.Test55,
            [nameof(Puzzles.Tetris3And3)]          = Puzzles.Tetris3And3,
            [nameof(Puzzles.TetrisBasicNegative)]  = Puzzles.TetrisBasicNegative,
            [nameof(Puzzles.TetrisBasicNegative2)] = Puzzles.TetrisBasicNegative2,
            [nameof(Puzzles.TetrisCombine)]        = Puzzles.TetrisCombine,
            [nameof(Puzzles.TetrisComplex)]        = Puzzles.TetrisComplex,
            [nameof(Puzzles.TetrisCross)]          = Puzzles.TetrisCross,
            [nameof(Puzzles.TetrisRotationPuzzle)] = Puzzles.TetrisRotationPuzzle,
            [nameof(Puzzles.TetrisSimple)]         = Puzzles.TetrisSimple,
            [nameof(Puzzles.Triangle1)]            = Puzzles.Triangle1,
            [nameof(Puzzles.Triangle2)]            = Puzzles.Triangle2,
            [nameof(Puzzles.TriangleOptimization)] = Puzzles.TriangleOptimization,
            [nameof(Puzzles.TriangleSectionTrap)]  = Puzzles.TriangleSectionTrap,
            [nameof(Puzzles.TunnelPuzzle)]         = Puzzles.TunnelPuzzle,
            [nameof(Puzzles.TunnelTetrisPuzzle)]   = Puzzles.TunnelTetrisPuzzle,
            [nameof(Puzzles.WrapBasic)]            = Puzzles.WrapBasic,
            [nameof(Puzzles.ComplexBeginning)]     = Puzzles.ComplexBeginning,
            [nameof(Puzzles.WrapLarge)]            = Puzzles.WrapLarge,
        };

        [DataTestMethod]
        [DataRow(nameof(Puzzles.DistortedColors))]
        [DataRow(nameof(Puzzles.Flashing))]
        [DataRow(nameof(Puzzles.HedgeTetris2))]
        [DataRow(nameof(Puzzles.MiddleChurch))]
        [DataRow(nameof(Puzzles.NewPuzzle))]
        [DataRow(nameof(Puzzles.Overlays))]
        [DataRow(nameof(Puzzles.SamplePuzzle))]
        [DataRow(nameof(Puzzles.StartShed))]
        [DataRow(nameof(Puzzles.Test55))]
        [DataRow(nameof(Puzzles.Tetris3And3))]
        [DataRow(nameof(Puzzles.TetrisBasicNegative))]
        [DataRow(nameof(Puzzles.TetrisBasicNegative2))]
        [DataRow(nameof(Puzzles.TetrisCombine))]
        [DataRow(nameof(Puzzles.TetrisComplex))]
        [DataRow(nameof(Puzzles.TetrisCross))]
        [DataRow(nameof(Puzzles.TetrisRotationPuzzle))]
        [DataRow(nameof(Puzzles.TetrisSimple))]
        [DataRow(nameof(Puzzles.Triangle1))]
        [DataRow(nameof(Puzzles.Triangle2))]
        [DataRow(nameof(Puzzles.TriangleOptimization))]
        [DataRow(nameof(Puzzles.TriangleSectionTrap))]
        [DataRow(nameof(Puzzles.TunnelPuzzle))]
        [DataRow(nameof(Puzzles.TunnelTetrisPuzzle))]
        [DataRow(nameof(Puzzles.WrapBasic))]
        public void SolvesCorrectly(string name)
        {
            var puzzle = PuzzleLookup[name]();
            var solver = new PuzzleSolver { Puzzle = puzzle };
            Assert.AreEqual(puzzle.ExpectedSolutions, solver.Solve());
        }

        [DataTestMethod]
        [DataRow(nameof(Puzzles.ComplexBeginning))]
        [DataRow(nameof(Puzzles.WrapLarge))]
        [TestCategory("Long")]
        public void SolvesCorrectlyLong(string name)
        {
            var puzzle = PuzzleLookup[name]();
            var solver = new PuzzleSolver { Puzzle = puzzle };
            Assert.AreEqual(puzzle.ExpectedSolutions, solver.Solve());
        }
    }
}
