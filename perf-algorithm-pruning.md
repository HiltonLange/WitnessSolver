# Branch: perf/algorithm-pruning

## Baseline
14,405 ms total (Release, .NET 8, 24 puzzles)

## Approach
Eliminate redundant O(n) loops in the hot path by maintaining incremental counters updated on edge add/remove, and skip constraint checks for sections that have no constraints.

## Planned changes

### 1. `Puzzle.cs` — eliminate edge loop in `CheckSolved` [HIGH]
Currently `CheckSolved` loops ALL edges to verify MustTraverse and MayTraverse when the solver reaches an end point. For a 4x4 grid this is ~40 edges per solved-check call.

Replace with two counters maintained in `PrepareToSolve`, `AddEdge`, and `RemoveEdge`:
- `_mustTraverseRemaining` — count of `MustTraverse` edges not yet traversed. Init = number of edges where `edge.MustTraverse`. Decremented in `AddEdge` when a MustTraverse edge is traversed; incremented in `RemoveEdge`.
- `_invalidMayTraverseCount` — count of `!MayTraverse` edges that have been traversed. Init = 0. Incremented in `AddEdge` when a forbidden edge is traversed; decremented in `RemoveEdge`.

In `CheckSolved`: if `_mustTraverseRemaining > 0 || _invalidMayTraverseCount > 0` → return false immediately.

### 2. `Puzzle.cs` — eliminate point loop in `CheckSolved` [MEDIUM]
`CheckSolved` also loops all points checking `MustTraverse`. Replace with:
- `_mustTraversePointRemaining` — count of `MustTraverse` points not yet visited. Decremented in `AddEdge` when the destination point has `MustTraverse`; incremented in `RemoveEdge`.

### 3. `Section.cs` — skip `CheckSection` for unconstrained sections [MEDIUM]
Add a `HasConstraints` bool to `Section`, set to `true` during puzzle setup if the section (or any cell it might contain) has squares, stars, triangles, or tetrises. Sections with no constraints always return `true` from `CheckSection` — skip the call entirely.

However, since sections split and merge dynamically, `HasConstraints` must be recalculated when sections split (in `FindSubSections`) or merged (in `UnionWith`).

### 4. `Puzzle.cs` — only check affected sections in `AddEdge` [MEDIUM]
The section-validity loop in `AddEdge` (lines 137-154) currently iterates all sections except the ones adjacent to the current edge. For puzzles with many sections this is wasted work. This is already partially optimised by the `Checked` flag on Section, so the actual `CheckSection` body is skipped if nothing changed — this change may have limited impact.

Skip this change — the `Checked` flag already handles it.

## Prediction
- Eliminating the O(|Edges|) loop in CheckSolved should help puzzles that find many solutions (Test55: 46,204; SamplePuzzle: 196) since CheckSolved is called once per end-node visit
- `_mustTraverseRemaining` most useful for tunnel and must-traverse puzzles
- Section HasConstraints skip will help simple color/triangle-free sections
- Combined estimate: **20–35% improvement** from baseline (~9–11 s total)
- Most likely to help: Test55, SamplePuzzle, TetrisCross
- Less impact than Branch 1 (allocation reduction) since most time is in actual computation

---

## Implementation notes

Initially implemented incremental counters (`_mustTraverseEdgeRemaining` etc.) updated in `AddEdge`/`RemoveEdge`. This caused two regressions: Overlays (1→0 solutions) and Test55 (46204→25776 solutions). Root cause: traversing edge B→A did not decrement the counter for `A→B.MustTraverse`, because only `edge.MustTraverse` was checked, not `edge.ReversedEdge.MustTraverse`. The bidirectional nature of the MustTraverse check made the counter incorrect.

Fixed by replacing counters with pre-built lists (`_mustTraverseEdges`, `_mustNotTraverseEdges`, `_mustTraversePoints`) populated once in `PrepareToSolve`. `CheckSolved` iterates only these small lists instead of all edges/points. The iteration is still O(constrained) rather than O(all), and avoids LINQ allocations, while being obviously correct.

`HasConstraints` on Section was implemented cleanly with no issues.

## Test results

| Puzzle | Baseline | Optimised | Δ |
|---|---|---|---|
| Flashing | 1090 ms | 1008 ms | -7% |
| Test55 | 2253 ms | 1982 ms | -12% |
| TunnelPuzzle | 2230 ms | 1584 ms | -29% |
| TetrisCombine | 531 ms | 380 ms | -28% |
| TetrisRotationPuzzle | 642 ms | 445 ms | -31% |
| TunnelTetrisPuzzle | 2541 ms | 1955 ms | -23% |
| TetrisComplex | 446 ms | 355 ms | -20% |
| TetrisCross | 3952 ms | 3104 ms | -21% |
| **TOTAL** | **14,405 ms** | **11,379 ms** | **-21%** |

24/24 tests pass, no regressions.

## Findings

Actual improvement was **21%**, within the predicted 20–35% range. The section `HasConstraints` short-circuit provided solid gains for all tetris and color puzzles. The edge/point pre-filtering in `CheckSolved` helped TunnelPuzzle and TunnelTetrisPuzzle most (these have many MustTraverse edges, so the O(|Edges|) loop was comparatively more expensive). The counter approach was the one significant misstep — O(1) in principle but wrong in practice due to the bidirectional MustTraverse semantics. The filtered-list approach is O(must-traverse-count) and correct. Branch 2 is less impactful than Branch 1 because it only affects CheckSolved calls and section checks, not the per-step allocation pressure.
