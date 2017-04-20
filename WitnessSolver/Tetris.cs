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
            }

        }

        public Tetris(Section section)
        {
            this.TetrisCells = new List<TetrisCell>();
            foreach (var cell in section.Cells)
            {
                this.TetrisCells.Add(new TetrisCell(cell.X, cell.Y));
            }

            this.Normalize();
        }

        public Tetris(List<TetrisCell> tetrisCells)
        {
            this.TetrisCells = tetrisCells;
            this.Normalize();
        }

        public override bool Equals(object obj)
        {
            Tetris other = (Tetris) obj;

            if (other == null)
            {
                return false;
            }

            if (other.XMax != this.XMax)
            {
                return false;
                
            }

            if (other.YMax != this.YMax)
            {
                return false;
            }

            if (other.TetrisCells.Count != this.TetrisCells.Count)
            {
                return false;
            }

            int[,] thisTetrisCount = new int[this.XMax + 1, this.YMax + 1];
            foreach (var tetrisCell in this.TetrisCells)
            {
                thisTetrisCount[tetrisCell.X, tetrisCell.Y] += tetrisCell.Count;
            }

            int[,] otherTetrisCount = new int[this.XMax + 1, this.YMax + 1];
            foreach (var tetrisCell in other.TetrisCells)
            {
                otherTetrisCount[tetrisCell.X, tetrisCell.Y] += tetrisCell.Count;
            }

            for (int x = 0; x <= this.XMax; x++)
            {
                for (int y = 0; y <= this.YMax; y++)
                {
                    if (thisTetrisCount[x, y] != otherTetrisCount[x, y])
                    {
                        return false;
                    }
                }
            }

            return true;
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
