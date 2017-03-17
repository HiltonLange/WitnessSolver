using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public Graphics FormGraphics;
        public BufferedGraphics Buffer;
        public Graphics BufferGraphics;
        public Puzzle Puzzle;

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

        public PuzzleDrawer(Puzzle puzzle)
        {
            this.Puzzle = puzzle;
        }

        public void DrawState(bool isSolved)
        {
            Color lineColor = isSolved ? Color.Green : Color.DarkRed;

            BufferedGraphicsContext currentContext = BufferedGraphicsManager.Current;
            this.Buffer = currentContext.Allocate(this.FormGraphics,
                new Rectangle(0, 0, (Puzzle.XSize + 1) * ScaleSize, (Puzzle.YSize + 1) * ScaleSize));
            this.BufferGraphics = this.Buffer.Graphics;

            BufferGraphics.Clear(Color.LightGray);

            foreach (var edge in Puzzle.Edges)
            {
                DrawEdge(edge, Pens.Gray, Pens.LightGray);
            }

            foreach (var edge in Puzzle.Route)
            {
                DrawEdge(edge, new Pen(lineColor, PathThickness), new Pen(Color.LightGray, PathThickness));
            }

            DrawStart();

            foreach (var cell in Puzzle.Cells)
            {
                DrawCell(cell);
            }

            foreach (var point in Puzzle.Points)
            {
                DrawPoint(point);
            }

            Buffer.Render();
        }

        public abstract void DrawPoint(Point point);

        public abstract void DrawCell(Cell cell);

        public abstract void DrawStart();

        public abstract void DrawEdge(Edge edge, Pen edgePen, Pen backgroundPen);

    }
}
