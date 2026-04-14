using System;
using System.Diagnostics;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WitnessSolver
{
    public partial class Form1 : Form
    {
        private CancellationTokenSource _cts;
        private Stopwatch _stopwatch;
        private System.Windows.Forms.Timer _renderTimer;
        private RectanglePuzzleDrawer _drawer;
        private SolverGraph _graph;
        private bool _browseMode;
        private int _browseIndex;
        private long _expectedSolutions = -1;

        public Form1()
        {
            this.InitializeComponent();
            this.outputPanel.Paint += OutputPanel_Paint;
        }

        private void btnSolve_Click(object sender, EventArgs e)
        {
            if (this.cmbPuzzle.SelectedItem == null) return;

            // Stop any previous solve
            this._renderTimer?.Stop();
            this._cts?.Cancel();

            this.btnSolve.Enabled = false;
            this.btnCancel.Enabled = true;
            this._browseMode = false;
            this._browseIndex = 0;
            this._cts = new CancellationTokenSource();

            var entry = (PuzzleEntry)this.cmbPuzzle.SelectedItem;
            this._expectedSolutions = entry.HasExpectedSolutions ? entry.ExpectedSolutions : -1;
            var puzzle = entry.Factory();
            this._graph = SolverGraph.Compile(puzzle);
            this._graph.CancellationToken = this._cts.Token;

            this._drawer = new RectanglePuzzleDrawer();
            this._drawer.SetGraph(this._graph);

            // Reset shared snapshot
            SolverSnapshot.Publish(null);

            this.lblSteps.Text = "0";
            this.lblRoutes.Text = "0";
            this.lblSolutions.Text = ExpectedText(0);
            this.lblStatus.Text = "Solving...";
            this.btnBrowse.Enabled = false;
            this.btnBrowse.Text = "Browse";
            this.btnPrev.Enabled = false;
            this.btnNext.Enabled = false;
            this.lblBrowse.Text = "";

            // Draw initial static board
            this.outputPanel.Invalidate();

            this._stopwatch = Stopwatch.StartNew();
            this._renderTimer = new System.Windows.Forms.Timer { Interval = 33 }; // ~30fps
            this._renderTimer.Tick += RenderTick;
            this._renderTimer.Start();

            var solver = new PuzzleSolver(this._graph);
            Task.Run(() => solver.Solve(), this._cts.Token).ContinueWith(res =>
            {
                if (this.IsDisposed || this.Disposing) return;
                this.Invoke(new MethodInvoker(() => OnSolveComplete(res)));
            });
        }

        private void RenderTick(object sender, EventArgs e)
        {
            var snapshot = SolverSnapshot.Current;
            if (snapshot == null || this._drawer == null) return;

            this.lblSteps.Text = snapshot.StepCount.ToString("N0");
            this.lblRoutes.Text = snapshot.RoutesFound.ToString("N0");
            this.lblSolutions.Text = ExpectedText(snapshot.SolutionsFound);
            this.lblElapsed.Text = this._stopwatch.Elapsed.ToString(@"mm\:ss\.f");

            // Enable browse once we have solutions
            if (snapshot.SolutionsFound > 0 && !this.btnBrowse.Enabled)
                this.btnBrowse.Enabled = true;

            if (!this._browseMode)
                this.outputPanel.Invalidate();
        }

        private void OutputPanel_Paint(object sender, PaintEventArgs e)
        {
            if (this._drawer == null || this._graph == null)
            {
                e.Graphics.Clear(PuzzleDrawer.BackgroundColor);
                return;
            }

            int[] route = null;
            Color routeColor = Color.DarkRed;

            if (this._browseMode)
            {
                route = this._graph.Solutions.Get(this._browseIndex);
                routeColor = Color.Green;
            }
            else
            {
                var snapshot = SolverSnapshot.Current;
                if (snapshot != null)
                    route = snapshot.RouteEdgeIndices;
            }

            this._drawer.DrawRoute(e.Graphics, route ?? new int[0], routeColor);
        }

        private void OnSolveComplete(Task<int> res)
        {
            this._renderTimer.Stop();
            this._stopwatch.Stop();

            this.btnSolve.Enabled = true;
            this.btnCancel.Enabled = false;
            this.lblElapsed.Text = this._stopwatch.Elapsed.ToString(@"mm\:ss\.fff");

            if (res.IsFaulted && res.Exception?.InnerException is OperationCanceledException)
            {
                this.lblStatus.Text = "Cancelled.";
            }
            else if (res.IsFaulted)
            {
                this.lblStatus.Text = $"Error: {res.Exception?.InnerException?.Message}";
            }
            else
            {
                int total = res.Result;
                int stored = this._graph.Solutions.StoredCount;
                this.lblSolutions.Text = ExpectedText(total);
                this.lblStatus.Text = $"Done — {total:N0} solutions ({stored:N0} stored).";

                if (stored > 0)
                {
                    this.btnBrowse.Enabled = true;
                    SwitchToBrowse();
                }
            }

            // Final render
            var finalSnapshot = SolverSnapshot.Current;
            if (finalSnapshot != null)
            {
                this.lblSteps.Text = finalSnapshot.StepCount.ToString("N0");
                this.lblRoutes.Text = finalSnapshot.RoutesFound.ToString("N0");
            }
        }

        private void SwitchToBrowse()
        {
            this._browseMode = true;
            this._browseIndex = 0;
            UpdateBrowseUI();
            this.outputPanel.Invalidate();
        }

        private void UpdateBrowseUI()
        {
            int stored = this._graph?.Solutions?.StoredCount ?? 0;
            this.btnPrev.Enabled = this._browseIndex > 0;
            this.btnNext.Enabled = this._browseIndex < stored - 1;
            this.lblBrowse.Text = stored > 0
                ? $"Solution {this._browseIndex + 1} of {stored:N0}"
                : "";
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this._cts?.Cancel();
            this.btnCancel.Enabled = false;
            this.lblStatus.Text = "Cancelling...";
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            if (this._browseMode)
            {
                this._browseMode = false;
                this.btnBrowse.Text = "Browse";
                this.btnPrev.Enabled = false;
                this.btnNext.Enabled = false;
                this.lblBrowse.Text = "";
            }
            else
            {
                SwitchToBrowse();
                this.btnBrowse.Text = "Watch";
            }
            this.outputPanel.Invalidate();
        }

        private void btnPrev_Click(object sender, EventArgs e)
        {
            if (this._browseIndex > 0)
            {
                this._browseIndex--;
                UpdateBrowseUI();
                this.outputPanel.Invalidate();
            }
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            int stored = this._graph?.Solutions?.StoredCount ?? 0;
            if (this._browseIndex < stored - 1)
            {
                this._browseIndex++;
                UpdateBrowseUI();
                this.outputPanel.Invalidate();
            }
        }

        private string ExpectedText(int found)
        {
            if (this._expectedSolutions >= 0)
                return $"{found:N0} / {this._expectedSolutions:N0}";
            return found.ToString("N0");
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            foreach (var entry in PuzzleCatalog.All)
                this.cmbPuzzle.Items.Add(entry);
            if (this.cmbPuzzle.Items.Count > 0)
                this.cmbPuzzle.SelectedIndex = 0;
        }
    }
}
