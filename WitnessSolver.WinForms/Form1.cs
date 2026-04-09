using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WitnessSolver
{
    public partial class Form1 : Form
    {
        private CancellationTokenSource _cts;
        private Stopwatch _stopwatch;
        private System.Windows.Forms.Timer _timer;

        public Form1()
        {
            this.InitializeComponent();
        }

        private void btnSolve_Click(object sender, EventArgs e)
        {
            if (this.cmbPuzzle.SelectedItem == null)
            {
                MessageBox.Show("Select a puzzle first.", "Witness Solver", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            this.btnSolve.Enabled = false;
            this.btnCancel.Enabled = true;
            this._cts = new CancellationTokenSource();

            var puzzle = (Puzzle)this.cmbPuzzle.SelectedItem;
            var graph = SolverGraph.Compile(puzzle);

            var drawer = new RectanglePuzzleDrawer();
            drawer.FormGraphics = this.outputPanel.CreateGraphics();
            drawer.SetGraph(graph);
            graph.Drawer = drawer;
            graph.CancellationToken = this._cts.Token;

            this.outputPanel.Invalidate();
            this.outputPanel.Update();
            drawer.DrawState(false);
            graph.Update += this.OnSolveUpdate;

            this._stopwatch = Stopwatch.StartNew();
            this._timer = new System.Windows.Forms.Timer { Interval = 100 };
            this._timer.Tick += (s, ev) => this.lblElapsed.Text = this._stopwatch.Elapsed.ToString(@"mm\:ss\.f");
            this._timer.Start();

            this.lblSteps.Text = "0";
            this.lblRoutes.Text = "0";
            this.lblSolutions.Text = "0";

            var solver = new PuzzleSolver(graph);
            var ct = this._cts.Token;

            Task.Run(() => solver.Solve(), ct).ContinueWith(res =>
            {
                this._timer.Stop();
                this._stopwatch.Stop();

                if (this.IsDisposed || this.Disposing) return;

                this.Invoke(new MethodInvoker(() =>
                {
                    this.btnSolve.Enabled = true;
                    this.btnCancel.Enabled = false;
                    this.lblElapsed.Text = this._stopwatch.Elapsed.ToString(@"mm\:ss\.fff");

                    if (res.IsFaulted && res.Exception?.InnerException is OperationCanceledException)
                        this.lblStatus.Text = "Cancelled.";
                    else if (res.IsFaulted)
                        this.lblStatus.Text = $"Error: {res.Exception?.InnerException?.Message}";
                    else
                        this.lblStatus.Text = $"Done — {res.Result} solutions found.";
                }));
            });
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this._cts?.Cancel();
            this.btnCancel.Enabled = false;
            this.lblStatus.Text = "Cancelling...";
        }

        private void OnSolveUpdate(object sender, SolverGraph.SolveEventArgs e)
        {
            if (this.Disposing || this.IsDisposed) return;

            if (this.InvokeRequired)
            {
                try { this.Invoke(new MethodInvoker(() => OnSolveUpdate(sender, e))); }
                catch (ObjectDisposedException) { }
                return;
            }

            this.lblSteps.Text = e.EdgesAdded.ToString("N0");
            this.lblRoutes.Text = e.RoutesFound.ToString("N0");
            this.lblSolutions.Text = e.SolutionsFound.ToString("N0");

            if (e.IsDone)
            {
                this.btnSolve.Enabled = true;
                this.btnCancel.Enabled = false;
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            foreach (var puzzle in Puzzles.AllPuzzles())
            {
                this.cmbPuzzle.Items.Add(puzzle);
            }

            if (this.cmbPuzzle.Items.Count > 0)
                this.cmbPuzzle.SelectedIndex = 0;
        }
    }
}
