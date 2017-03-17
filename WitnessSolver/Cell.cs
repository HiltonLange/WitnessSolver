using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WitnessSolver
{
    class Cell
    {
        public List<Edge> EdgeLoopClockwise;
        public List<Edge> EdgeLoopAntiClockwise;
        public char SquareColorLetter;
        public char StarColorLetter;
        public int X;
        public int Y;

        public Cell(List<Edge> edgeLoopClockwise, List<Edge> edgeLoopAntiClockwise, int x, int y)
        {
            this.EdgeLoopClockwise = edgeLoopClockwise;
            this.EdgeLoopAntiClockwise = edgeLoopAntiClockwise;
            this.X = x;
            this.Y = y;
            this.SquareColorLetter = ' ';
            this.StarColorLetter = ' ';
        }
    }
}
