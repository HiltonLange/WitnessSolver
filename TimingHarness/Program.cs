using System;
using System.Collections.Generic;
using System.Diagnostics;
using WitnessSolver;

var puzzles = new List<(string name, Func<Puzzle> factory)>
{
    ("SamplePuzzle",        Puzzles.SamplePuzzle),
    ("Triangle1",           Puzzles.Triangle1),
    ("Triangle2",           Puzzles.Triangle2),
    ("TriangleOptimization",Puzzles.TriangleOptimization),
    ("TriangleSectionTrap", Puzzles.TriangleSectionTrap),
    ("Overlays",            Puzzles.Overlays),
    ("StartShed",           Puzzles.StartShed),
    ("MiddleChurch",        Puzzles.MiddleChurch),
    ("Flashing",            Puzzles.Flashing),
    ("DistortedColors",     Puzzles.DistortedColors),
    ("NewPuzzle",           Puzzles.NewPuzzle),
    ("Test55",              Puzzles.Test55),
    ("WrapBasic",           Puzzles.WrapBasic),
    ("TunnelPuzzle",        Puzzles.TunnelPuzzle),
    ("TetrisSimple",        Puzzles.TetrisSimple),
    ("TetrisCombine",       Puzzles.TetrisCombine),
    ("TetrisBasicNegative", Puzzles.TetrisBasicNegative),
    ("TetrisBasicNegative2",Puzzles.TetrisBasicNegative2),
    ("TetrisRotationPuzzle",Puzzles.TetrisRotationPuzzle),
    ("Tetris3And3",         Puzzles.Tetris3And3),
    ("TunnelTetrisPuzzle",  Puzzles.TunnelTetrisPuzzle),
    ("HedgeTetris2",        Puzzles.HedgeTetris2),
    ("TetrisComplex",       Puzzles.TetrisComplex),
    ("TetrisCross",         Puzzles.TetrisCross),
};

long totalMs = 0;
foreach (var (name, factory) in puzzles)
{
    var puzzle = factory();
    var solver = new PuzzleSolver { Puzzle = puzzle };
    var sw = Stopwatch.StartNew();
    int solutions = solver.Solve();
    sw.Stop();
    totalMs += sw.ElapsedMilliseconds;
    Console.WriteLine($"{sw.ElapsedMilliseconds,6} ms  {solutions,4} solutions  {name}");
}
Console.WriteLine($"{"------",6}");
Console.WriteLine($"{totalMs,6} ms  TOTAL");
