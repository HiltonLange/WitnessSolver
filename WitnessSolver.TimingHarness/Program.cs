using System;
using System.Collections.Generic;
using System.Diagnostics;
using WitnessSolver;

var puzzles = new List<(string Name, Func<Puzzle> Factory)>
{
    ("SamplePuzzle",         Puzzles.SamplePuzzle),
    ("Triangle1",            Puzzles.Triangle1),
    ("Triangle2",            Puzzles.Triangle2),
    ("TriangleOptimization", Puzzles.TriangleOptimization),
    ("TriangleSectionTrap",  Puzzles.TriangleSectionTrap),
    ("Overlays",             Puzzles.Overlays),
    ("StartShed",            Puzzles.StartShed),
    ("MiddleChurch",         Puzzles.MiddleChurch),
    ("Flashing",             Puzzles.Flashing),
    ("DistortedColors",      Puzzles.DistortedColors),
    ("NewPuzzle",            Puzzles.NewPuzzle),
    ("Test55",               Puzzles.Test55),
    ("WrapBasic",            Puzzles.WrapBasic),
    ("TunnelPuzzle",         Puzzles.TunnelPuzzle),
    ("TetrisSimple",         Puzzles.TetrisSimple),
    ("TetrisCombine",        Puzzles.TetrisCombine),
    ("TetrisBasicNegative",  Puzzles.TetrisBasicNegative),
    ("TetrisBasicNegative2", Puzzles.TetrisBasicNegative2),
    ("TetrisRotation",       Puzzles.TetrisRotationPuzzle),
    ("Tetris3And3",          Puzzles.Tetris3And3),
    ("TunnelTetris",         Puzzles.TunnelTetrisPuzzle),
    ("TetrisComplex",        Puzzles.TetrisComplex),
    ("TetrisCross",          Puzzles.TetrisCross),
    ("SwampyBoots",          Puzzles.SwampyBoots),
};

Console.WriteLine($"{"Time",8}  {"Solutions",10}  Puzzle");
Console.WriteLine(new string('-', 50));

long totalMs = 0;
foreach (var (name, factory) in puzzles)
{
    var puzzle = factory();
    var graph = SolverGraph.Compile(puzzle);
    var solver = new PuzzleSolver(graph);

    var sw = Stopwatch.StartNew();
    int solutions = solver.Solve();
    sw.Stop();

    totalMs += sw.ElapsedMilliseconds;
    Console.WriteLine($"{sw.ElapsedMilliseconds,7}ms  {solutions,10}  {name}");
}

Console.WriteLine(new string('-', 50));
Console.WriteLine($"{totalMs,7}ms  {"",10}  TOTAL");
