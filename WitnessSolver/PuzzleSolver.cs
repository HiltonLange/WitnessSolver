using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WitnessSolver
{
    class PuzzleSolver
    {
        public Puzzle Puzzle;

        public void SolvePuzzle()
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

                    var choiceEdges = this.Puzzle.PossibleOutEdges();
                    state.Edges = choiceEdges;
                    state.EdgePointer = 0;
                    state.EdgeProcessed = false;
                }
                else
                {
                    if (state.EdgePointer == state.Edges.Count)
                    {
                        stack.Pop();
                        continue;
                    }

                    if (!state.EdgeProcessed)
                    {
                        var outEdge = state.Edges[state.EdgePointer];
                        if (!outEdge.End.Visited)
                        {
                            var goodMove = this.Puzzle.AddEdge(outEdge);
                            if (goodMove)
                            {
                                var newState = new PuzzleSolverState();
                                stack.Push(newState);
                            }
                            state.EdgeProcessed = true;
                        }
                        else
                        {
                            state.EdgePointer++;
                        }

                    }
                    else
                    {
                        var outEdge = state.Edges[state.EdgePointer];
                        this.Puzzle.RemoveEdge(outEdge);
                        state.EdgePointer++;
                        state.EdgeProcessed = false;
                    }
                }
            }
        }
    }
}
