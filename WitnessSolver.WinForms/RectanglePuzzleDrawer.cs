using System;
using System.Drawing;

namespace WitnessSolver
{
    internal class RectanglePuzzleDrawer : PuzzleDrawer
    {
        public RectanglePuzzleDrawer()
        {
        }

        protected override void DrawStart()
        {
            this.BufferGraphics.FillEllipse(
                Brushes.Black,
                this.Graph.Start.X * ScaleSize + GridOffset - StartSize / 2,
                this.Graph.Start.Y * ScaleSize + GridOffset - StartSize / 2,
                StartSize,
                StartSize);
        }

        protected override void DrawEdge(SolverEdge edge, Pen edgePen, Pen backgroundPen)
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

            if (!edge.MayTraverse || !edge.Reverse.MayTraverse)
            {
                this.DrawPartialLine(backgroundPen, edgeX1, edgeY1, edgeX2, edgeY2, EdgeBrokenLengthFraction);
            }

            if (edge.MustTraverse || edge.Reverse.MustTraverse)
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

        protected override void DrawCell(SolverCell cell)
        {
            var cellXCenter = cell.X * ScaleSize + ScaleSize / 2 + GridOffset;
            var cellYCenter = cell.Y * ScaleSize + ScaleSize / 2 + GridOffset;

            DrawCellSquare(cell, cellXCenter, cellYCenter);
            DrawCellStar(cell, cellXCenter, cellYCenter);
            DrawCellTriangle(cell, cellXCenter, cellYCenter);
            DrawCellTetris(cell, cellXCenter, cellYCenter);
        }

        private void DrawCellSquare(SolverCell cell, int cellXCenter, int cellYCenter)
        {
            if (cell.SquareColor != ' ')
            {
                this.BufferGraphics.FillRectangle(new SolidBrush(this.LetterColor[cell.SquareColor]),
                    cellXCenter - CellSquareSize / 2,
                    cellYCenter - CellSquareSize / 2,
                    CellSquareSize,
                    CellSquareSize);
            }
        }

        private void DrawCellStar(SolverCell cell, int cellXCenter, int cellYCenter)
        {
            if (cell.StarColor != ' ')
            {
                this.BufferGraphics.FillPolygon(new SolidBrush(this.LetterColor[cell.StarColor]),
                    new[]
                    {
                        new PointF(cellXCenter - CellStarSize1, cellYCenter + CellStarSize1),
                        new PointF(cellXCenter + CellStarSize1, cellYCenter + CellStarSize1),
                        new PointF(cellXCenter + CellStarSize1, cellYCenter - CellStarSize1),
                        new PointF(cellXCenter - CellStarSize1, cellYCenter - CellStarSize1),
                    });

                this.BufferGraphics.FillPolygon(new SolidBrush(this.LetterColor[cell.StarColor]),
                    new[]
                    {
                        new PointF(cellXCenter, cellYCenter + CellStarSize2),
                        new PointF(cellXCenter + CellStarSize2, cellYCenter),
                        new PointF(cellXCenter, cellYCenter - CellStarSize2),
                        new PointF(cellXCenter - CellStarSize2, cellYCenter),
                    });
            }
        }

        private void DrawCellTriangle(SolverCell cell, int cellXCenter, int cellYCenter)
        {
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

        private void DrawCellTetris(SolverCell cell, int cellXCenter, int cellYCenter)
        {
            if (cell.Tetris != null)
            {
                foreach (var tetrisCell in cell.Tetris.TetrisCells)
                {
                    float tetrisCellXCenter = (2 * tetrisCell.X - cell.Tetris.XMax) * CellTetrisSpacing;
                    float tetrisCellYCenter = (2 * tetrisCell.Y - cell.Tetris.YMax) * CellTetrisSpacing;

                    var tetrisCellXCorners = new float[]
                    {
                        -CellTetrisSize, CellTetrisSize, CellTetrisSize, -CellTetrisSize,
                    };
                    var tetrisCellYCorners = new float[]
                    {
                        CellTetrisSize, CellTetrisSize, -CellTetrisSize, -CellTetrisSize,
                    };

                    if (cell.Tetris.AnyRotation)
                    {
                        Rotate30(ref tetrisCellXCenter, ref tetrisCellYCenter);
                        for (int i = 0; i < 4; i++)
                        {
                            Rotate30(ref tetrisCellXCorners[i], ref tetrisCellYCorners[i]);
                        }
                    }

                    tetrisCellXCenter += cellXCenter;
                    tetrisCellYCenter += cellYCenter;

                    var tetrisCellCorners = new[]
                    {
                        new PointF(tetrisCellXCenter + tetrisCellXCorners[0], tetrisCellYCenter + tetrisCellYCorners[0]),
                        new PointF(tetrisCellXCenter + tetrisCellXCorners[1], tetrisCellYCenter + tetrisCellYCorners[1]),
                        new PointF(tetrisCellXCenter + tetrisCellXCorners[2], tetrisCellYCenter + tetrisCellYCorners[2]),
                        new PointF(tetrisCellXCenter + tetrisCellXCorners[3], tetrisCellYCenter + tetrisCellYCorners[3]),
                    };

                    if (tetrisCell.Count > 0)
                    {
                        this.BufferGraphics.FillPolygon(new SolidBrush(CellTetrisColor), tetrisCellCorners);
                    }
                    else
                    {
                        this.BufferGraphics.DrawPolygon(new Pen(CellTetrisColor), tetrisCellCorners);
                    }
                }
            }
        }

        private static void Rotate30(ref float x, ref float y)
        {
            double angle = -30 * Math.PI / 180;
            double newX = Math.Cos(angle) * x - Math.Sin(angle) * y;
            double newY = Math.Sin(angle) * x + Math.Cos(angle) * y;
            x = (float) newX;
            y = (float) newY;
        }

        protected override void DrawNode(SolverNode node)
        {
            if (!node.MustTraverse && !node.IsEnd)
            {
                return;
            }

            var pointX = node.X * ScaleSize + GridOffset;
            var pointY = node.Y * ScaleSize + GridOffset;

            if (node.MustTraverse)
            {
                this.BufferGraphics.DrawRectangle(
                    Pens.Red,
                    pointX - MustTraverseSize / 2,
                    pointY - MustTraverseSize / 2,
                    MustTraverseSize,
                    MustTraverseSize);
            }

            if (node.IsEnd)
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
