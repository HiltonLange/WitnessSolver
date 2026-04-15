using System;
using System.Collections.Generic;

namespace WitnessSolver
{
    public class RectanglePuzzle : Puzzle
    {
        public Cell[,] Cell;

        public RectanglePuzzle(string name, int xSize, int ySize, Point start, Point end)
        {
            this.Name = name;
            this.XSize = xSize;
            this.YSize = ySize;
            this.Route = new List<Edge>();

            // Create points
            this.Points = new Point[xSize + 1, ySize + 1];
            for (var x = 0; x <= xSize; x++)
            {
                for (var y = 0; y <= ySize; y++)
                {
                    this.Points[x, y] = new Point(x, y);
                }
            }

            // Create edges
            this.Edges = new HashSet<Edge>();
            int[] dirX = { 1, 0, -1, 0 };
            int[] dirY = { 0, -1, 0, 1 };

            for (var x = 0; x <= xSize; x++)
            {
                for (var y = 0; y <= ySize; y++)
                {
                    var point = this.Points[x, y];
                    for (var k = 0; k < 4; k++)
                    {
                        var x2 = x + dirX[k];
                        var y2 = y + dirY[k];
                        if (x2 >= 0 && x2 <= this.XSize && y2 >= 0 && y2 <= this.YSize)
                        {
                            var targetPoint = this.Points[x2, y2];
                            var e = new Edge(point, targetPoint);
                            point.OutEdges.Add(targetPoint, e);
                            targetPoint.InEdges.Add(point, e);
                            this.Edges.Add(e);
                        }
                    }
                }
            }

            // Create cells
            this.Cells = new HashSet<Cell>();
            this.Cell = new Cell[xSize, ySize];

            for (var x = 0; x < xSize; x++)
            {
                for (var y = 0; y < ySize; y++)
                {
                    var corners = new Point[4];
                    corners[0] = this.Points[x, y];
                    corners[1] = this.Points[x + 1, y];
                    corners[2] = this.Points[x + 1, y + 1];
                    corners[3] = this.Points[x, y + 1];

                    this.CreateCellFromPoints(corners, x, y);
                }
            }

            this.Start = this.Points[start.X, start.Y];
            this.Points[end.X, end.Y].End = true;
        }

        public void AddWrap()
        {
            if (this.XSize < 2)
            {
                throw new ArgumentException("Cannot apply wrap to puzzles of XWidth less than 2");
            }

            this.Wrap = true;

            // Add edges between right hand points and left hand points
            for (int y = 0; y <= this.YSize; y++)
            {
                var rightPoint = this.Points[this.XSize, y];
                var leftPoint = this.Points[0, y];
                var e = new Edge(rightPoint, leftPoint);
                rightPoint.OutEdges.Add(leftPoint, e);
                leftPoint.InEdges.Add(rightPoint, e);
                this.Edges.Add(e);

                e = new Edge(leftPoint, rightPoint);
                leftPoint.OutEdges.Add(rightPoint, e);
                rightPoint.InEdges.Add(leftPoint, e);
                this.Edges.Add(e);
            }

            // Make space for an extra column of cells
            Cell[,] newCell = new Cell[this.XSize + 1, this.YSize];
            for (int x = 0; x < this.XSize; x++)
            {
                for (int y = 0; y < this.YSize; y++)
                {
                    newCell[x, y] = this.Cell[x, y];
                }
            }
            this.Cell = newCell;

            // Add cells on the wraparound
            for (int y = 0; y < this.YSize; y++)
            {
                var corners = new Point[4];
                corners[0] = this.Points[this.XSize, y];
                corners[1] = this.Points[0, y];
                corners[2] = this.Points[0, y + 1];
                corners[3] = this.Points[this.XSize, y + 1];

                this.CreateCellFromPoints(corners, this.XSize, y);
            }
        }

        private void CreateCellFromPoints(Point[] corners, int x, int y)
        {
            var edgeLoopClockwise = new List<Edge>();
            edgeLoopClockwise.Add(corners[0].OutEdges[corners[1]]);
            edgeLoopClockwise.Add(corners[1].OutEdges[corners[2]]);
            edgeLoopClockwise.Add(corners[2].OutEdges[corners[3]]);
            edgeLoopClockwise.Add(corners[3].OutEdges[corners[0]]);

            var cell = new Cell(edgeLoopClockwise, x, y);
            this.Cells.Add(cell);
            this.Cell[x, y] = cell;

            foreach (var edge in edgeLoopClockwise)
            {
                edge.RightCell = cell;
                edge.ReversedEdge.LeftCell = cell;
            }

            cell.SquareColorLetter = ' ';
        }
    }
}
