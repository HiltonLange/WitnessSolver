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

        public Graphics FormGraphics;
        protected Graphics BufferGraphics;
        protected Puzzle Puzzle;

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

        protected PuzzleDrawer(Puzzle puzzle)
        {
            this.Puzzle = puzzle;
        }

        public void DrawState(bool isSolved)
        {
            if (this.FormGraphics == null)
            {
                return;
            }

            var lineColor = isSolved ? Color.Green : Color.DarkRed;

            var currentContext = BufferedGraphicsManager.Current;
            var buffer = currentContext.Allocate(this.FormGraphics,
                new Rectangle(0, 0, (this.Puzzle.XSize + 1) * ScaleSize, (this.Puzzle.YSize + 1) * ScaleSize));
            this.BufferGraphics = buffer.Graphics;
            this.BufferGraphics.Clear(Color.LightGray);

            foreach (var edge in this.Puzzle.Edges)
            {
                this.DrawEdge(edge, Pens.Gray, Pens.LightGray);
            }

            foreach (var edge in this.Puzzle.Route)
            {
                this.DrawEdge(edge, new Pen(lineColor, PathThickness), new Pen(Color.LightGray, PathThickness));
            }

            this.DrawStart();

            foreach (var cell in this.Puzzle.Cells)
            {
                this.DrawCell(cell);
            }

            foreach (var point in this.Puzzle.Points)
            {
                this.DrawPoint(point);
            }

            buffer.Render();
        }

        protected abstract void DrawPoint(Point point);

        protected abstract void DrawCell(Cell cell);

        protected abstract void DrawStart();

        protected abstract void DrawEdge(Edge edge, Pen edgePen, Pen backgroundPen);
    }
}
