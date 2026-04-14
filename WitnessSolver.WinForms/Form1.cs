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

        // Watch mode: flash solutions
        private int _lastSeenSolutionCount;
        private int[] _flashRoute;
        private DateTime _flashUntil;

        public Form1()
        {
            this.InitializeComponent();
            this.outputPanel.Paint += OutputPanel_Paint;
            this.outputPanel.DoubleBuffered(true);
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
            this._lastSeenSolutionCount = 0;
            this._flashRoute = null;
            this._cts = new CancellationTokenSource();

            var entry = (PuzzleEntry)this.cmbPuzzle.SelectedItem;
            this._expectedSolutions = entry.HasExpectedSolutions ? entry.ExpectedSolutions : -1;
            var puzzle = entry.Factory();
            this._graph = SolverGraph.Compile(puzzle);
            this._graph.CancellationToken = this._cts.Token;

            this._drawer = new RectanglePuzzleDrawer();
            this._drawer.SetGraph(this._graph);

            SolverSnapshot.Publish(null);

            this.lblSteps.Text = "0";
            this.lblRoutes.Text = "0";
            this.lblSolutions.Text = ExpectedText(0);
            this.lblStatus.Text = "Solving...";
            this.SetMode(false);
            this.rdoWatch.Checked = true;
            this.rdoBrowse.Enabled = false;

            this.outputPanel.Invalidate();

            this._stopwatch = Stopwatch.StartNew();
            this._renderTimer = new System.Windows.Forms.Timer { Interval = 33 };
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

            // Enable browse once solutions exist
            if (snapshot.SolutionsFound > 0 && !this.rdoBrowse.Enabled)
                this.rdoBrowse.Enabled = true;

            // Watch mode: check for new solutions to flash
            if (!this._browseMode && snapshot.SolutionsFound > this._lastSeenSolutionCount)
            {
                var latestRoute = this._graph.Solutions.Get(this._graph.Solutions.StoredCount - 1);
                if (latestRoute != null)
                {
                    this._flashRoute = latestRoute;
                    this._flashUntil = DateTime.UtcNow.AddMilliseconds(500);
                }
                this._lastSeenSolutionCount = snapshot.SolutionsFound;
            }

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
            else if (this._flashRoute != null && DateTime.UtcNow < this._flashUntil)
            {
                route = this._flashRoute;
                routeColor = Color.Green;
            }
            else
            {
                this._flashRoute = null;
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
                    this.rdoBrowse.Enabled = true;
                    this.rdoBrowse.Checked = true;
                    SetMode(true);
                }
            }

            var finalSnapshot = SolverSnapshot.Current;
            if (finalSnapshot != null)
            {
                this.lblSteps.Text = finalSnapshot.StepCount.ToString("N0");
                this.lblRoutes.Text = finalSnapshot.RoutesFound.ToString("N0");
            }
            this.outputPanel.Invalidate();
        }

        private void SetMode(bool browse)
        {
            this._browseMode = browse;
            this.pnlBrowseControls.Visible = browse;

            if (browse)
            {
                int stored = this._graph?.Solutions?.StoredCount ?? 0;
                this.trkSolution.Minimum = 0;
                this.trkSolution.Maximum = Math.Max(0, stored - 1);
                this.trkSolution.Value = this._browseIndex;
                this.nudSolution.Minimum = 1;
                this.nudSolution.Maximum = stored;
                this.nudSolution.Value = this._browseIndex + 1;
                UpdateBrowseLabel();
            }

            this.outputPanel.Invalidate();
        }

        private void UpdateBrowseLabel()
        {
            int stored = this._graph?.Solutions?.StoredCount ?? 0;
            int total = this._graph?.Solutions?.TotalFound ?? 0;
            this.lblBrowse.Text = stored > 0
                ? $"Solution {this._browseIndex + 1} of {stored:N0}" + (total > stored ? $" (sampled from {total:N0})" : "")
                : "";
        }

        private void rdoWatch_CheckedChanged(object sender, EventArgs e)
        {
            if (this.rdoWatch.Checked) SetMode(false);
        }

        private void rdoBrowse_CheckedChanged(object sender, EventArgs e)
        {
            if (this.rdoBrowse.Checked) SetMode(true);
        }

        private void trkSolution_ValueChanged(object sender, EventArgs e)
        {
            this._browseIndex = this.trkSolution.Value;
            this.nudSolution.Value = this._browseIndex + 1;
            UpdateBrowseLabel();
            this.outputPanel.Invalidate();
        }

        private void nudSolution_ValueChanged(object sender, EventArgs e)
        {
            this._browseIndex = (int)this.nudSolution.Value - 1;
            this.trkSolution.Value = this._browseIndex;
            UpdateBrowseLabel();
            this.outputPanel.Invalidate();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this._cts?.Cancel();
            this.btnCancel.Enabled = false;
            this.lblStatus.Text = "Cancelling...";
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

    // Extension to enable double-buffering on Panel
    static class PanelExtensions
    {
        public static void DoubleBuffered(this Panel panel, bool value)
        {
            typeof(Panel).GetProperty("DoubleBuffered",
                System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic)
                ?.SetValue(panel, value);
        }
    }
}
