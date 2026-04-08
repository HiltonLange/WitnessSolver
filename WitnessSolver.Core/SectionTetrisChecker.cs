using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WitnessSolver
{
    public class SectionTetrisChecker
    {
        public static bool CanContainExactly(Section section, List<Tetris> tetrisList)
        {
            int xMin = int.MaxValue;
            int yMin = int.MaxValue;
            int xMax = int.MinValue;
            int yMax = int.MinValue;
            foreach (var cell in section.Cells)
            {
                xMin = Math.Min(xMin, cell.X);
                xMax = Math.Max(xMax, cell.X);
                yMin = Math.Min(yMin, cell.Y);
                yMax = Math.Max(yMax, cell.Y);
            }

            bool hasNegative = tetrisList.Any(tetris => tetris.HasNegative);
            int buffer = hasNegative ? 2 : 0;

            int[,] emptyCellCount = new int[xMax - xMin + 1 + 2 * buffer, yMax - yMin + 1 + 2 * buffer];
            int emptyCells = 0;

            foreach (var cell in section.Cells)
            {
                emptyCellCount[cell.X - xMin + buffer, cell.Y - yMin + buffer]++;
                emptyCells++;
            }

            int requiredCells = tetrisList.Sum<Tetris>(tetris => tetris.TetrisCells.Sum(tetrisCell => tetrisCell.Count));
            if (requiredCells != emptyCells)
            {
                return false;
            }

            var sortedTetrisList = tetrisList.OrderByDescending(tetris => tetris.HasNegative).ToList();

            return SectionTetrisChecker.CanContainExactly(sortedTetrisList, 0, emptyCellCount, xMax - xMin + 2 * buffer, yMax - yMin + 2 * buffer);
        }

        public static bool CanContainExactly(List<Tetris> sortedTetrisList, int tetrisIndex, int[,] emptyCellCount, int xMax, int yMax)
        {
            if (tetrisIndex == sortedTetrisList.Count)
            {
                return true;
            }

            foreach (var tetris in sortedTetrisList[tetrisIndex].Orientations)
            {
                for (int x = 0; x <= xMax - tetris.XMax; x++)
                {
                    for (int y = 0; y <= yMax - tetris.YMax; y++)
                    {
                        bool good = true;
                        foreach (var tetrisCell in tetris.TetrisCells)
                        {
                            int count = emptyCellCount[tetrisCell.X + x, tetrisCell.Y + y] -= tetrisCell.Count;
                            good &= count >= 0;
                        }

                        if (good)
                        {
                            if (CanContainExactly(sortedTetrisList, tetrisIndex + 1, emptyCellCount, xMax, yMax))
                            {
                                return true;
                            }
                        }

                        foreach (var tetrisCell in tetris.TetrisCells)
                        {
                            emptyCellCount[tetrisCell.X + x, tetrisCell.Y + y] += tetrisCell.Count;
                        }
                    }
                }
            }

            return false;
        }
    }
}

