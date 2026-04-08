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

## Implementation notes (filled in after coding)

## Test results (filled in after running)

## Findings
