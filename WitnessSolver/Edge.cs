using System.Collections.Generic;

namespace WitnessSolver
{
    class Edge
    {
        public readonly Point Start;
        public readonly Point End;

        public Cell RightCell;
        public Cell LeftCell;

        public bool Traversed;
        public bool MayTraverse;
        public bool MustTraverse;
        public bool CalculatedMustTraverse;

        public bool Need
            =>
                this.MustTraverse || this.CalculatedMustTraverse || this.ReversedEdge.MustTraverse ||
                this.ReversedEdge.CalculatedMustTraverse;

        public bool Valid
            =>
                this.MayTraverse && this.ReversedEdge.MayTraverse;

        public bool Used
            =>
                this.Traversed || this.ReversedEdge.Traversed;

        private Edge reversedEdge;

        public Edge ReversedEdge => this.reversedEdge ?? (this.reversedEdge = this.End.OutEdges[this.Start]);

        public Edge(Point start, Point end)
        {
            this.Start = start;
            this.End = end;
            this.Traversed = false;
            this.MayTraverse = true;
            this.MustTraverse = false;
        }

        public override bool Equals(object obj)
        {
            var other = obj as Edge;

            return other.Start.Equals(this.Start) && other.End.Equals(this.End);
        }

        public override int GetHashCode()
        {
            return this.Start.GetHashCode() * 100007 + this.End.GetHashCode();
        }

        public IEnumerable<Cell> AdjacentCells()
        {
            if (this.LeftCell != null)
            {
                yield return this.LeftCell;
            }

            if (this.RightCell != null)
            {
                yield return this.RightCell;
            }
        }
    }
}
