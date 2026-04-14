# 🔲 WitnessSolver

A brute-force solver for puzzles from [The Witness](https://store.steampowered.com/app/210970/The_Witness/) by Jonathan Blow.

Given a puzzle definition (grid, colors, stars, tetris pieces, triangles, must-traverse points), the solver explores every possible path and finds all valid solutions.

## Architecture

```
Puzzle Definition  ──compile──►  SolverGraph  ──solve──►  Solutions
   (readable DSL)                (optimized)              (results)
```

- **WitnessSolver.Core** — solver engine. `SolverGraph.Compile(puzzle)` transforms a puzzle definition into an optimized array-based graph. `PuzzleSolver` performs recursive DFS with backtracking.
- **WitnessSolver.Puzzles** — puzzle library. Each puzzle is a factory method decorated with `[PuzzleDefinition(category)]`. Auto-discovered via reflection.
- **WitnessSolver.WinForms** — GUI. Visualizes the solving process in real-time with path animation. Supports cancellation.
- **WitnessSolver.Tests** — MSTest. Data-driven tests auto-generated from the puzzle library.
- **WitnessSolver.TimingHarness** — console benchmark. Runs all puzzles, verifies correctness, outputs per-puzzle timing + JSON artifact.

## Quick Start

```bash
# Run the GUI
dotnet run --project WitnessSolver.WinForms

# Run all tests
dotnet test

# Run the timing harness (Normal + Short + Medium + Long)
dotnet run --project WitnessSolver.TimingHarness -- Long

# Run just Medium+ for perf focus
dotnet run --project WitnessSolver.TimingHarness -- Medium
```

## Puzzle Categories

| Category | Count | Typical time | CI gate? |
|----------|------:|-------------|----------|
| Trivial  | 8     | < 1ms       | ✅ Unit tests |
| Short    | 3     | 1-50ms      | ✅ Unit tests |
| Medium   | 12    | 50ms-5s     | ✅ Timing harness |
| Long     | 2     | 5s-100s     | ✅ Timing harness |
| VeryLong | 1     | 16+ minutes | Manual only |

## Adding a Puzzle

Just add a decorated factory method anywhere in `WitnessSolver.Puzzles`:

```csharp
[PuzzleDefinition(PuzzleCategory.Medium)]
public static Puzzle MyPuzzle()
{
    var puzzle = new RectanglePuzzle("My Puzzle", 4, 4, new Point(0, 4), new Point(4, 0));
    puzzle.Cell[0, 0].SquareColorLetter = 'R';
    puzzle.Cell[1, 0].SquareColorLetter = 'B';
    puzzle.ExpectedSolutions = 42;
    return puzzle;
}
```

Tests, timing harness, WinForms combo, and CI all pick it up automatically.

## Supported Puzzle Elements

- ✅ Colored squares (section separation)
- ✅ Stars (exactly-two-of-color constraint)
- ✅ Triangles (edge-count constraint)
- ✅ Tetris pieces (section shape matching, including negatives and rotations)
- ✅ Must-traverse points and edges
- ✅ May-not-traverse (broken) edges
- ✅ Wrapping grids (cylindrical topology)
- 🔲 Reflections / symmetry
- 🔲 Error/elimination marks

## Performance

The solver finds ~860,000 solutions/second on a modern machine (measured on AnotherPuzzle: 13.9M solutions in 16 minutes). The `SolverGraph.Compile()` architecture provides ~38% speedup over naive dictionary-based traversal by converting to flat arrays with precomputed properties.

## CI

GitHub Actions runs on every PR:
1. Build all projects
2. Solve all puzzles up to Long (correctness + timing)
3. Post per-puzzle timing table to the PR summary

[![CI](https://github.com/HiltonLange/WitnessSolver/actions/workflows/ci.yml/badge.svg)](https://github.com/HiltonLange/WitnessSolver/actions/workflows/ci.yml)
