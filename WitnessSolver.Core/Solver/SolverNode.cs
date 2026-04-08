namespace WitnessSolver
{
    public class SolverNode
    {
        public readonly int X;
        public readonly int Y;
        public readonly int Index;
        public readonly SolverEdge[] OutEdges;
        public readonly bool IsEnd;
        public readonly bool MustTraverse;
        public readonly bool IsOnBoundary;

        // Solver-mutable state
        public bool Visited;
        public int NeedCount;

        public SolverNode(int x, int y, int index, SolverEdge[] outEdges, bool isEnd, bool mustTraverse, bool isOnBoundary, int needCount)
        {
            this.X = x;
            this.Y = y;
            this.Index = index;
            this.OutEdges = outEdges;
            this.IsEnd = isEnd;
            this.MustTraverse = mustTraverse;
            this.IsOnBoundary = isOnBoundary;
            this.NeedCount = needCount;
        }
    }
}
