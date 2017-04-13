using System.Collections.Generic;

namespace WitnessSolver
{
    class Cell
    {
        public readonly List<Edge> EdgeLoopClockwise;
        public char SquareColorLetter;
        public char StarColorLetter;
        public readonly int X;
        public readonly int Y;
        public int CellIndex;
        public int? TriangleCount;
        public int UsedEdgeCount;

        public Cell(List<Edge> edgeLoopClockwise, int x, int y)
        {
            this.EdgeLoopClockwise = edgeLoopClockwise;
            this.X = x;
            this.Y = y;
            this.SquareColorLetter = ' ';
            this.StarColorLetter = ' ';
            this.UsedEdgeCount = 0;
        }
    }
}
