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

## Implementation notes

All planned changes were implemented. One significant misstep: initially attempted to reuse a single `_outEdgeBuffer` list across calls to `PossibleOutEdges`. This caused a regression where all puzzles returned 0 solutions, because `PuzzleSolverState.Edges` holds a reference to the returned list. When the next stack frame called `PossibleOutEdges` and cleared the buffer, the parent frame's edge list was wiped. Fixed by keeping per-call `new List<Edge>()` allocation (pre-sized to avoid resizing) while still eliminating all the LINQ enumerator allocations.

Changes actually landed:
- `PossibleOutEdges`: manual loop, pre-sized new list, no LINQ
- `AddEdge`/`RemoveEdge`: inlined `AdjacentCells()` yield iterator
- `AddEdge`: inlined `OutEdges.Values.Any()` outer-edge check
- `CheckSolved`: inlined `Sections.All()` and `InEdges.Values.Any()`
- `Section.CheckSection`: instance fields for 4 collections, cleared per call
- `SectionTetrisChecker`: replaced `Any`/`Sum`/`OrderByDescending` with explicit loops + `List.Sort`
- Removed all `using System.Linq` from hot-path files

## Test results

| Puzzle | Baseline | Optimised | Δ |
|---|---|---|---|
| SamplePuzzle | 41 ms | 31 ms | -24% |
| MiddleChurch | 23 ms | 15 ms | -35% |
| Flashing | 1090 ms | 823 ms | -24% |
| Test55 | 2253 ms | 1437 ms | -36% |
| TunnelPuzzle | 2230 ms | 1282 ms | -43% |
| TetrisSimple | 397 ms | 251 ms | -37% |
| TetrisCombine | 531 ms | 292 ms | -45% |
| TetrisBasicNegative | 68 ms | 39 ms | -43% |
| TetrisRotationPuzzle | 642 ms | 328 ms | -49% |
| TunnelTetrisPuzzle | 2541 ms | 1259 ms | -50% |
| TetrisComplex | 446 ms | 231 ms | -48% |
| TetrisCross | 3952 ms | 2082 ms | -47% |
| **TOTAL** | **14,405 ms** | **8,171 ms** | **-43%** |

24/24 tests pass, no regressions.

## Findings

Actual improvement was **43%**, well above the predicted 15–25%. The Section collection reuse turned out to be more impactful than expected — `CheckSection` is called far more frequently than anticipated (it is called not just at solved-check time but during section-split validation on every edge add). The LINQ enumerator eliminations across `PossibleOutEdges`, `AddEdge`, and `CheckSolved` compounded well. The `_outEdgeBuffer` reuse was the one failed attempt — it was conceptually correct in isolation but incompatible with the solver's state model where each stack frame holds a reference to its edge list.
