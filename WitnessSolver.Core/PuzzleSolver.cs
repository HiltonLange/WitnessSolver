using System.Collections.Generic;

namespace WitnessSolver
{
    public class PuzzleSolver
    {
        public Puzzle Puzzle;
        public SolverGraph Graph;

        public int Solve()
        {
            if (this.Graph != null)
                return SolveGraph();
            return SolveLegacy();
        }

        private int SolveLegacy()
        {
            this.Puzzle.PrepareToSolve();
            this.TryAllStepsLegacy();
            this.Puzzle.SendUpdate(true);
            return this.Puzzle.Solutions.Count;
        }

        private int SolveGraph()
        {
            this.TryAllStepsGraph();
            this.Graph.SendUpdate(true);
            return this.Graph.Solutions.Count;
        }

        private void TryAllStepsGraph()
        {
            var stack = new Stack<SolverState>();
            stack.Push(new SolverState());

            while (stack.Count > 0)
            {
                var state = stack.Peek();

                if (state.Edges == null)
                {
                    if (this.Graph.CheckSolved())
                    {
                        stack.Pop();
                    }
                    else
                    {
                        state.Edges = this.Graph.PossibleOutEdges();
                        state.EdgePointer = 0;
                        state.EdgeProcessed = false;
                    }
                }
                else
                {
                    if (state.EdgePointer < state.Edges.Count)
                    {
                        var outEdge = state.Edges[state.EdgePointer];

                        if (!state.EdgeProcessed)
                        {
                            if (this.Graph.AddEdge(outEdge))
                                stack.Push(new SolverState());
                            state.EdgeProcessed = true;
                        }
                        else
                        {
                            this.Graph.RemoveEdge(outEdge);
                            state.EdgePointer++;
                            state.EdgeProcessed = false;
                        }
                    }
                    else
                    {
                        stack.Pop();
                    }
                }
            }
        }

        private void TryAllStepsLegacy()
        {
            var stack = new Stack<PuzzleSolverState>();
            stack.Push(new PuzzleSolverState());

            while (stack.Count > 0)
            {
                var state = stack.Peek();

                if (state.Edges == null)
                {
                    if (this.Puzzle.CheckSolved())
                    {
                        stack.Pop();
                    }
                    else
                    {
                        state.Edges = this.Puzzle.PossibleOutEdges();
                        state.EdgePointer = 0;
                        state.EdgeProcessed = false;
                    }
                }
                else
                {
                    if (state.EdgePointer < state.Edges.Count)
                    {
                        var outEdge = state.Edges[state.EdgePointer];

                        if (!state.EdgeProcessed)
                        {
                            if (this.Puzzle.AddEdge(outEdge))
                                stack.Push(new PuzzleSolverState());
                            state.EdgeProcessed = true;
                        }
                        else
                        {
                            this.Puzzle.RemoveEdge(outEdge);
                            state.EdgePointer++;
                            state.EdgeProcessed = false;
                        }
                    }
                    else
                    {
                        stack.Pop();
                    }
                }
            }
        }

        private class SolverState
        {
            public List<SolverEdge> Edges;
            public int EdgePointer;
            public bool EdgeProcessed;
        }
    }
}

