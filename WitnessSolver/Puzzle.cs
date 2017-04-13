using System;
using System.Collections.Generic;
using System.Linq;

namespace WitnessSolver
{
    internal abstract class Puzzle
    {
        protected string Name;

        public Point Start;
        protected Point Location;

        protected int AllRouteCount;
        protected int GoodRouteCount;
        private long StepCount;
        private long StepCountLoop;
        private long StepShowPeriod = 100000;

        public int XSize;
        public int YSize;

        public List<Edge> Route;
        public Point[,] Points;
        public HashSet<Edge> Edges;
        public HashSet<Cell> Cells;

        private List<Section> Sections;

        public List<List<Edge>> Solutions;

        public event EventHandler<PuzzleSolveEventArgs> Update;

        public PuzzleDrawer Drawer;
        public long ExpectedSolutions;
        public bool Wrap;

        private void CalculateOptimizations()
        {
            // Calculate which edges must be traversed because they seperate colors
            foreach (var edge in this.Edges)
            {
                if (edge.LeftCell != null &&
                    edge.RightCell != null &&
                    edge.LeftCell.SquareColorLetter != ' ' &&
                    edge.RightCell.SquareColorLetter != ' ' &&
                    edge.LeftCell.SquareColorLetter != edge.RightCell.SquareColorLetter)
                {
                    edge.CalculatedMustTraverse = true;
                }
            }

            // Calculate point into
            // Only 2 edges into a mandatory point makes them both mandatory edges
            // Sum up mandatory edge count from a point
            foreach (var point in this.Points)
            {
                if (point.MustTraverse && point.OutEdges.Count == 2)
                {
                    foreach (var outEdge in point.OutEdges.Values)
                    {
                        outEdge.CalculatedMustTraverse = true;
                    }
                }

                point.NeedCount = point.OutEdges.Values.Count(edge => edge.Need);
            }

            // Number all the cells for easy enumeration
            int cellIdx = 0;
            foreach (var cell in this.Cells)
            {
                cell.CellIndex = cellIdx++;
            }
        }

        public void PrepareToSolve()
        {
            this.Solutions = new List<List<Edge>>();
            this.CalculateOptimizations();
            this.Sections = new List<Section>
            {
                new Section(new List<Cell>(this.Cells), this.Cells.Count),
            };
            this.Start.Visited = true;
            this.SendUpdate(false);
            this.Drawer.DrawState(false);
        }

        public bool AddEdge(Edge edge)
        {
            this.Location = edge.End;
            this.Location.Visited = true;
            edge.Traversed = true;
            this.Route.Add(edge);
            if (edge.Need)
            {
                edge.Start.NeedCount--;
                edge.End.NeedCount--;
            }

            // Check for section split
            if (edge.LeftCell != null && edge.RightCell != null)
            {
                if (this.Location.OutEdges.Values.Any(outEdge => outEdge.LeftCell == null || outEdge.RightCell == null))
                {
                    // We have reached an outer edge, likely a section split here
                    var oldSection = edge.LeftCell.Section;
                    var newSections = oldSection.FindSubSections();

                    if (newSections != null)
                    {
                        // Section split has occurred
                        this.Sections.Remove(oldSection);
                        this.Sections.AddRange(newSections);
                    }
                }
            }

            var good = true;

            // Check that we're not going past cells too many times
            foreach (var cell in edge.AdjacentCells())
            {
               cell.Section.Checked = false;
                if (cell.TriangleCount.HasValue)
                {
                    good &= cell.TriangleCount > cell.UsedEdgeCount;
                    cell.UsedEdgeCount++;
                }
            }

            // Check if any previous unchecked sections can now be checked
            foreach (var section in this.Sections)
            {
                if (edge.LeftCell != null && edge.LeftCell.Section == section)
                {
                    continue;
                }

                if (edge.RightCell != null && edge.RightCell.Section == section)
                {
                    continue;
                }

                if (!section.CheckSection())
                {
                    good = false;
                    break;
                }
            }

            return good;
        }

        public void RemoveEdge(Edge edge)
        {
            this.Location.Visited = false;
            this.Location = edge.Start;
            edge.Traversed = false;
            this.Route.RemoveAt(this.Route.Count - 1);
            if (edge.Need)
            {
                edge.Start.NeedCount++;
                edge.End.NeedCount++;
            }

            // Restore the triangle count
            foreach (var cell in edge.AdjacentCells())
            {
                cell.Section.Checked = false;
                if (cell.TriangleCount.HasValue)
                {
                    cell.UsedEdgeCount--;
                }
            }

            // Check for section merge
            if (edge.LeftCell != null && edge.RightCell != null)
            {
                if (edge.LeftCell.Section != edge.RightCell.Section)
                {
                    var rightSection = edge.RightCell.Section;
                    edge.LeftCell.Section.UnionWith(rightSection);
                    this.Sections.Remove(rightSection);
                }
            }
        }

        public List<Edge> PossibleOutEdges()
        {
            if (this.Location.NeedCount > 1)
            {
                return new List<Edge>();
            }

            var choiceEdges = this.Location.OutEdges.Values.Where(edge => !edge.Used && edge.Valid && !edge.End.Visited);

            if (this.Location.NeedCount == 1)
            {
                return choiceEdges.Where(edge => edge.Need).Take(1).ToList();
            }

            return choiceEdges.ToList();
        }

        public bool CheckSolved()
        {
            this.PeriodicDraw();

            if (this.Location.End)
            {
                this.AllRouteCount++;

                // Check edge traversals
                foreach (var edge in this.Edges)
                {
                    if (edge.MustTraverse && !edge.Traversed && !edge.ReversedEdge.Traversed)
                    {
                        return false;
                    }

                    if (!edge.MayTraverse && (edge.Traversed || edge.ReversedEdge.Traversed))
                    {
                        return false;
                    }
                }

                // Check point traversals
                foreach (var point in this.Points)
                {
                    if (point.MustTraverse)
                    {
                        if (!point.InEdges.Values.Any(edge => edge.Traversed))
                        {
                            return false;
                        }
                    }
                }

                // Check all sections
                if (!this.Sections.All(section => section.CheckSection()))
                {
                    return false;
                }

                // Puzzle is solved!
                this.Drawer.DrawState(true);
                this.SendUpdate(false);

                this.GoodRouteCount++;
                this.Solutions.Add(new List<Edge>(this.Route));
                char[] route = new char[this.Route.Count];
                for (int i = 0; i < this.Route.Count; i++)
                {
                    route[i] = this.Route[i].ShortName;
                }
                System.Diagnostics.Debug.WriteLine(new string(route));
                return true;
            }

            return false;
        }

        private void PeriodicDraw()
        {
            this.StepCount++;
            this.StepCountLoop++;

            if (this.StepCountLoop == this.StepShowPeriod)
            {
                this.Drawer.DrawState(false);
                this.SendUpdate(false);
                this.StepCountLoop = 0;
            }
        }

        public void SendUpdate(bool isDone)
        {
            var puzzleSolveEventArgs = new PuzzleSolveEventArgs()
            {
                EdgesAdded = this.StepCount,
                RoutesFound = this.AllRouteCount,
                SolutionsFound = this.GoodRouteCount,
                IsDone = isDone,
            };

            this.Update?.Invoke(this, puzzleSolveEventArgs);
        }

        public override string ToString()
        {
            return this.Name;
        }

        public class PuzzleSolveEventArgs : EventArgs
        {
            public long EdgesAdded;
            public long RoutesFound;
            public long SolutionsFound;
            public bool IsDone;
        }
    }
}
