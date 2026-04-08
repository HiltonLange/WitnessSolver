using System.Collections.Generic;
using System.Drawing;

namespace WitnessSolver
{
    abstract class PuzzleDrawer : IPuzzleDrawer
    {
        internal const int ScaleSize = 75;
        internal const float EdgeLengthFraction = 0.95F;
        internal const float EdgeBrokenLengthFraction = 0.44F;
        internal const int MustTraverseSize = 10;
        internal const int GridOffset = 40;
        internal const int StartSize = 20;
        internal const int EndSize = 7;
        internal const int PathThickness = 5;

        internal const int CellSquareSize = 14;
        internal const int CellStarSize1 = 7;
        internal const int CellStarSize2 = 10;

        internal const int CellTriangleSize = 7;
        internal const int CellTriangleSpacing = 15;
        internal Color CellTriangleColor = Color.Orange;

        internal const int CellTetrisSize = 4;
        internal const int CellTetrisSpacing = 5;
        internal Color CellTetrisColor = Color.Red;
        internal Color CellTetrisNegativeColor = Color.Blue;

        public Graphics FormGraphics;
        protected Graphics BufferGraphics;
        protected SolverGraph Graph;

        public bool DrawAllGoodRoutes = true;
        public int GoodRouteShowTimeMs = 1000;
        public bool Decay = true;
        public double DecayRate = 0.9;

        internal readonly Dictionary<char, Color> LetterColor = new Dictionary<char, Color>
        {
            {'R', Color.Red },
            {'G', Color.Green },
            {'B', Color.Blue },
            {'W', Color.White },
            {'L', Color.Black },
            {'Y', Color.Yellow },
            {'P', Color.Purple },
        };

        protected PuzzleDrawer()
        {
        }

        public void SetGraph(SolverGraph graph)
        {
            this.Graph = graph;
        }

        public bool IsInitialized => this.FormGraphics != null && this.Graph != null;

        public void DrawState(bool isSolved)
        {
            if (!this.IsInitialized)
            {
                return;
            }

            if (isSolved && !this.DrawAllGoodRoutes)
            {
                return;
            }

            var lineColor = isSolved ? Color.Green : Color.DarkRed;

            var currentContext = BufferedGraphicsManager.Current;
            var buffer = currentContext.Allocate(this.FormGraphics,
                new Rectangle(0, 0, (this.Graph.XSize + (this.Graph.Wrap ? 2 : 1)) * ScaleSize, (this.Graph.YSize + 1) * ScaleSize));
            this.BufferGraphics = buffer.Graphics;
            this.BufferGraphics.Clear(Color.LightGray);

            foreach (var edge in this.Graph.Edges)
            {
                this.DrawEdge(edge, Pens.Gray, Pens.LightGray);
            }

            foreach (var edge in this.Graph.Route)
            {
                this.DrawEdge(edge, new Pen(lineColor, PathThickness), new Pen(Color.LightGray, PathThickness));
            }

            this.DrawStart();

            foreach (var cell in this.Graph.Cells)
            {
                this.DrawCell(cell);
            }

            foreach (var node in this.Graph.Nodes)
            {
                this.DrawNode(node);
            }

            buffer.Render();

            if (isSolved)
            {
                System.Threading.Thread.Sleep(this.GoodRouteShowTimeMs);
                if (this.Decay)
                {
                    this.GoodRouteShowTimeMs = (int)(this.GoodRouteShowTimeMs * this.DecayRate);
                }
            }
        }

        protected abstract void DrawNode(SolverNode node);

        protected abstract void DrawCell(SolverCell cell);

        protected abstract void DrawStart();

        protected abstract void DrawEdge(SolverEdge edge, Pen edgePen, Pen backgroundPen);
    }
}
