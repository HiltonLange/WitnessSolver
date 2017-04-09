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

        public void CalculateOptimizations()
        {
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

            foreach (var point in this.Points)
            {
                if (point.MustTraverse && point.OutEdges.Count == 2)
                {
                    foreach (var outEdge in point.OutEdges.Values)
                    {
                        outEdge.CalculatedMustTraverse = true;
                    }
                }
            }
        }

        public void PrepareToSolve()
        {
            this.Solutions = new List<List<Edge>>();
            this.Sections = new List<Section>
            {
                new Section(new List<Cell>(this.Cells)),
            };

            this.CalculateOptimizations();
            this.Start.Visited = true;
            this.SendUpdate(true);
        }

        public bool AddEdge(Edge edge)
        {
            var good = true;

            this.Location = edge.End;
            this.Location.Visited = true;
            edge.Traversed = true;
            this.Route.Add(edge);

            // Check for section split
            if (edge.LeftCell != null && edge.RightCell != null)
            {
                if (this.Location.OutEdges.Values.Any(outEdge => outEdge.LeftCell == null || outEdge.RightCell == null))
                {
                    // Should be a section split here
                    var oldSection = this.Sections.First(section => section.Cells.Contains(edge.LeftCell));
                    var newSections = FindSubSections(oldSection.Cells);

                    if (newSections != null)
                    {
                        // Section split has occurred
                        this.Sections.Remove(oldSection);
                        this.Sections.AddRange(newSections);
                    }
                }
            }

            // Check if any previous unchecked sections can now be checked
            foreach (var section in this.Sections)
            {
                var sectionGood = this.CheckSection(section);
                if (!sectionGood && !section.Cells.Contains(edge.LeftCell) && !section.Cells.Contains(edge.RightCell))
                {
                    good = false;
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

            // Check for section merge
            if (edge.LeftCell != null && edge.RightCell != null)
            {
                var leftSection = this.Sections.First(section => section.Cells.Contains(edge.LeftCell));
                var rightSection = this.Sections.First(section => section.Cells.Contains(edge.RightCell));
                if (leftSection != rightSection)
                {
                    var mergedSection = new Section(leftSection.Cells.Union(rightSection.Cells));
                    this.Sections.Remove(leftSection);
                    this.Sections.Remove(rightSection);
                    this.Sections.Add(mergedSection);
                }
            }
        }

        public List<Edge> PossibleOutEdges()
        {
            var choiceEdges = this.Location.OutEdges.Values.Where(edge => !edge.Used && edge.Valid).ToList();
            var needEdges = choiceEdges.Where(edge => edge.Need).ToList();

            // Multiple mandatory paths are impossible to follow
            if (needEdges.Count > 1)
            {
                choiceEdges = new List<Edge>();
            }

            // One mandatory path must be followed
            if (needEdges.Count == 1)
            {
                choiceEdges = needEdges;
            }

            // Only return edges where the destination node is available
            choiceEdges = choiceEdges.Where(outEdge => !outEdge.End.Visited).ToList();

            return choiceEdges;
        }

        public bool CheckSolved()
        {
            this.PeriodicDraw();

            if (this.Location.End)
            {
                this.AllRouteCount++;

                // Check all sections
                var good = this.Sections.All(this.CheckSection);

                // Check edge traversals
                foreach (var edge in this.Edges)
                {
                    if (edge.MustTraverse && !edge.Traversed && !edge.ReversedEdge.Traversed)
                    {
                        good = false;
                    }

                    if (!edge.MayTraverse && (edge.Traversed || edge.ReversedEdge.Traversed))
                    {
                        good = false;
                    }
                }

                // Check point traversals
                foreach (var point in this.Points)
                {
                    if (point.MustTraverse)
                    {
                        if (!point.InEdges.Values.Any(edge => edge.Traversed))
                        {
                            good = false;
                        }
                    }
                }

                // Puzzle is solved!
                if (good)
                {
                    //this.Drawer.DrawState(true);
                    //System.Threading.Thread.Sleep(50);
                    //Application.DoEvents();
                    this.GoodRouteCount++;
                    this.Solutions.Add(new List<Edge>(this.Route));
                    return true;
                }
            }

            return false;
        }

        private bool CheckSection(Section section)
        {
            if (section.Checked)
            {
                return section.Correct;
            }

            var good = true;

            var squareLetters = new HashSet<char>();
            var starLetters = new HashSet<char>();
            var colorLetterCount = new Dictionary<char, int>();
            foreach (var cell in section.Cells)
            {
                // Check for square
                if (cell.SquareColorLetter != ' ')
                {
                    squareLetters.Add(cell.SquareColorLetter);
                    if (!colorLetterCount.ContainsKey(cell.SquareColorLetter))
                    {
                        colorLetterCount[cell.SquareColorLetter] = 0;
                    }

                    colorLetterCount[cell.SquareColorLetter]++;
                }

                // Check for star
                if (cell.StarColorLetter != ' ')
                {
                    starLetters.Add(cell.StarColorLetter);
                    if (!colorLetterCount.ContainsKey(cell.StarColorLetter))
                    {
                        colorLetterCount[cell.StarColorLetter] = 0;
                    }

                    colorLetterCount[cell.StarColorLetter]++;
                }
            }

            // Only 1 square of a color allowed
            if (squareLetters.Count > 1)
            {
                good = false;
            }

            // A star indicates exactly 2 of that color
            foreach (var starLetter in starLetters)
            {
                if (colorLetterCount[starLetter] != 2)
                {
                    good = false;
                }
            }

            section.Checked = true;
            section.Correct = good;
            return good;
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

        private void SendUpdate(bool isDone)
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

        private static List<Section> FindSubSections(HashSet<Cell> sectionScope)
        {
            var sections = new List<Section>();

            var visited = new HashSet<Cell>();

            foreach (var cell in sectionScope)
            {
                if (!visited.Contains(cell))
                {
                    var cells = new HashSet<Cell> { cell };
                    var cellQueue = new Queue<Cell>();
                    cellQueue.Enqueue(cell);
                    visited.Add(cell);

                    while (cellQueue.Count > 0)
                    {
                        var currentCell = cellQueue.Dequeue();
                        foreach (var edge in currentCell.EdgeLoopClockwise)
                        {
                            if (!edge.Traversed && !edge.ReversedEdge.Traversed && edge.LeftCell != null && !visited.Contains(edge.LeftCell))
                            {
                                visited.Add(edge.LeftCell);
                                cellQueue.Enqueue(edge.LeftCell);
                                cells.Add(edge.LeftCell);
                            }
                        }
                    }

                    if (cells.Count == sectionScope.Count)
                    {
                        // Section is intact
                        return null;
                    }

                    sections.Add(new Section(cells));
                }
            }

            return sections;
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
