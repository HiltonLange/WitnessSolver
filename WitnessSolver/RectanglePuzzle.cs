using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace WitnessSolver
{
    class RectanglePuzzle : Puzzle
    {
        public Cell[,] Cell;

        public RectanglePuzzle(string name, int xSize, int ySize, Point start, Point end)
        {
            this.Name = name;
            this.XSize = xSize;
            this.YSize = ySize;

            this.AllRouteCount = 0;
            this.GoodRouteCount = 0;
            this.Route = new List<Edge>();

            // Create points
            this.Points = new Point[xSize + 1, ySize + 1];
            for (int x = 0; x <= xSize; x++)
            {
                for (int y = 0; y <= ySize; y++)
                {
                    this.Points[x, y] = new Point(x, y);
                }
            }

            // Create edges
            this.Edges = new HashSet<Edge>();
            int[] dirX = { 1, 0, -1, 0 };
            int[] dirY = { 0, -1, 0, 1 };

            for (int x = 0; x <= xSize; x++)
            {
                for (int y = 0; y <= ySize; y++)
                {
                    Point point = this.Points[x, y];
                    for (int k = 0; k < 4; k++)
                    {
                        int x2 = x + dirX[k];
                        int y2 = y + dirY[k];
                        if (x2 >= 0 && x2 <= this.XSize && y2 >= 0 && y2 <= this.YSize)
                        {
                            Point targetPoint = this.Points[x2, y2];
                            Edge e = new Edge(point, targetPoint);
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

            for (int x = 0; x < xSize; x++)
            {
                for (int y = 0; y < ySize; y++)
                {
                    List<Edge> edgeLoopClockwise = new List<Edge>();
                    List<Edge> edgeLoopAntiClockwise = new List<Edge>();
                    Point[] corners = new Point[4];
                    corners[0] = this.Points[x, y];
                    corners[1] = this.Points[x + 1, y];
                    corners[2] = this.Points[x + 1, y + 1];
                    corners[3] = this.Points[x, y + 1];

                    edgeLoopClockwise.Add(corners[0].OutEdges[corners[1]]);
                    edgeLoopClockwise.Add(corners[1].OutEdges[corners[2]]);
                    edgeLoopClockwise.Add(corners[2].OutEdges[corners[3]]);
                    edgeLoopClockwise.Add(corners[3].OutEdges[corners[0]]);

                    edgeLoopAntiClockwise.Add(corners[0].OutEdges[corners[3]]);
                    edgeLoopAntiClockwise.Add(corners[3].OutEdges[corners[2]]);
                    edgeLoopAntiClockwise.Add(corners[2].OutEdges[corners[1]]);
                    edgeLoopAntiClockwise.Add(corners[1].OutEdges[corners[0]]);

                    Cell cell = new Cell(edgeLoopClockwise, edgeLoopAntiClockwise, x, y);
                    this.Cells.Add(cell);
                    this.Cell[x, y] = cell;

                    foreach (var edge in edgeLoopClockwise)
                    {
                        edge.RightCell = cell;
                    }

                    foreach (var edge in edgeLoopAntiClockwise)
                    {
                        edge.LeftCell = cell;
                    }

                    cell.SquareColorLetter = ' ';
                }
            }

            this.Start = this.Points[start.X, start.Y];
            this.Points[end.X, end.Y].End = true;
            this.Location = this.Start;

            this.Drawer = new RectanglePuzzleDrawer(this);
        }
    }
}
