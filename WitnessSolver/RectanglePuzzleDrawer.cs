using System.Drawing;

namespace WitnessSolver
{
    internal class RectanglePuzzleDrawer : PuzzleDrawer
    {
        public RectanglePuzzleDrawer(Puzzle puzzle)
            : base(puzzle)
        {
            this.Puzzle = puzzle;
        }

        protected override void DrawStart()
        {
            this.BufferGraphics.FillEllipse(
                Brushes.Black,
                this.Puzzle.Start.X * ScaleSize + GridOffset - StartSize / 2,
                this.Puzzle.Start.Y * ScaleSize + GridOffset - StartSize / 2,
                StartSize,
                StartSize);
        }

        protected override void DrawEdge(Edge edge, Pen edgePen, Pen backgroundPen)
        {
            var edgeX1 = edge.Start.X * ScaleSize + GridOffset;
            var edgeX2 = edge.End.X * ScaleSize + GridOffset;
            var edgeY1 = edge.Start.Y * ScaleSize + GridOffset;
            var edgeY2 = edge.End.Y * ScaleSize + GridOffset;

            // Account for wraparound cases
            if (edge.Start.X == 0 && edge.End.X > 1)
            {
                edgeX1 = (edge.End.X + 1) * ScaleSize + GridOffset;
            }

            if (edge.End.X == 0 && edge.Start.X > 1)
            {
                edgeX2 = (edge.Start.X + 1) * ScaleSize + GridOffset;
            }

            this.DrawPartialLine(edgePen, edgeX1, edgeY1, edgeX2, edgeY2, EdgeLengthFraction);
            //buffer.Graphics.DrawLine(pen, edgeX1 * 0.8F + edgeX2 * 0.2F, edgeY1, edgeX2, edgeY2);

            if (!edge.MayTraverse || !edge.ReversedEdge.MayTraverse)
            {
                this.DrawPartialLine(backgroundPen, edgeX1, edgeY1, edgeX2, edgeY2, EdgeBrokenLengthFraction);
            }

            if (edge.MustTraverse || edge.ReversedEdge.MustTraverse)
            {
                this.BufferGraphics.DrawRectangle(
                    Pens.Red,
                    (edgeX1 + edgeX2) / 2 - MustTraverseSize / 2,
                    (edgeY1 + edgeY2) / 2 - MustTraverseSize / 2,
                    MustTraverseSize,
                    MustTraverseSize);
            }
        }

        private void DrawPartialLine(Pen pen, int x1, int y1, int x2, int y2,
            float lengthFraction)
        {
            this.BufferGraphics.DrawLine(
                pen,
                x1 * lengthFraction + x2 * (1 - lengthFraction),
                y1 * lengthFraction + y2 * (1 - lengthFraction),
                x2 * lengthFraction + x1 * (1 - lengthFraction),
                y2 * lengthFraction + y1 * (1 - lengthFraction));
        }

        protected override void DrawCell(Cell cell)
        {
            var cellXCenter = cell.X * ScaleSize + ScaleSize / 2 + GridOffset;
            var cellYCenter = cell.Y * ScaleSize + ScaleSize / 2 + GridOffset;

            if (cell.SquareColorLetter != ' ')
            {
                this.BufferGraphics.FillRectangle(new SolidBrush(this.LetterColor[cell.SquareColorLetter]),
                    cellXCenter - CellSquareSize / 2,
                    cellYCenter - CellSquareSize / 2,
                    CellSquareSize,
                    CellSquareSize);
            }

            if (cell.StarColorLetter != ' ')
            {
                this.BufferGraphics.FillPolygon(new SolidBrush(this.LetterColor[cell.StarColorLetter]),
                    new[]
                    {
                        new PointF(cellXCenter - CellStarSize1, cellYCenter + CellStarSize1),
                        new PointF(cellXCenter + CellStarSize1, cellYCenter + CellStarSize1),
                        new PointF(cellXCenter + CellStarSize1, cellYCenter - CellStarSize1),
                        new PointF(cellXCenter - CellStarSize1, cellYCenter - CellStarSize1),
                    });

                this.BufferGraphics.FillPolygon(new SolidBrush(this.LetterColor[cell.StarColorLetter]),
                    new[]
                    {
                        new PointF(cellXCenter, cellYCenter + CellStarSize2),
                        new PointF(cellXCenter + CellStarSize2, cellYCenter),
                        new PointF(cellXCenter, cellYCenter - CellStarSize2),
                        new PointF(cellXCenter - CellStarSize2, cellYCenter),
                    });
            }

            if (cell.TriangleCount.HasValue)
            {
                int trianglesXStart = cellXCenter - ((cell.TriangleCount.Value - 1) * CellTriangleSpacing) / 2;
                for (int i = 0; i < cell.TriangleCount.Value; i++)
                {
                    int triangleXCenter = trianglesXStart + i * CellTriangleSpacing;
                    this.BufferGraphics.FillPolygon(new SolidBrush(CellTriangleColor),
                    new[]
                    {
                    new PointF(triangleXCenter, cellYCenter - CellTriangleSize),
                    new PointF(triangleXCenter - CellTriangleSize, cellYCenter + CellTriangleSize),
                    new PointF(triangleXCenter + CellTriangleSize, cellYCenter + CellTriangleSize),
                    });
                }
            }
        }

        protected override void DrawPoint(Point point)
        {
            if (!point.MustTraverse && !point.End)
            {
                return;
            }

            var pointX = point.X * ScaleSize + GridOffset;
            var pointY = point.Y * ScaleSize + GridOffset;

            if (point.MustTraverse)
            {
                this.BufferGraphics.DrawRectangle(
                    Pens.Red,
                    pointX - MustTraverseSize / 2,
                    pointY - MustTraverseSize / 2,
                    MustTraverseSize,
                    MustTraverseSize);
            }

            if (point.End)
            {
                this.BufferGraphics.DrawLine(
                    new Pen(Color.Black, PathThickness),
                    pointX - EndSize,
                    pointY - EndSize,
                    pointX + EndSize,
                    pointY + EndSize);

                this.BufferGraphics.DrawLine(
                    new Pen(Color.Black, PathThickness),
                    pointX - EndSize,
                    pointY + EndSize,
                    pointX + EndSize,
                    pointY - EndSize);
            }
        }
    }
}
