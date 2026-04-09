namespace WitnessSolver
{
    public class SolverCell
    {
        public readonly int X;
        public readonly int Y;
        public readonly int Index;
        public readonly char SquareColor;
        public readonly char StarColor;
        public readonly int? TriangleCount;
        public readonly Tetris Tetris;

        // Edge loop for connectivity checks (populated during compile)
        public SolverEdge[] EdgeLoop;

        // Solver-mutable state
        public int UsedEdgeCount;
        public Section Section;

        public SolverCell(int x, int y, int index, char squareColor, char starColor, int? triangleCount, Tetris tetris)
        {
            this.X = x;
            this.Y = y;
            this.Index = index;
            this.SquareColor = squareColor;
            this.StarColor = starColor;
            this.TriangleCount = triangleCount;
            this.Tetris = tetris;
        }
    }
}
