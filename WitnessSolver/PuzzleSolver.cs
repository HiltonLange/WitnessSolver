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

        public void TryAllStepsRecursive()
        {
            // If this is a valid route, immediately return
            if (this.Puzzle.CheckSolved())
            {
                return;
            }

            // Enumerate valid out edges
            var validEdges = this.Puzzle.PossibleOutEdges();

            foreach (var outEdge in validEdges)
            {
                // Add an edge and check we haven't broken anything
                var goodMove = this.Puzzle.AddEdge(outEdge);
                if (goodMove)
                {
                    // If we can still move forward, then attempt to do so
                    this.TryAllStepsRecursive();
                }

                // Undo added edge, restoring previous state
                this.Puzzle.RemoveEdge(outEdge);
            }
        }

        public void TryAllStepsIterative()
        {
            var stack = new Stack<PuzzleSolverState>();
            stack.Push(new PuzzleSolverState());

            while (stack.Count > 0)
            {
                // Look at the next state on the stack to be processed
                var state = stack.Peek();

                if (state.Edges == null)
                {
                    // We've just arrived at this node. Enumerate possible out nodes

                    if (this.Puzzle.CheckSolved())
                    {
                        // The puzzle is solved, pop this off the stack
                        stack.Pop();
                    }
                    else
                    {
                        // The puzzle is unsolved, enumerate out edges
                        var choiceEdges = this.Puzzle.PossibleOutEdges();
                        state.Edges = choiceEdges;

                        // Prepare iterator
                        state.EdgePointer = 0;
                        state.EdgeProcessed = false;
                    }
                }
                else
                {
                    if (state.EdgePointer < state.Edges.Count)
                    {
                        // Iterator is pointing at an edge to process
                        var outEdge = state.Edges[state.EdgePointer];

                        if (!state.EdgeProcessed)
                        {
                            // Edge is not processed
                            var goodMove = this.Puzzle.AddEdge(outEdge);

                            if (goodMove)
                            {
                                // Push the next node onto the stack
                                var newState = new PuzzleSolverState();
                                stack.Push(newState);
                            }

                            state.EdgeProcessed = true;
                        }
                        else
                        {
                            // Edge has been processed, undo change
                            this.Puzzle.RemoveEdge(outEdge);
                            state.EdgePointer++;
                            state.EdgeProcessed = false;
                        }
                    }
                    else
                    {
                        // Iterator is complete, pop this off the stack
                        stack.Pop();
                    }
                }
            }
        }
    }
}
