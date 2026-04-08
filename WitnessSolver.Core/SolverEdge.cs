namespace WitnessSolver
{
    public class SolverEdge
    {
        public readonly SolverNode Start;
        public readonly SolverNode End;
        public SolverEdge Reverse;
        public readonly SolverCell[] AdjacentCells;
        public readonly bool Need;
        public readonly bool Valid;
        public readonly bool MustTraverse;
        public readonly bool MayTraverse;
        public readonly char ShortName;

        // Solver-mutable state
        public bool Traversed;

        public bool IsUsed => this.Traversed || this.Reverse.Traversed;

        public SolverEdge(SolverNode start, SolverNode end, SolverCell[] adjacentCells, bool need, bool valid, bool mustTraverse, bool mayTraverse, char shortName)
        {
            this.Start = start;
            this.End = end;
            this.AdjacentCells = adjacentCells;
            this.Need = need;
            this.Valid = valid;
            this.MustTraverse = mustTraverse;
            this.MayTraverse = mayTraverse;
            this.ShortName = shortName;
        }
    }
}
