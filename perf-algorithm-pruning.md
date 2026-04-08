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

## Implementation notes (filled in after coding)

## Test results (filled in after running)

## Findings
