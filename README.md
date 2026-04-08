# WitnessSolver

A puzzle solver for [The Witness](https://en.wikipedia.org/wiki/The_Witness_(2016_video_game)), built as a C# WinForms application.

## What it does

Pick a puzzle from the dropdown, hit Solve, and watch the solver trace paths across the grid in real time. When it finishes, it reports how many valid solutions exist.

The solver uses an iterative depth-first backtracking search. It walks from the start point to the end point, pruning invalid branches early using constraint checks after each step.

## Supported puzzle types

| Constraint | Description |
|---|---|
| **Colored squares** | Same-color squares must end up in the same section; different colors must be separated |
| **Stars** | Two stars of the same color must share a section (exactly) |
| **Triangles** | The path must pass along exactly N edges of that cell |
| **Tetris pieces** | Pieces in a section must tile it exactly; supports rotation and negative (subtractor) pieces |
| **Must-traverse edges/points** | Certain edges or intersection dots must be included in the path |
| **Wrap-around** | The path can wrap from the right edge to the left |
| **Tunnels** | Path enters one side of the grid and exits from a different location |

Not all puzzle types from the game are implemented yet (e.g. symmetry, sound, environment puzzles).

## Puzzles included

26 puzzles are defined, ranging from simple introductions to complex combinations:

- Basic color separation, stars, triangles
- Tetris variants: simple, combined, complex, negative, rotations, cross, tunnels
- Wrap-around grids
- Puzzles named after specific in-game locations (Start Shed, Middle Church, Distorted Colors, etc.)

## Project structure

```
WitnessSolver/
├── Puzzle.cs               # Abstract base: graph structure, backtracking, section tracking
├── RectanglePuzzle.cs      # Rectangular grid builder, wrap support
├── PuzzleSolver.cs         # Iterative DFS solver
├── Cell.cs / Edge.cs / Point.cs / Section.cs  # Core graph primitives
├── Tetris.cs / SectionTetrisChecker.cs        # Tetris constraint logic
├── PuzzleDrawer.cs / RectanglePuzzleDrawer.cs # Real-time rendering
├── Form1.cs                # WinForms UI
└── Puzzles/                # All puzzle definitions
```

## Requirements

- Windows
- .NET Framework (Visual Studio / MSBuild)

Open `WitnessSolver.sln` and run.
