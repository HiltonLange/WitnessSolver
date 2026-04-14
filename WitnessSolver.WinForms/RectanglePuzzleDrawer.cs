using System;
using System.Drawing;

namespace WitnessSolver
{
    internal class RectanglePuzzleDrawer : PuzzleDrawer
    {
        public RectanglePuzzleDrawer()
        {
        }

        protected override void DrawStart(Graphics g)
        {
            g.FillEllipse(
                Brushes.Black,
                this.Graph.Start.X * ScaleSize + GridOffset - StartSize / 2,
                this.Graph.Start.Y * ScaleSize + GridOffset - StartSize / 2,
                StartSize,
                StartSize);
        }

        protected override void DrawEdge(Graphics g, SolverEdge edge, Pen edgePen, Pen backgroundPen)
        {
            var edgeX1 = edge.Start.X * ScaleSize + GridOffset;
            var edgeX2 = edge.End.X * ScaleSize + GridOffset;
            var edgeY1 = edge.Start.Y * ScaleSize + GridOffset;
            var edgeY2 = edge.End.Y * ScaleSize + GridOffset;

            if (edge.Start.X == 0 && edge.End.X > 1)
                edgeX1 = (edge.End.X + 1) * ScaleSize + GridOffset;
            if (edge.End.X == 0 && edge.Start.X > 1)
                edgeX2 = (edge.Start.X + 1) * ScaleSize + GridOffset;

            DrawPartialLine(g, edgePen, edgeX1, edgeY1, edgeX2, edgeY2, EdgeLengthFraction);

            if (backgroundPen != null)
            {
                if (!edge.MayTraverse || !edge.Reverse.MayTraverse)
                    DrawPartialLine(g, backgroundPen, edgeX1, edgeY1, edgeX2, edgeY2, EdgeBrokenLengthFraction);

                if (edge.MustTraverse || edge.Reverse.MustTraverse)
                {
                    g.DrawRectangle(Pens.Red,
                        (edgeX1 + edgeX2) / 2 - MustTraverseSize / 2,
                        (edgeY1 + edgeY2) / 2 - MustTraverseSize / 2,
                        MustTraverseSize, MustTraverseSize);
                }
            }
        }

        private static void DrawPartialLine(Graphics g, Pen pen, int x1, int y1, int x2, int y2, float lengthFraction)
        {
            g.DrawLine(pen,
                x1 * lengthFraction + x2 * (1 - lengthFraction),
                y1 * lengthFraction + y2 * (1 - lengthFraction),
                x2 * lengthFraction + x1 * (1 - lengthFraction),
                y2 * lengthFraction + y1 * (1 - lengthFraction));
        }

        protected override void DrawCell(Graphics g, SolverCell cell)
        {
            var cx = cell.X * ScaleSize + ScaleSize / 2 + GridOffset;
            var cy = cell.Y * ScaleSize + ScaleSize / 2 + GridOffset;

            if (cell.SquareColor != ' ')
            {
                g.FillRectangle(new SolidBrush(this.LetterColor[cell.SquareColor]),
                    cx - CellSquareSize / 2, cy - CellSquareSize / 2, CellSquareSize, CellSquareSize);
            }

            if (cell.StarColor != ' ')
            {
                var brush = new SolidBrush(this.LetterColor[cell.StarColor]);
                g.FillPolygon(brush, new[] {
                    new PointF(cx - CellStarSize1, cy + CellStarSize1),
                    new PointF(cx + CellStarSize1, cy + CellStarSize1),
                    new PointF(cx + CellStarSize1, cy - CellStarSize1),
                    new PointF(cx - CellStarSize1, cy - CellStarSize1) });
                g.FillPolygon(brush, new[] {
                    new PointF(cx, cy + CellStarSize2),
                    new PointF(cx + CellStarSize2, cy),
                    new PointF(cx, cy - CellStarSize2),
                    new PointF(cx - CellStarSize2, cy) });
            }

            if (cell.TriangleCount.HasValue)
            {
                int trianglesXStart = cx - ((cell.TriangleCount.Value - 1) * CellTriangleSpacing) / 2;
                for (int i = 0; i < cell.TriangleCount.Value; i++)
                {
                    int tcx = trianglesXStart + i * CellTriangleSpacing;
                    g.FillPolygon(new SolidBrush(CellTriangleColor), new[] {
                        new PointF(tcx, cy - CellTriangleSize),
                        new PointF(tcx - CellTriangleSize, cy + CellTriangleSize),
                        new PointF(tcx + CellTriangleSize, cy + CellTriangleSize) });
                }
            }

            if (cell.Tetris != null)
            {
                foreach (var tc in cell.Tetris.TetrisCells)
                {
                    float tcx = (2 * tc.X - cell.Tetris.XMax) * CellTetrisSpacing;
                    float tcy = (2 * tc.Y - cell.Tetris.YMax) * CellTetrisSpacing;
                    var xc = new float[] { -CellTetrisSize, CellTetrisSize, CellTetrisSize, -CellTetrisSize };
                    var yc = new float[] { CellTetrisSize, CellTetrisSize, -CellTetrisSize, -CellTetrisSize };

                    if (cell.Tetris.AnyRotation)
                    {
                        Rotate30(ref tcx, ref tcy);
                        for (int i = 0; i < 4; i++) Rotate30(ref xc[i], ref yc[i]);
                    }

                    tcx += cx; tcy += cy;
                    var corners = new[] {
                        new PointF(tcx + xc[0], tcy + yc[0]), new PointF(tcx + xc[1], tcy + yc[1]),
                        new PointF(tcx + xc[2], tcy + yc[2]), new PointF(tcx + xc[3], tcy + yc[3]) };

                    if (tc.Count > 0)
                        g.FillPolygon(new SolidBrush(CellTetrisColor), corners);
                    else
                        g.DrawPolygon(new Pen(CellTetrisColor), corners);
                }
            }
        }

        private static void Rotate30(ref float x, ref float y)
        {
            double angle = -30 * Math.PI / 180;
            double nx = Math.Cos(angle) * x - Math.Sin(angle) * y;
            double ny = Math.Sin(angle) * x + Math.Cos(angle) * y;
            x = (float)nx; y = (float)ny;
        }

        protected override void DrawNode(Graphics g, SolverNode node)
        {
            if (!node.MustTraverse && !node.IsEnd) return;

            var px = node.X * ScaleSize + GridOffset;
            var py = node.Y * ScaleSize + GridOffset;

            if (node.MustTraverse)
                g.DrawRectangle(Pens.Red, px - MustTraverseSize / 2, py - MustTraverseSize / 2, MustTraverseSize, MustTraverseSize);

            if (node.IsEnd)
            {
                using (var pen = new Pen(Color.Black, PathThickness))
                {
                    g.DrawLine(pen, px - EndSize, py - EndSize, px + EndSize, py + EndSize);
                    g.DrawLine(pen, px - EndSize, py + EndSize, px + EndSize, py - EndSize);
                }
            }
        }
    }
}
