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
        public bool AnyRotation = false;
        public int XMax { get; protected set; }
        public int YMax { get; protected set; }
        public char ColorLetter = ' ';

        public bool HasNegative { get; protected set; }

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

        public Tetris(List<TetrisCell> tetrisCells)
        {
            this.TetrisCells = tetrisCells;
            this.Normalize();
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
