# WitnessSolver Performance History

All measurements on the same Windows machine, .NET 8.0 Release configuration,
`dotnet test` with MSTest. Times are wall-clock as reported by the test runner.

> **Note**: The original .NET Framework 4.5.2 project (pre-conversion) ran all
> non-Long tests in ~48s on this machine in Visual Studio Debug mode. That build
> is no longer runnable from CLI — it's the last data point before this repo
> was revived.

## Test Results by Commit

| Test                     | Solutions  | `5b0b5b7` | `a9f2504` |
|--------------------------|------------|-----------|-----------|
| TestFlashing             |         32 |    1.0s   |    0.9s   |
| TestMiddleChurch         |          2 |    9ms    |    6ms    |
| TestNewPuzzle            |          1 |    1ms    |   <1ms    |
| TestOverlays             |          1 |   <1ms    |   <1ms    |
| TestSamplePuzzle         |        196 |   10ms    |    7ms    |
| TestStartShed            |          1 |   <1ms    |   <1ms    |
| TestTest55               |     46,204 |    2.0s   |    1.0s   |
| TestDistortedColors      |          5 |   81ms    |   55ms    |
| TestTriangle1            |          2 |   <1ms    |   <1ms    |
| TestTriangle2            |          4 |   <1ms    |   <1ms    |
| TestTriangleOptimization |          3 |   <1ms    |   <1ms    |
| TestTriangleSectionTrap  |          1 |   <1ms    |   <1ms    |
| TestWrapBasic            |          5 |   <1ms    |   <1ms    |
| TestTetrisSimple         |         10 |  361ms    |  269ms    |
| TestTetrisCombine        |          3 |  402ms    |  274ms    |
| TestTetrisComplex        |        246 |  374ms    |  258ms    |
| TestTetrisBasicNegative  |        116 |   53ms    |   34ms    |
| TestTetrisBasicNegative2 |          4 |   37ms    |   26ms    |
| TestTunnelPuzzle         |         40 |    1.0s   |    1.0s   |
| TestTetrisRotation       |         88 |  472ms    |  309ms    |
| TestTetris3And3          |          1 |   31ms    |   13ms    |
| TestTunnelTetris         |        116 |    1.0s   |    1.0s   |
| TestTetrisCross          |        126 |    3.0s   |    2.0s   |
| **Non-Long Total**       |            | **~13s**  |  **~8s**  |
|                          |            |           |           |
| TestWrapLarge *(Long)*   |      1,072 |   22s     |   13s     |
| TestComplexBeginning *(Long)* | 2,812 |  106s     |   72s     |
| TestAnotherPuzzle *(Long)* | 13,948,825 | 25m 35s | 16m 11s  |

## Commit Reference

| Commit    | Description |
|-----------|-------------|
| `5b0b5b7` | **Restructure only** — 3-project split (Core/WinForms/Tests), .NET 8, SDK-style csproj. No algorithm or perf changes. Original solver code with LINQ, Dictionary iteration, iterative DFS state machine. |
| `a9f2504` | **SolverGraph architecture** — `SolverGraph.Compile(puzzle)` produces optimized array-based topology. Recursive DFS (CLR call stack instead of manual `Stack<State>`). Precomputed `Need`/`Valid`/`Reverse` as readonly fields. `OutEdges` as flat arrays instead of `Dictionary<Point, Edge>`. Definition types are immutable after construction. |
