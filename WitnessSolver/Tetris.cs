using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Schema;

namespace WitnessSolver
{
    class Tetris
    {
        public List<TetrisCell> TetrisCells;
        public readonly bool AnyRotation;
        public int XMax { get; protected set; }
        public int YMax { get; protected set; }
        public char ColorLetter = ' ';

        public bool HasNegative { get; protected set; }

        public readonly List<Tetris> Orientations;

        public void Normalize()
        {
            int minX = int.MaxValue;
            int minY = int.MaxValue;
            this.XMax = 0;
            this.YMax = 0;

            this.TetrisCells.RemoveAll(tetrisCell => tetrisCell.Count == 0);

            foreach (var tetrisCell in this.TetrisCells)
            {
                minX = Math.Min(minX, tetrisCell.X);
                minY = Math.Min(minY, tetrisCell.Y);
            }

            foreach (var tetrisCell in this.TetrisCells)
            {
                tetrisCell.X -= minX;
                tetrisCell.Y -= minY;
                this.XMax = Math.Max(this.XMax, tetrisCell.X);
                this.YMax = Math.Max(this.YMax, tetrisCell.Y);
                if (tetrisCell.Count < 0)
                {
                    this.HasNegative = true;
                }
            }
        }

        public Tetris(List<TetrisCell> tetrisCells, char colorLetter = ' ', bool anyRotation = false)
        {
            this.TetrisCells = tetrisCells;
            this.ColorLetter = colorLetter;
            this.AnyRotation = anyRotation;
            this.Normalize();
            this.Orientations = new List<Tetris> { this };

            if (this.AnyRotation)
            {
                this.Orientations.Add(this.Orientations[0].RotateClockwise());
                this.Orientations.Add(this.Orientations[1].RotateClockwise());
                this.Orientations.Add(this.Orientations[2].RotateClockwise());
            }
        }

        private Tetris RotateClockwise()
        {
            List<TetrisCell> newCells = new List<TetrisCell>();
            foreach (var tetrisCell in this.TetrisCells)
            {
                newCells.Add(new TetrisCell(this.YMax - tetrisCell.Y, tetrisCell.X, tetrisCell.Count));
            }
            return new Tetris(newCells, this.ColorLetter, false);
        }
    }

    class TetrisCell
    {
        public int X;
        public int Y;
        public int Count;

        public TetrisCell(int x, int y, int count = 1)
        {
            this.X = x;
            this.Y = y;
            this.Count = count;
        }
    }
}
