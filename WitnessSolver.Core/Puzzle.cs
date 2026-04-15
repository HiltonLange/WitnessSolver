using System.Collections.Generic;

namespace WitnessSolver
{
    public abstract class Puzzle
    {
        public string Name;
        public Point Start;
        public int XSize;
        public int YSize;
        public bool Wrap;

        public List<Edge> Route;
        public Point[,] Points;
        public HashSet<Edge> Edges;
        public HashSet<Cell> Cells;

        public override string ToString() => this.Name;
    }
}
