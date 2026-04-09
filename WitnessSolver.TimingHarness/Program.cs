using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using WitnessSolver;

PuzzleLibrary.RegisterAssembly(typeof(Puzzles).Assembly);

var maxCategory = PuzzleCategory.Long;
if (args.Length > 0 && Enum.TryParse<PuzzleCategory>(args[0], true, out var parsed))
    maxCategory = parsed;

var puzzles = PuzzleLibrary.Where(maxCategory);

Console.WriteLine($"{"Time",8}  {"Solutions",10}  {"Expected",10}  {"Status",6}  Puzzle");
Console.WriteLine(new string('-', 65));

var results = new List<(string Name, long Ms, int Solutions, long Expected, bool Pass)>();
long totalMs = 0;
bool allPass = true;

foreach (var entry in puzzles)
{
    var puzzle = entry.Factory();
    var graph = SolverGraph.Compile(puzzle);
    var solver = new PuzzleSolver(graph);

    var sw = Stopwatch.StartNew();
    int solutions = solver.Solve();
    sw.Stop();

    bool pass = puzzle.ExpectedSolutions == solutions;
    if (!pass) allPass = false;
    totalMs += sw.ElapsedMilliseconds;
    results.Add((entry.Name, sw.ElapsedMilliseconds, solutions, puzzle.ExpectedSolutions, pass));

    var status = pass ? "  OK" : "FAIL";
    Console.WriteLine($"{sw.ElapsedMilliseconds,7}ms  {solutions,10}  {puzzle.ExpectedSolutions,10}  {status,6}  {entry.Name}");
}

Console.WriteLine(new string('-', 65));
Console.WriteLine($"{totalMs,7}ms  {"",10}  {"",10}  {(allPass ? "  OK" : "FAIL"),6}  TOTAL ({results.Count} puzzles)");

// Write GitHub Actions step summary if available
var summaryPath = Environment.GetEnvironmentVariable("GITHUB_STEP_SUMMARY");
if (!string.IsNullOrEmpty(summaryPath))
{
    using var writer = new StreamWriter(summaryPath, append: true);
    writer.WriteLine("## Puzzle Solver Results");
    writer.WriteLine();
    writer.WriteLine("| Puzzle | Time | Solutions | Status |");
    writer.WriteLine("|--------|-----:|----------:|:------:|");
    foreach (var r in results)
    {
        var icon = r.Pass ? "✅" : "❌";
        writer.WriteLine($"| {r.Name} | {r.Ms}ms | {r.Solutions:N0} | {icon} |");
    }
    writer.WriteLine($"| **TOTAL** | **{totalMs}ms** | | {(allPass ? "✅" : "❌")} |");
}

Environment.Exit(allPass ? 0 : 1);
