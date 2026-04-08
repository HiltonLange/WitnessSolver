# Branch: perf/precalculations

## Baseline
14,405 ms total (Release, .NET 8, 24 puzzles)

## Approach
Pre-compute things at setup time that are currently recomputed on every solver step, and cache results that don't change between repeated calls.

## Planned changes

### 1. `Edge.cs` — eagerly initialise `ReversedEdge` [MEDIUM]
`Edge.ReversedEdge` is a lazy property backed by a dictionary lookup (`this.End.OutEdges[this.Start]`). In the hot path it is accessed in `AddEdge` (`edge.Need`), `PossibleOutEdges` (`edge.Used`, `edge.Valid`), `RemoveEdge`, `CheckSolved`, and `Section.FindSubSections`. After puzzle construction all edges exist, so we can resolve `ReversedEdge` eagerly in `CalculateOptimizations()` and eliminate the dictionary lookup on every access.

### 2. `Edge.cs` — cache `Need` and `Valid` as fields [MEDIUM]
`Need` = `MustTraverse || CalculatedMustTraverse || ReversedEdge.MustTraverse || ReversedEdge.CalculatedMustTraverse`
`Valid` = `MayTraverse && ReversedEdge.MayTraverse`

Both are computed on every property access. After `CalculateOptimizations()` these values are stable (nothing changes them during solving). Cache them as pre-computed fields, re-evaluated once during `CalculateOptimizations`.

`Need` is used in `PossibleOutEdges` (every step), `AddEdge`/`RemoveEdge` (NeedCount updates), and `NeedCount` initialisation — very hot.
`Valid` is used in `PossibleOutEdges` (every step) — very hot.
`Used` = `Traversed || ReversedEdge.Traversed` changes during solving, cannot cache.

### 3. `Section.cs` — pre-sort cells into constraint buckets on construction [LOW]
`CheckSection` iterates all cells every time checking SquareColorLetter, StarColorLetter, TriangleCount, Tetris. For sections with mixed constrained/unconstrained cells, most iterations are no-ops. Pre-build lists of only the constrained cells on Section construction. But sections are dynamic (split/merge), so this requires rebuilding the lists in `UnionWith` and `FindSubSections`. Since sections rarely have many constrained cells, the benefit is limited. **Skip this — complexity not worth it.**

### 4. `SectionTetrisChecker.cs` — cache the sorted tetris list [LOW]
`CanContainExactly` re-sorts `tetrisList` on every call. The sorted order never changes for a given set of pieces. Cache it on Section after first sort. But `tetrisList` itself is rebuilt each `CheckSection` call (from cell iteration), so the cached sort is invalidated each time. Without reusing `tetrisList`, cannot cache the sort. **Skip unless combined with #3.**

### 5. `Point.cs` — replace `Dictionary<Point, Edge>` with arrays [HIGH]
`Point.OutEdges` is a `Dictionary<Point, Edge>`. In `PossibleOutEdges`, we iterate `OutEdges.Values`. Dictionary iteration involves pointer indirection and hash-table overhead. For a regular grid, each point has at most 4 edges. Replacing with a small fixed-size array (`Edge[]`) of size 4 would:
- Make `PossibleOutEdges` iterate a 4-element array instead of a Dictionary
- Eliminate the `ReversedEdge` lazy lookup (Edge constructor can set reverse directly)
- Make `NeedCount` iteration O(4) with no allocation

This is the highest-effort change but potentially the highest impact since `OutEdges` is accessed in the inner-most loop.

## Prediction
Changes #1 and #2 together (eager ReversedEdge + cached Need/Valid):
- ReversedEdge lazy init involves a dictionary lookup. Eliminating it for every `Need`/`Valid`/`Used` check saves ~1 dict lookup per step.
- Need and Valid are accessed 2-4 times per step in PossibleOutEdges + AddEdge.
- Estimated: **10–20% improvement** from baseline (~11–12 s total)

Change #5 (array-based OutEdges):
- Could be significant but requires extensive refactoring. Attempted if time allows.
- Estimated additional: **5–15%**

Combined realistic estimate: **15–25% improvement** (~11–12 s total)

---

## Implementation notes

Changes #1 and #2 were implemented together as planned:
- `Edge.cs`: `Need` and `Valid` changed from computed properties to `{ get; private set; }` fields. `CacheComputedProperties()` method added (called once from `CalculateOptimizations()`). `reversedEdge` is also eagerly resolved there to eliminate the dictionary lookup on first hot-path access.
- `Puzzle.cs`: `CalculateOptimizations()` calls `edge.CacheComputedProperties()` after all flags are set, then computes `NeedCount` using the cached `Need` values.

**Bug caught during implementation**: Initial commit had `NeedCount` computed *before* `CacheComputedProperties()` was called. Since `Need` is a field defaulting to `false`, all `NeedCount` values were 0, disabling MustTraverse pruning entirely. `TriangleOptimization` (0-solution 6x5 grid, relies on pruning) took 136 s. Fixed by splitting the double-duty points loop into two passes: one for setting `CalculatedMustTraverse`, then cache all edges, then compute `NeedCount`.

Change #3 (Section constraint buckets) and #4 (cached tetris sort) were skipped as planned.

Change #5 (array-based OutEdges) was not attempted in this branch — complexity not warranted given results achieved.

## Test results

```
    40 ms   196 solutions  SamplePuzzle
     0 ms     1 solutions  Triangle1
     0 ms     2 solutions  Triangle2
     0 ms     0 solutions  TriangleOptimization
     0 ms     1 solutions  TriangleSectionTrap
     1 ms     1 solutions  Overlays
     0 ms     1 solutions  StartShed
    21 ms     5 solutions  MiddleChurch
   995 ms   239 solutions  Flashing
    65 ms    46 solutions  DistortedColors
     0 ms     6 solutions  NewPuzzle
  2024 ms 46204 solutions  Test55
     0 ms     5 solutions  WrapBasic
  1619 ms     4 solutions  TunnelPuzzle
   291 ms    10 solutions  TetrisSimple
   382 ms     3 solutions  TetrisCombine
    51 ms   116 solutions  TetrisBasicNegative
    32 ms     4 solutions  TetrisBasicNegative2
   430 ms    88 solutions  TetrisRotationPuzzle
    20 ms     1 solutions  Tetris3And3
  1896 ms   116 solutions  TunnelTetrisPuzzle
    19 ms     0 solutions  HedgeTetris2
   370 ms   246 solutions  TetrisComplex
  3111 ms   126 solutions  TetrisCross
------
 11367 ms  TOTAL
```

MSTest (non-Long): 24/24 passed.

## Findings

**Result: 11,367 ms vs 14,405 ms baseline — 21% improvement.**

Prediction was 10–20%; actual result slightly exceeded the upper bound.

The gain comes entirely from eliminating redundant property-expression evaluation:
- `Need` was re-evaluated on every `AddEdge`/`RemoveEdge` NeedCount update and every `PossibleOutEdges` call. Now a single field read.
- `Valid` was re-evaluated on every `PossibleOutEdges` call. Now a single field read.
- `ReversedEdge` dictionary lookup happens zero times during solving (eagerly resolved at setup).

The hidden bug (NeedCount computed before caching) was a useful reminder: when splitting a compound initialisation loop, order dependencies must be explicit. The fix — three separate passes — is cleaner than the original single loop anyway.
