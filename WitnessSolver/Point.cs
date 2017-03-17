using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WitnessSolver
{
    class Point
    {
        public readonly int X;
        public readonly int Y;
        public Dictionary<Point, Edge> OutEdges;
        public Dictionary<Point, Edge> InEdges;
        public bool Visited;
        public bool MustTraverse;
        public bool End;

        public Point(int x, int y)
        {
            this.X = x;
            this.Y = y;
            this.OutEdges = new Dictionary<Point, Edge>();
            this.InEdges = new Dictionary<Point, Edge>();
            this.Visited = false;
        }

        public Point(Point point)
        {
            this.X = point.X;
            this.Y = point.Y;
            this.MustTraverse = false;
        }

        public override bool Equals(object obj)
        {
            Point other = obj as Point;
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
