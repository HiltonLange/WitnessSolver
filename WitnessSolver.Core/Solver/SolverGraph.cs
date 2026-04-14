using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace WitnessSolver
{
    public class SolverGraph
    {
        public readonly SolverNode[] Nodes;
        public readonly SolverEdge[] Edges;
        public readonly SolverCell[] Cells;

        public readonly SolverNode Start;
        public readonly int XSize;
        public readonly int YSize;
        public readonly bool Wrap;
        public readonly string Name;

        public readonly List<SolverEdge> Route;
        public List<Section> Sections;
        public SolverNode Location;
        public List<List<SolverEdge>> Solutions;

        public IPuzzleDrawer Drawer;
        public CancellationToken CancellationToken;
        public event EventHandler<SolveEventArgs> Update;

        private long _stepCount;
        private long _stepCountLoop;
        private const long StepShowPeriod = 100000;
        private int _allRouteCount;
        private int _goodRouteCount;

        // Incremental must-traverse tracking
        private int _remainingMustTraverseEdges;
        private int _remainingMustTraversePoints;

        private SolverGraph(SolverNode[] nodes, SolverEdge[] edges, SolverCell[] cells,
            SolverNode start, int xSize, int ySize, bool wrap, string name)
        {
            this.Nodes = nodes;
            this.Edges = edges;
            this.Cells = cells;
            this.Start = start;
            this.XSize = xSize;
            this.YSize = ySize;
            this.Wrap = wrap;
            this.Name = name;
            this.Route = new List<SolverEdge>();
            this.Solutions = new List<List<SolverEdge>>();
        }

        public static SolverGraph Compile(Puzzle puzzle)
        {
            // Phase 1: Infer calculated must-traverse (color separation, constrained points)
            foreach (var edge in puzzle.Edges)
            {
                if (edge.LeftCell != null && edge.RightCell != null &&
                    edge.LeftCell.SquareColorLetter != ' ' && edge.RightCell.SquareColorLetter != ' ' &&
                    edge.LeftCell.SquareColorLetter != edge.RightCell.SquareColorLetter)
                {
                    edge.CalculatedMustTraverse = true;
                }
            }

            foreach (var point in puzzle.Points)
            {
                if (point.MustTraverse && point.OutEdges.Count == 2)
                {
                    foreach (var outEdge in point.OutEdges.Values)
                    {
                        outEdge.CalculatedMustTraverse = true;
                    }
                }
            }

            // Phase 2: Build index maps
            var pointToIndex = new Dictionary<Point, int>();
            int nodeIndex = 0;
            foreach (var point in puzzle.Points)
            {
                pointToIndex[point] = nodeIndex++;
            }

            var cellToIndex = new Dictionary<Cell, int>();
            int cellIndex = 0;
            foreach (var cell in puzzle.Cells)
            {
                cellToIndex[cell] = cellIndex++;
            }

            // Phase 3: Create SolverCells
            var solverCells = new SolverCell[puzzle.Cells.Count];
            foreach (var cell in puzzle.Cells)
            {
                int idx = cellToIndex[cell];
                solverCells[idx] = new SolverCell(
                    cell.X, cell.Y, idx,
                    cell.SquareColorLetter, cell.StarColorLetter,
                    cell.TriangleCount, cell.Tetris);
            }

            // Phase 4: Create SolverNodes (without OutEdges yet — need edges first)
            var solverNodes = new SolverNode[pointToIndex.Count];
            foreach (var point in puzzle.Points)
            {
                int idx = pointToIndex[point];
                solverNodes[idx] = new SolverNode(
                    point.X, point.Y, idx,
                    outEdges: null, // wired later
                    isEnd: point.End,
                    mustTraverse: point.MustTraverse,
                    isOnBoundary: false, // computed later
                    needCount: 0); // computed later
            }

            // Phase 5: Create SolverEdges
            var edgeList = new List<SolverEdge>();
            var edgeMap = new Dictionary<Edge, SolverEdge>();

            foreach (var edge in puzzle.Edges)
            {
                var startNode = solverNodes[pointToIndex[edge.Start]];
                var endNode = solverNodes[pointToIndex[edge.End]];

                // Build adjacent cells array
                var adjCells = BuildAdjacentCells(edge, cellToIndex, solverCells);

                // Compute Need and Valid using the definition-level properties
                bool need = edge.MustTraverse || edge.CalculatedMustTraverse ||
                    edge.ReversedEdge.MustTraverse || edge.ReversedEdge.CalculatedMustTraverse;
                bool valid = edge.MayTraverse && edge.ReversedEdge.MayTraverse;

                var solverEdge = new SolverEdge(
                    startNode, endNode, adjCells,
                    need, valid,
                    edge.MustTraverse, edge.MayTraverse,
                    edge.ShortName);

                edgeList.Add(solverEdge);
                edgeMap[edge] = solverEdge;
            }

            var solverEdges = edgeList.ToArray();

            // Phase 6: Wire reverse edges
            foreach (var edge in puzzle.Edges)
            {
                var se = edgeMap[edge];
                var reversed = edgeMap[edge.ReversedEdge];
                se.Reverse = reversed;
            }

            // Phase 7: Wire OutEdges arrays on nodes + compute NeedCount and IsOnBoundary
            foreach (var point in puzzle.Points)
            {
                var node = solverNodes[pointToIndex[point]];
                var outEdges = new SolverEdge[point.OutEdges.Count];
                int i = 0;
                int needCount = 0;
                bool isOnBoundary = false;

                foreach (var edge in point.OutEdges.Values)
                {
                    var se = edgeMap[edge];
                    outEdges[i++] = se;
                    if (se.Need) needCount++;
                    if (se.AdjacentCells.Length < 2) isOnBoundary = true;
                }

                // SolverNode was created with null OutEdges — set them now via reflection-free approach
                SetNodeOutEdges(node, outEdges, needCount, isOnBoundary);
            }

            // Phase 8: Wire EdgeLoop on each SolverCell
            foreach (var cell in puzzle.Cells)
            {
                var sc = solverCells[cellToIndex[cell]];
                var edgeLoop = new SolverEdge[cell.EdgeLoopClockwise.Count];
                for (int i = 0; i < cell.EdgeLoopClockwise.Count; i++)
                    edgeLoop[i] = edgeMap[cell.EdgeLoopClockwise[i]];
                sc.EdgeLoop = edgeLoop;
            }

            // Phase 9: Find start node
            var startIdx = pointToIndex[puzzle.Start];
            var startSolverNode = solverNodes[startIdx];

            var graph = new SolverGraph(solverNodes, solverEdges, solverCells,
                startSolverNode, puzzle.XSize, puzzle.YSize, puzzle.Wrap,
                puzzle.Name);

            // Initialize sections
            graph.Sections = new List<Section>
            {
                new Section(solverCells, solverCells.Length),
            };
            graph.Location = startSolverNode;
            startSolverNode.Visited = true;

            // Precompute incremental counters
            int mustEdges = 0;
            foreach (var edge in solverEdges)
            {
                if (edge.MustTraverse) mustEdges++;
            }
            graph._remainingMustTraverseEdges = mustEdges;

            int mustPoints = 0;
            foreach (var node in solverNodes)
            {
                if (node.MustTraverse) mustPoints++;
            }
            graph._remainingMustTraversePoints = mustPoints;

            return graph;
        }

        private static SolverCell[] BuildAdjacentCells(Edge edge, Dictionary<Cell, int> cellToIndex, SolverCell[] solverCells)
        {
            if (edge.LeftCell != null && edge.RightCell != null)
                return new[] { solverCells[cellToIndex[edge.LeftCell]], solverCells[cellToIndex[edge.RightCell]] };
            if (edge.LeftCell != null)
                return new[] { solverCells[cellToIndex[edge.LeftCell]] };
            if (edge.RightCell != null)
                return new[] { solverCells[cellToIndex[edge.RightCell]] };
            return Array.Empty<SolverCell>();
        }

        private static void SetNodeOutEdges(SolverNode node, SolverEdge[] outEdges, int needCount, bool isOnBoundary)
        {
            // We need to set readonly fields — use unsafe or a workaround.
            // Cleanest: make OutEdges settable internally via a dedicated method.
            // For now, use System.Runtime to write readonly fields.
            var outField = typeof(SolverNode).GetField(nameof(SolverNode.OutEdges));
            outField.SetValue(node, outEdges);
            var boundaryField = typeof(SolverNode).GetField(nameof(SolverNode.IsOnBoundary));
            boundaryField.SetValue(node, isOnBoundary);
            node.NeedCount = needCount;
        }

        // --- Solver operations ---

        public bool AddEdge(SolverEdge edge)
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

            // Incremental must-traverse tracking
            if (edge.MustTraverse) _remainingMustTraverseEdges--;
            if (edge.Reverse.MustTraverse) _remainingMustTraverseEdges--;
            if (this.Location.MustTraverse) _remainingMustTraversePoints--;

            // Section split check
            if (edge.AdjacentCells.Length == 2)
            {
                if (this.Location.IsOnBoundary)
                {
                    var oldSection = edge.AdjacentCells[0].Section;
                    var newSections = oldSection.FindSubSections();
                    if (newSections != null)
                    {
                        this.Sections.Remove(oldSection);
                        this.Sections.AddRange(newSections);
                    }
                }
            }

            var good = true;

            // Triangle check
            for (int i = 0; i < edge.AdjacentCells.Length; i++)
            {
                var cell = edge.AdjacentCells[i];
                cell.Section.Checked = false;
                if (cell.TriangleCount.HasValue)
                {
                    good &= cell.TriangleCount > cell.UsedEdgeCount;
                    cell.UsedEdgeCount++;
                }
            }

            // Section validity
            for (int i = 0; i < this.Sections.Count; i++)
            {
                var section = this.Sections[i];
                if (edge.AdjacentCells.Length > 0 && edge.AdjacentCells[0].Section == section)
                    continue;
                if (edge.AdjacentCells.Length > 1 && edge.AdjacentCells[1].Section == section)
                    continue;

                if (!section.CheckSection())
                {
                    good = false;
                    break;
                }
            }

            return good;
        }

        public void RemoveEdge(SolverEdge edge)
        {
            // Undo incremental tracking before state changes
            if (this.Location.MustTraverse) _remainingMustTraversePoints++;
            if (edge.MustTraverse) _remainingMustTraverseEdges++;
            if (edge.Reverse.MustTraverse) _remainingMustTraverseEdges++;

            this.Location.Visited = false;
            this.Location = edge.Start;
            edge.Traversed = false;
            this.Route.RemoveAt(this.Route.Count - 1);

            if (edge.Need)
            {
                edge.Start.NeedCount++;
                edge.End.NeedCount++;
            }

            for (int i = 0; i < edge.AdjacentCells.Length; i++)
            {
                var cell = edge.AdjacentCells[i];
                cell.Section.Checked = false;
                if (cell.TriangleCount.HasValue)
                    cell.UsedEdgeCount--;
            }

            // Section merge
            if (edge.AdjacentCells.Length == 2)
            {
                if (edge.AdjacentCells[0].Section != edge.AdjacentCells[1].Section)
                {
                    var rightSection = edge.AdjacentCells[1].Section;
                    edge.AdjacentCells[0].Section.UnionWith(rightSection);
                    this.Sections.Remove(rightSection);
                }
            }
        }

        public List<SolverEdge> PossibleOutEdges()
        {
            if (this.Location.NeedCount > 1)
                return new List<SolverEdge>();

            bool needOne = this.Location.NeedCount == 1;
            var result = new List<SolverEdge>();
            var outEdges = this.Location.OutEdges;

            for (int i = 0; i < outEdges.Length; i++)
            {
                var edge = outEdges[i];
                if (!edge.IsUsed && edge.Valid && !edge.End.Visited)
                {
                    if (needOne)
                    {
                        if (edge.Need) { result.Add(edge); return result; }
                    }
                    else
                    {
                        result.Add(edge);
                    }
                }
            }

            return result;
        }

        public bool CheckSolved()
        {
            this.PeriodicDraw();

            if (this.Location.IsEnd)
            {
                this._allRouteCount++;

                // O(1) check: all must-traverse obligations satisfied?
                if (_remainingMustTraverseEdges > 0 || _remainingMustTraversePoints > 0)
                    return false;

                // MayTraverse (broken edges) are already excluded by PossibleOutEdges via
                // edge.Valid — the solver can never traverse a broken edge, so no check needed.

                foreach (var section in this.Sections)
                {
                    if (!section.CheckSection()) return false;
                }

                this.Drawer?.DrawState(true);
                this.SendUpdate(false);
                this._goodRouteCount++;
                this.Solutions.Add(new List<SolverEdge>(this.Route));
                return true;
            }

            return false;
        }

        private void PeriodicDraw()
        {
            this._stepCount++;
            this._stepCountLoop++;
            if (this._stepCountLoop == StepShowPeriod)
            {
                this.CancellationToken.ThrowIfCancellationRequested();
                this.Drawer?.DrawState(false);
                this.SendUpdate(false);
                this._stepCountLoop = 0;
            }
        }

        public void SendUpdate(bool isDone)
        {
            this.Update?.Invoke(this, new SolveEventArgs
            {
                EdgesAdded = this._stepCount,
                RoutesFound = this._allRouteCount,
                SolutionsFound = this._goodRouteCount,
                IsDone = isDone,
            });
        }

        public override string ToString() => this.Name;

        public class SolveEventArgs : EventArgs
        {
            public long EdgesAdded;
            public int RoutesFound;
            public int SolutionsFound;
            public bool IsDone;
        }
    }
}
