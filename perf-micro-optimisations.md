# Branch: perf/micro-optimisations

## Baseline
14,405 ms total (Release, .NET 8, 24 puzzles, AnotherPuzzle excluded)

## Approach
Replace heap allocations and LINQ in the hot path with pre-allocated structures, explicit loops, and inline logic. No algorithmic changes — purely reduce GC pressure and method call overhead.

## Planned changes

### 1. `Puzzle.cs` — `PossibleOutEdges()` [HIGH]
Replace two `Where(...).ToList()` calls with a single manual loop filling a pre-allocated `List<Edge>` that is reused across calls (stored on the Puzzle, cleared each call).

### 2. `Puzzle.cs` — `AddEdge()` / `RemoveEdge()` — `AdjacentCells()` [MEDIUM]
`edge.AdjacentCells()` uses `yield return`, allocating an `IEnumerator` on every call. Replace with two inline null checks on `edge.LeftCell` / `edge.RightCell`.

### 3. `Puzzle.cs` — `RemoveEdge()` — Route as Stack [MEDIUM]
`Route` is a `List<Edge>`; `RemoveAt(Count - 1)` is O(1) in practice but a `Stack<Edge>` makes the intent clearer and avoids bounds arithmetic. Minor.

### 4. `Section.cs` — `CheckSection()` — reuse collections [HIGH]
`squareLetters`, `starLetters`, `colorLetterCount`, `tetrisList` are heap-allocated on every call. Make them instance fields on `Section`, clear and reuse them.

### 5. `SectionTetrisChecker.cs` — `CanContainExactly()` — avoid LINQ [MEDIUM]
`tetrisList.Sum(...)` and `tetrisList.Any(...)` on lines 25/37 allocate enumerators. Replace with explicit loops.
`tetrisList.OrderByDescending(...).ToList()` on line 43 allocates a new sorted list. Replace with `List.Sort()` on a copy.

### 6. `Puzzle.cs` — `CheckSolved()` — avoid LINQ in Sections.All() [LOW]
`this.Sections.All(section => section.CheckSection())` allocates a delegate+enumerator. Replace with an explicit `for` loop.

## Prediction
- Sections with many tetris or color puzzles will benefit most from #4
- `PossibleOutEdges` is called O(nodes) times — #1 should have the widest impact
- Combined estimate: **15–25% reduction** (~11–12 s total)
- Most likely to help: TetrisCross, TunnelTetrisPuzzle, TetrisComplex
- Least likely to help: fast triangle/simple puzzles (too few steps)

---

## Implementation notes (filled in after coding)

## Test results (filled in after running)

## Findings
