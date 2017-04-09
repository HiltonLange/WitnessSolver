using System.Collections.Generic;

namespace WitnessSolver
{
    class Point
    {
        public readonly int X;
        public readonly int Y;
        public readonly Dictionary<Point, Edge> OutEdges;
        public readonly Dictionary<Point, Edge> InEdges;
        public bool Visited;
        public bool MustTraverse;
        public bool End;
        public int NeedCount;

        public Point(int x, int y)
        {
            this.X = x;
            this.Y = y;
            this.OutEdges = new Dictionary<Point, Edge>();
            this.InEdges = new Dictionary<Point, Edge>();
            this.Visited = false;
        }

        public override bool Equals(object obj)
        {
            var other = obj as Point;
            if (other == null)
            {
                return false;
            }

            return other.X == this.X && other.Y == this.Y;
        }

        public override int GetHashCode()
        {
            return this.X * 100007 + this.Y;
        }
    }
}
