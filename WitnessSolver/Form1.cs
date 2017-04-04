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
            this.InitializeComponent();
        }

        private void btnSolve_Click(object sender, EventArgs e)
        {
            var puzzle = (Puzzle)this.cmbPuzzle.SelectedItem;

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

        private void Form1_Load(object sender, EventArgs e)
        {
            foreach (var puzzle in Puzzles.AllPuzzles())
            {
                this.cmbPuzzle.Items.Add(puzzle);
            }
        }
    }
}
