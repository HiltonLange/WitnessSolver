using System.Collections.Generic;
using System.Drawing;

namespace WitnessSolver
{
    abstract class PuzzleDrawer
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

        internal static readonly Color BackgroundColor = Color.FromArgb(40, 40, 45);
        internal static readonly Color GridBackgroundColor = Color.LightGray;

        protected SolverGraph Graph;

        public bool DrawAllGoodRoutes = true;

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

        public void SetGraph(SolverGraph graph)
        {
            this.Graph = graph;
            this._staticBoard = null;
        }

        // --- Cached static board ---
        private Bitmap _staticBoard;

        public Bitmap GetStaticBoard()
        {
            if (_staticBoard == null)
                _staticBoard = RenderStaticBoard();
            return _staticBoard;
        }

        private Bitmap RenderStaticBoard()
        {
            int width = (this.Graph.XSize + (this.Graph.Wrap ? 2 : 1)) * ScaleSize + GridOffset * 2;
            int height = (this.Graph.YSize + 1) * ScaleSize + GridOffset * 2;
            var bmp = new Bitmap(width, height);
            using (var g = Graphics.FromImage(bmp))
            {
                g.Clear(GridBackgroundColor);

                foreach (var edge in this.Graph.Edges)
                    DrawEdge(g, edge, Pens.Gray, Pens.LightGray);

                DrawStart(g);

                foreach (var cell in this.Graph.Cells)
                    DrawCell(g, cell);

                foreach (var node in this.Graph.Nodes)
                    DrawNode(g, node);
            }
            return bmp;
        }

        // --- Route overlay ---
        public void DrawRoute(Graphics target, int[] routeEdgeIndices, Color lineColor)
        {
            if (this.Graph == null || routeEdgeIndices == null) return;

            // Draw static board
            var board = GetStaticBoard();
            target.DrawImage(board, 0, 0);

            // Overlay route
            using (var pen = new Pen(lineColor, PathThickness))
            {
                for (int i = 0; i < routeEdgeIndices.Length; i++)
                {
                    var edge = this.Graph.Edges[routeEdgeIndices[i]];
                    DrawEdge(target, edge, pen, null);
                }
            }
        }

        // --- Abstract drawing primitives ---
        protected abstract void DrawNode(Graphics g, SolverNode node);
        protected abstract void DrawCell(Graphics g, SolverCell cell);
        protected abstract void DrawStart(Graphics g);
        protected abstract void DrawEdge(Graphics g, SolverEdge edge, Pen edgePen, Pen backgroundPen);
    }
}
