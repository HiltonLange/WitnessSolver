using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WitnessSolver
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void btnSolve_Click(object sender, EventArgs e)
        {
            var puzzle = this.ComplexBeginning();

            puzzle.Drawer.FormGraphics = this.outputPanel.CreateGraphics();
            puzzle.Drawer.DrawState(false);
            puzzle.Update += this.puzzle_OnUpdate;

            Task.Run(() => puzzle.FindAllRoutes());

            //MessageBox.Show(String.Format("{0} solutions in {1} total routes", puzzle.GoodRouteCount, puzzle.AllRouteCount));
        }

        private void puzzle_OnUpdate(object sender, Puzzle.PuzzleSolveEventArgs e)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new MethodInvoker(delegate { puzzle_OnUpdate(sender, e); }));
                return;
            }

            this.lblRoutes.Text = e.RoutesFound.ToString();
            this.lblSteps.Text = e.EdgesAdded.ToString();
            this.lblSolutions.Text = e.SolutionsFound.ToString();
        }

        private Puzzle Test55()
        {
            RectanglePuzzle puzzle = new RectanglePuzzle(5, 5, new Point(0, 5), new Point(5, 0));

            return puzzle;
        }


        private Puzzle Overlays()
        {
            RectanglePuzzle puzzle = new RectanglePuzzle(5, 5, new Point(0, 5), new Point(5, 0));

            puzzle.Cell[0, 0].StarColorLetter = 'P';
            puzzle.Cell[1, 0].StarColorLetter = 'G';
            puzzle.Cell[4, 4].StarColorLetter = 'G';
            puzzle.Cell[3, 4].StarColorLetter = 'P';

            puzzle.Cell[0, 0].SquareColorLetter = 'W';
            puzzle.Cell[2, 4].SquareColorLetter = 'B';

            puzzle.Points[0, 1].OutEdges[puzzle.Points[1, 1]].MustTraverse = true;

            // ..
            int y = -1;
            puzzle.Points[0, 2+y].OutEdges[puzzle.Points[1, 2+y]].MustTraverse = true;
            puzzle.Points[0, 4+y].OutEdges[puzzle.Points[1, 4+y]].MustTraverse = true;
            puzzle.Points[1, 2+y].OutEdges[puzzle.Points[1, 3+y]].MustTraverse = true;
            puzzle.Points[1, 3+y].OutEdges[puzzle.Points[1, 4+y]].MustTraverse = true;

            // ...
            // . .
            //   .
            //puzzle.Points[5, 4].OutEdges[puzzle.Points[4, 4]].MustTraverse = true;
            //puzzle.Points[4, 4].OutEdges[puzzle.Points[4, 3]].MustTraverse = true;
            //puzzle.Points[4, 3].OutEdges[puzzle.Points[4, 2]].MustTraverse = true;
            //puzzle.Points[4, 2].OutEdges[puzzle.Points[3, 2]].MustTraverse = true;
            //puzzle.Points[3, 2].OutEdges[puzzle.Points[3, 3]].MustTraverse = true;
            //puzzle.Points[3, 3].OutEdges[puzzle.Points[2, 3]].MustTraverse = true;
            //puzzle.Points[2, 3].OutEdges[puzzle.Points[2, 2]].MustTraverse = true;
            //puzzle.Points[2, 2].OutEdges[puzzle.Points[2, 1]].MustTraverse = true;
            //puzzle.Points[2, 1].OutEdges[puzzle.Points[3, 1]].MustTraverse = true;
            //puzzle.Points[3, 1].OutEdges[puzzle.Points[4, 1]].MustTraverse = true;
            //puzzle.Points[4, 1].OutEdges[puzzle.Points[5, 1]].MustTraverse = true;

            // ...
            // ..
            // .
            //puzzle.Points[5, 2].OutEdges[puzzle.Points[4, 2]].MustTraverse = true;
            //puzzle.Points[4, 2].OutEdges[puzzle.Points[4, 3]].MustTraverse = true;
            //puzzle.Points[4, 3].OutEdges[puzzle.Points[3, 3]].MustTraverse = true;
            //puzzle.Points[3, 3].OutEdges[puzzle.Points[3, 4]].MustTraverse = true;
            //puzzle.Points[3, 4].OutEdges[puzzle.Points[2, 4]].MustTraverse = true;
            //puzzle.Points[2, 4].OutEdges[puzzle.Points[2, 3]].MustTraverse = true;
            //puzzle.Points[2, 3].OutEdges[puzzle.Points[2, 2]].MustTraverse = true;
            //puzzle.Points[2, 2].OutEdges[puzzle.Points[2, 1]].MustTraverse = true;
            //puzzle.Points[2, 1].OutEdges[puzzle.Points[3, 1]].MustTraverse = true;
            //puzzle.Points[3, 1].OutEdges[puzzle.Points[4, 1]].MustTraverse = true;
            //puzzle.Points[4, 1].OutEdges[puzzle.Points[5, 1]].MustTraverse = true;

            //   .
            // ...
            // . .
            puzzle.Points[4, 0].OutEdges[puzzle.Points[4, 1]].MustTraverse = true;
            puzzle.Points[4, 1].OutEdges[puzzle.Points[3, 1]].MustTraverse = true;
            puzzle.Points[3, 1].OutEdges[puzzle.Points[2, 1]].MustTraverse = true;
            puzzle.Points[2, 1].OutEdges[puzzle.Points[2, 2]].MustTraverse = true;
            puzzle.Points[2, 2].OutEdges[puzzle.Points[2, 3]].MustTraverse = true;
            puzzle.Points[2, 3].OutEdges[puzzle.Points[3, 3]].MustTraverse = true;
            puzzle.Points[3, 3].OutEdges[puzzle.Points[3, 2]].MustTraverse = true;
            puzzle.Points[3, 2].OutEdges[puzzle.Points[4, 2]].MustTraverse = true;
            puzzle.Points[4, 2].OutEdges[puzzle.Points[4, 3]].MustTraverse = true;
            puzzle.Points[4, 3].OutEdges[puzzle.Points[5, 3]].MustTraverse = true;

            return puzzle;
        }

        private Puzzle Flashing()
        {
            RectanglePuzzle puzzle = new RectanglePuzzle(5, 5, new Point(0, 5), new Point(5, 0));

            puzzle.Cell[4, 0].StarColorLetter = 'B';
            puzzle.Cell[0, 4].StarColorLetter = 'Y';

            puzzle.Cell[0, 2].SquareColorLetter = 'Y';
            puzzle.Cell[0, 3].SquareColorLetter = 'B';
            puzzle.Cell[2, 1].SquareColorLetter = 'Y';
            puzzle.Cell[4, 3].SquareColorLetter = 'Y';
            puzzle.Cell[4, 4].SquareColorLetter = 'B';

            return puzzle;
        }

        private Puzzle ComplexBeginning()
        {
            Point[] starts =
            {
                new Point(0,7),
                new Point(2,5),
                new Point(4,2),
                new Point(6,4),
            };

            Point[] ends =
            {
                new Point(0, 0),
                new Point(7, 0),
                new Point(7, 7),
            };

            RectanglePuzzle puzzle = new RectanglePuzzle(7, 7, starts[3], ends[0]);

            puzzle.Cell[0, 0].SquareColorLetter = 'L';
            puzzle.Cell[0, 1].SquareColorLetter = 'W';
            puzzle.Cell[1, 0].SquareColorLetter = 'W';
            puzzle.Cell[2, 4].SquareColorLetter = 'L';
            puzzle.Cell[2, 5].SquareColorLetter = 'W';
            puzzle.Cell[3, 1].SquareColorLetter = 'L';
            puzzle.Cell[3, 2].SquareColorLetter = 'W';
            puzzle.Cell[4, 1].SquareColorLetter = 'L';
            puzzle.Cell[4, 2].SquareColorLetter = 'W';
            puzzle.Cell[5, 0].SquareColorLetter = 'L';
            puzzle.Cell[5, 6].SquareColorLetter = 'W';
            puzzle.Cell[6, 0].SquareColorLetter = 'W';
            puzzle.Cell[6, 5].SquareColorLetter = 'L';
            puzzle.Cell[6, 6].SquareColorLetter = 'L';

            puzzle.Points[0, 7].OutEdges[puzzle.Points[0, 6]].MustTraverse = true;
            puzzle.Points[0, 7].OutEdges[puzzle.Points[1, 7]].MustTraverse = true;
            puzzle.Points[1, 5].OutEdges[puzzle.Points[2, 5]].MustTraverse = true;
            puzzle.Points[5, 7].OutEdges[puzzle.Points[6, 7]].MustTraverse = true;
            puzzle.Points[7, 0].OutEdges[puzzle.Points[6, 0]].MustTraverse = true;
            puzzle.Points[7, 0].OutEdges[puzzle.Points[7, 1]].MustTraverse = true;
            puzzle.Points[7, 4].OutEdges[puzzle.Points[6, 4]].MustTraverse = true;
            puzzle.Points[7, 4].OutEdges[puzzle.Points[7, 3]].MustTraverse = true;

            puzzle.Points[ends[1].X, ends[1].Y].End = true;
            puzzle.Points[ends[2].X, ends[2].Y].End = true;

            //puzzle.Cell[0, 2].StarColorLetter = 'R';

            return puzzle;
        }

        private Puzzle StartShed()
        {
            RectanglePuzzle puzzle = new RectanglePuzzle(3, 3, new Point(3, 0), new Point(0, 1));
            puzzle.Points[0, 2].MustTraverse = true;
            puzzle.Points[1, 2].MustTraverse = true;
            puzzle.Points[1, 0].MustTraverse = true;
            puzzle.Points[2, 1].MustTraverse = true;
            puzzle.Points[3, 1].MustTraverse = true;
            puzzle.Points[3, 3].MustTraverse = true;

            puzzle.Points[0, 0].OutEdges[puzzle.Points[1, 0]].MayTraverse = false;
            puzzle.Points[0, 0].OutEdges[puzzle.Points[0, 1]].MayTraverse = false;
            puzzle.Points[2, 1].OutEdges[puzzle.Points[3, 1]].MayTraverse = false;
            puzzle.Points[2, 2].OutEdges[puzzle.Points[2, 3]].MayTraverse = false;

            return puzzle;
        }

        private Puzzle NewPuzzle()
        {
            RectanglePuzzle puzzle = new RectanglePuzzle(3, 3, new Point(3, 0), new Point(0, 1));
            puzzle.Points[0, 2].MustTraverse = true;
            puzzle.Points[1, 2].MustTraverse = true;
            puzzle.Points[2, 1].MustTraverse = true;
            puzzle.Points[3, 1].MustTraverse = true;

            puzzle.Points[0, 0].OutEdges[puzzle.Points[1, 0]].MayTraverse = false;
            puzzle.Points[0, 0].OutEdges[puzzle.Points[0, 1]].MayTraverse = false;
            puzzle.Points[2, 1].OutEdges[puzzle.Points[3, 1]].MayTraverse = false;
            puzzle.Points[2, 2].OutEdges[puzzle.Points[2, 3]].MayTraverse = false;

            return puzzle;
        }

        private Puzzle MiddleChurch()
        {
            RectanglePuzzle puzzle = new RectanglePuzzle(4, 4, new Point(0, 4), new Point(4, 0));
            puzzle.Cell[0, 0].StarColorLetter = 'W';
            puzzle.Cell[1, 0].StarColorLetter = 'W';
            puzzle.Cell[2, 0].StarColorLetter = 'W';
            puzzle.Cell[3, 0].StarColorLetter = 'W';

            puzzle.Cell[0, 1].StarColorLetter = 'W';
            puzzle.Cell[1, 1].StarColorLetter = 'W';
            puzzle.Cell[2, 1].StarColorLetter = 'B';
            puzzle.Cell[3, 1].StarColorLetter = 'R';

            puzzle.Cell[0, 2].StarColorLetter = 'B';
            puzzle.Cell[1, 2].StarColorLetter = 'B';
            puzzle.Cell[2, 2].StarColorLetter = 'B';
            puzzle.Cell[3, 2].StarColorLetter = 'R';

            puzzle.Cell[0, 3].StarColorLetter = 'R';
            puzzle.Cell[1, 3].StarColorLetter = 'R';
            puzzle.Cell[2, 3].StarColorLetter = 'R';
            puzzle.Cell[3, 3].StarColorLetter = 'R';

            return puzzle;
        }

        private Puzzle SamplePuzzle()
        {
            RectanglePuzzle puzzle = new RectanglePuzzle(4, 4, new Point(0, 4), new Point(4, 0));

            puzzle.Cell[0, 1].SquareColorLetter = 'R';
            puzzle.Cell[0, 2].SquareColorLetter = 'R';
            puzzle.Cell[0, 3].SquareColorLetter = 'R';
            puzzle.Cell[1, 2].SquareColorLetter = 'R';
            puzzle.Cell[1, 3].SquareColorLetter = 'R';
            puzzle.Cell[2, 3].SquareColorLetter = 'R';

            puzzle.Cell[2, 2].SquareColorLetter = 'R';

            puzzle.Cell[1, 0].SquareColorLetter = 'B';
            puzzle.Cell[2, 0].SquareColorLetter = 'B';
            puzzle.Cell[3, 0].SquareColorLetter = 'B';
            puzzle.Cell[2, 1].SquareColorLetter = 'B';
            puzzle.Cell[3, 1].SquareColorLetter = 'B';
            puzzle.Cell[3, 2].SquareColorLetter = 'B';

            puzzle.Points[3, 1].OutEdges[puzzle.Points[3, 2]].MayTraverse = false;

            return puzzle;
        }
    }
}
