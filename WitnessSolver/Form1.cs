using System;
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
            this.btnSolve.Enabled = false;
            var puzzle = (Puzzle)this.cmbPuzzle.SelectedItem;

            puzzle.Drawer.FormGraphics = this.outputPanel.CreateGraphics();
            puzzle.Drawer.DrawState(false);
            puzzle.Update += this.puzzle_OnUpdate;
            var solver = new PuzzleSolver { Puzzle = puzzle };

            Task.Run(() => solver.Solve()).ContinueWith((res) =>
            {
                MessageBox.Show(String.Format("Found {0} solutions.", res.Result.ToString()));
            });
        }

        private void puzzle_OnUpdate(object sender, Puzzle.PuzzleSolveEventArgs e)
        {
            if (this.Disposing || this.IsDisposed)
            {
                return;
            }

            if (this.InvokeRequired && !this.Disposing && !this.IsDisposed)
            {
                this.Invoke(new MethodInvoker(delegate { puzzle_OnUpdate(sender, e); }));
                return;
            }

            this.lblRoutes.Text = e.RoutesFound.ToString();
            this.lblSteps.Text = e.EdgesAdded.ToString();
            this.lblSolutions.Text = e.SolutionsFound.ToString();

            if (e.IsDone)
            {
                this.btnSolve.Enabled = true;
            }
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
