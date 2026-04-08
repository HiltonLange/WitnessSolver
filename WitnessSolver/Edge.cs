using System;
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

        // Cached after CalculateOptimizations — stable for lifetime of solve
        public bool Need { get; private set; }
        public bool Valid { get; private set; }

        public bool Used
            =>
                this.Traversed || this.ReversedEdge.Traversed;

        private Edge reversedEdge;

        public Edge ReversedEdge
        {
            get
            {
                if (this.reversedEdge == null)
                    this.reversedEdge = this.End.OutEdges[this.Start];
                return this.reversedEdge;
            }
        }

        /// <summary>Called once from CalculateOptimizations after all edges and flags are set.</summary>
        public void CacheComputedProperties()
        {
            // Eagerly resolve ReversedEdge to eliminate future dictionary lookups
            if (this.reversedEdge == null)
                this.reversedEdge = this.End.OutEdges[this.Start];

            this.Need = this.MustTraverse || this.CalculatedMustTraverse ||
                        this.reversedEdge.MustTraverse || this.reversedEdge.CalculatedMustTraverse;
            this.Valid = this.MayTraverse && this.reversedEdge.MayTraverse;
        }

        private char? shortName;

        public char ShortName
        {
            get
            {
                if (this.shortName.HasValue)
                {
                    return this.shortName.Value;
                }
                int dx = this.End.X - this.Start.X;
                int dy = this.End.Y - this.Start.Y;
                if (dx > 1)
                {
                    dx = -1;
                }
                if (dx < -1)
                {
                    dx = 1;
                }

                if (dx == 1 && dy == 0)
                {
                    this.shortName = 'R';
                }

                if (dx == -1 && dy == 0)
                {
                    this.shortName = 'L';
                }

                if (dx == 0 && dy == 1)
                {
                    this.shortName = 'D';
                }

                if (dx == 0 && dy == -1)
                {
                    this.shortName = 'U';
                }

                if (!this.shortName.HasValue)
                {
                    throw new Exception("This edge has unexpected dx and dy");
                }

                return this.shortName.Value;
            }
        }

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
