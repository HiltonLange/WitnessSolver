using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text.Json;
using WitnessSolver;

// Default: run everything below VeryLong. CLI arg overrides max category.
var maxCategory = PuzzleCategory.Long;
if (args.Length > 0 && Enum.TryParse<PuzzleCategory>(args[0], true, out var parsed))
    maxCategory = parsed;

var puzzles = PuzzleCatalog.AtMost(maxCategory);

Console.WriteLine($"{"Time",8}  {"Solutions",10}  {"Expected",10}  {"Status",6}  {"Cat",8}  Puzzle");
Console.WriteLine(new string('-', 72));

var results = new List<PerfResult>();
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

    bool pass = !entry.HasExpectedSolutions || entry.ExpectedSolutions == solutions;
    if (!pass) allPass = false;
    totalMs += sw.ElapsedMilliseconds;

    var result = new PerfResult
    {
        Name = entry.Name,
        Category = entry.Category.ToString(),
        TimeMs = sw.ElapsedMilliseconds,
        Solutions = solutions,
        Expected = entry.HasExpectedSolutions ? entry.ExpectedSolutions : -1,
        Pass = pass,
    };
    results.Add(result);

    var status = pass ? "  OK" : "FAIL";
    var expectedStr = entry.HasExpectedSolutions ? entry.ExpectedSolutions.ToString() : "?";
    Console.WriteLine($"{sw.ElapsedMilliseconds,7}ms  {solutions,10}  {expectedStr,10}  {status,6}  {entry.Category,8}  {entry.Name}");
}

Console.WriteLine(new string('-', 72));
Console.WriteLine($"{totalMs,7}ms  {"",10}  {"",10}  {(allPass ? "  OK" : "FAIL"),6}  {"",8}  TOTAL ({results.Count} puzzles)");

// Write JSON artifact
var jsonOutput = new PerfReport
{
    Timestamp = DateTime.UtcNow.ToString("o"),
    Commit = Environment.GetEnvironmentVariable("GITHUB_SHA") ?? "local",
    TotalMs = totalMs,
    Results = results,
};
var jsonPath = Path.Combine(Directory.GetCurrentDirectory(), "perf-results.json");
File.WriteAllText(jsonPath, JsonSerializer.Serialize(jsonOutput, new JsonSerializerOptions { WriteIndented = true }));
Console.WriteLine($"\nJSON artifact: {jsonPath}");

// Write GitHub Actions step summary (Medium+ only)
var summaryPath = Environment.GetEnvironmentVariable("GITHUB_STEP_SUMMARY");
if (!string.IsNullOrEmpty(summaryPath))
{
    using var writer = new StreamWriter(summaryPath, append: true);
    writer.WriteLine("## Puzzle Solver Performance");
    writer.WriteLine();
    writer.WriteLine("| Puzzle | Category | Time | Solutions | Status |");
    writer.WriteLine("|--------|----------|-----:|----------:|:------:|");

    long summaryTotal = 0;
    foreach (var r in results)
    {
        if (Enum.TryParse<PuzzleCategory>(r.Category, out var cat) && cat < PuzzleCategory.Medium)
            continue;
        var icon = r.Pass ? "✅" : "❌";
        writer.WriteLine($"| {r.Name} | {r.Category} | {r.TimeMs}ms | {r.Solutions:N0} | {icon} |");
        summaryTotal += r.TimeMs;
    }
    writer.WriteLine($"| **TOTAL (Medium+)** | | **{summaryTotal}ms** | | {(allPass ? "✅" : "❌")} |");
}

Environment.Exit(allPass ? 0 : 1);

record PerfResult
{
    public string Name { get; set; }
    public string Category { get; set; }
    public long TimeMs { get; set; }
    public int Solutions { get; set; }
    public long Expected { get; set; }
    public bool Pass { get; set; }
}

record PerfReport
{
    public string Timestamp { get; set; }
    public string Commit { get; set; }
    public long TotalMs { get; set; }
    public List<PerfResult> Results { get; set; }
}
