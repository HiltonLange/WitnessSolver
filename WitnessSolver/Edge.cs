using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

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

        public Edge ReversedEdge
        {
            get {
                if (this.reversedEdge == null)
                {
                    this.reversedEdge = this.End.OutEdges[this.Start];
                }

                return this.reversedEdge;
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
            Edge other = obj as Edge;
            if (other == null)
            {
                return false;
            }

            return other.Start.Equals(this.Start) && other.End.Equals(this.End);
        }

        public override int GetHashCode()
        {
            return this.Start.GetHashCode() * 100007 + this.End.GetHashCode();
        }
    }
}
