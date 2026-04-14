# Copilot Instructions — WitnessSolver

## Project Overview
A brute-force solver for puzzles from The Witness. Recursive DFS with backtracking.
The codebase has a clean separation between puzzle definition (DSL) and solver execution
(compiled SolverGraph with precomputed arrays).

## Architecture
- **WitnessSolver.Core** — solver engine, `SolverGraph.Compile()`, `PuzzleSolver`, `PuzzleLibrary`
- **WitnessSolver.Puzzles** — puzzle definitions with `[PuzzleDefinition(category)]` attributes
- **WitnessSolver.WinForms** — GUI with real-time visualization
- **WitnessSolver.Tests** — data-driven MSTest, auto-discovered from PuzzleLibrary
- **WitnessSolver.TimingHarness** — console benchmark + JSON artifact

## Key Patterns
- Puzzle definition types (`Puzzle`, `Cell`, `Edge`, `Point`) are immutable after construction
- `SolverGraph.Compile(puzzle)` produces the optimized solver state (SolverNode/SolverEdge/SolverCell)
- The solver never touches definition types during solving
- Adding a puzzle = one `[PuzzleDefinition]` factory method. No lists to update.
- `PuzzleLibrary` discovers puzzles via reflection, sorted by (complexity, expected solutions)

## Code Style
- Explicit types, no `var` for non-obvious types
- Minimal comments — only when the "why" isn't obvious
- Expression-bodied members where clean
- No LINQ in solver hot paths (use indexed loops on arrays)
- `readonly` fields for topology, mutable fields only for solver state

## Testing
- `dotnet test` runs Trivial + Short puzzles (~170ms)
- `dotnet run --project WitnessSolver.TimingHarness -- Long` runs everything except VeryLong
- CI gates on build + timing harness (correctness + perf)

## Git Conventions
- Squash merge to master
- Branch names: `feature/description` or `perf/description`
- Commit messages: imperative mood, body explains why
