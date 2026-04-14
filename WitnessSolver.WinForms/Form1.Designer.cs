namespace WitnessSolver
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.btnSolve = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.lblSteps = new System.Windows.Forms.Label();
            this.lblRoutes = new System.Windows.Forms.Label();
            this.lblSolutions = new System.Windows.Forms.Label();
            this.lblElapsed = new System.Windows.Forms.Label();
            this.lblStatus = new System.Windows.Forms.Label();
            this.outputPanel = new System.Windows.Forms.Panel();
            this.cmbPuzzle = new System.Windows.Forms.ComboBox();
            this.lblStepsCaption = new System.Windows.Forms.Label();
            this.lblRoutesCaption = new System.Windows.Forms.Label();
            this.lblSolutionsCaption = new System.Windows.Forms.Label();
            this.rdoWatch = new System.Windows.Forms.RadioButton();
            this.rdoBrowse = new System.Windows.Forms.RadioButton();
            this.pnlBrowseControls = new System.Windows.Forms.Panel();
            this.trkSolution = new System.Windows.Forms.TrackBar();
            this.nudSolution = new System.Windows.Forms.NumericUpDown();
            this.lblBrowse = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // cmbPuzzle
            // 
            this.cmbPuzzle.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbPuzzle.Location = new System.Drawing.Point(12, 12);
            this.cmbPuzzle.Name = "cmbPuzzle";
            this.cmbPuzzle.Size = new System.Drawing.Size(160, 23);
            this.cmbPuzzle.TabIndex = 0;
            // 
            // btnSolve
            // 
            this.btnSolve.Location = new System.Drawing.Point(180, 11);
            this.btnSolve.Name = "btnSolve";
            this.btnSolve.Size = new System.Drawing.Size(75, 25);
            this.btnSolve.TabIndex = 1;
            this.btnSolve.Text = "Solve";
            this.btnSolve.UseVisualStyleBackColor = true;
            this.btnSolve.Click += new System.EventHandler(this.btnSolve_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Enabled = false;
            this.btnCancel.Location = new System.Drawing.Point(260, 11);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 25);
            this.btnCancel.TabIndex = 2;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // lblStepsCaption
            // 
            this.lblStepsCaption.AutoSize = true;
            this.lblStepsCaption.ForeColor = System.Drawing.Color.Gray;
            this.lblStepsCaption.Location = new System.Drawing.Point(350, 6);
            this.lblStepsCaption.Name = "lblStepsCaption";
            this.lblStepsCaption.Text = "Steps";
            // 
            // lblSteps
            // 
            this.lblSteps.AutoSize = true;
            this.lblSteps.Font = new System.Drawing.Font(System.Drawing.FontFamily.GenericMonospace, 9F);
            this.lblSteps.Location = new System.Drawing.Point(350, 22);
            this.lblSteps.Name = "lblSteps";
            this.lblSteps.Text = "0";
            // 
            // lblRoutesCaption
            // 
            this.lblRoutesCaption.AutoSize = true;
            this.lblRoutesCaption.ForeColor = System.Drawing.Color.Gray;
            this.lblRoutesCaption.Location = new System.Drawing.Point(460, 6);
            this.lblRoutesCaption.Name = "lblRoutesCaption";
            this.lblRoutesCaption.Text = "Routes";
            // 
            // lblRoutes
            // 
            this.lblRoutes.AutoSize = true;
            this.lblRoutes.Font = new System.Drawing.Font(System.Drawing.FontFamily.GenericMonospace, 9F);
            this.lblRoutes.Location = new System.Drawing.Point(460, 22);
            this.lblRoutes.Name = "lblRoutes";
            this.lblRoutes.Text = "0";
            // 
            // lblSolutionsCaption
            // 
            this.lblSolutionsCaption.AutoSize = true;
            this.lblSolutionsCaption.ForeColor = System.Drawing.Color.Gray;
            this.lblSolutionsCaption.Location = new System.Drawing.Point(570, 6);
            this.lblSolutionsCaption.Name = "lblSolutionsCaption";
            this.lblSolutionsCaption.Text = "Solutions";
            // 
            // lblSolutions
            // 
            this.lblSolutions.AutoSize = true;
            this.lblSolutions.Font = new System.Drawing.Font(System.Drawing.FontFamily.GenericMonospace, 9F, System.Drawing.FontStyle.Bold);
            this.lblSolutions.ForeColor = System.Drawing.Color.DarkGreen;
            this.lblSolutions.Location = new System.Drawing.Point(570, 22);
            this.lblSolutions.Name = "lblSolutions";
            this.lblSolutions.Text = "0";
            // 
            // lblElapsed
            // 
            this.lblElapsed.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            this.lblElapsed.Font = new System.Drawing.Font(System.Drawing.FontFamily.GenericMonospace, 10F);
            this.lblElapsed.Location = new System.Drawing.Point(700, 14);
            this.lblElapsed.Name = "lblElapsed";
            this.lblElapsed.Size = new System.Drawing.Size(86, 20);
            this.lblElapsed.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.lblElapsed.Text = "";
            // 
            // outputPanel
            // 
            this.outputPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.outputPanel.BackColor = System.Drawing.Color.FromArgb(40, 40, 45);
            this.outputPanel.Location = new System.Drawing.Point(12, 42);
            this.outputPanel.Name = "outputPanel";
            this.outputPanel.Size = new System.Drawing.Size(774, 610);
            this.outputPanel.TabIndex = 10;
            // 
            // lblStatus
            // 
            this.lblStatus.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            this.lblStatus.ForeColor = System.Drawing.Color.DimGray;
            this.lblStatus.Location = new System.Drawing.Point(12, 688);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(774, 20);
            this.lblStatus.Text = "Select a puzzle and click Solve.";
            // 
            // rdoWatch
            // 
            this.rdoWatch.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            this.rdoWatch.AutoSize = true;
            this.rdoWatch.Checked = true;
            this.rdoWatch.Location = new System.Drawing.Point(12, 658);
            this.rdoWatch.Name = "rdoWatch";
            this.rdoWatch.Text = "Watch";
            this.rdoWatch.CheckedChanged += new System.EventHandler(this.rdoWatch_CheckedChanged);
            // 
            // rdoBrowse
            // 
            this.rdoBrowse.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            this.rdoBrowse.AutoSize = true;
            this.rdoBrowse.Enabled = false;
            this.rdoBrowse.Location = new System.Drawing.Point(80, 658);
            this.rdoBrowse.Name = "rdoBrowse";
            this.rdoBrowse.Text = "Browse";
            this.rdoBrowse.CheckedChanged += new System.EventHandler(this.rdoBrowse_CheckedChanged);
            // 
            // pnlBrowseControls
            // 
            this.pnlBrowseControls.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            this.pnlBrowseControls.Location = new System.Drawing.Point(160, 655);
            this.pnlBrowseControls.Size = new System.Drawing.Size(626, 28);
            this.pnlBrowseControls.Visible = false;
            this.pnlBrowseControls.Controls.Add(this.trkSolution);
            this.pnlBrowseControls.Controls.Add(this.nudSolution);
            this.pnlBrowseControls.Controls.Add(this.lblBrowse);
            // 
            // trkSolution
            // 
            this.trkSolution.Location = new System.Drawing.Point(0, 0);
            this.trkSolution.Size = new System.Drawing.Size(300, 28);
            this.trkSolution.TickStyle = System.Windows.Forms.TickStyle.None;
            this.trkSolution.ValueChanged += new System.EventHandler(this.trkSolution_ValueChanged);
            // 
            // nudSolution
            // 
            this.nudSolution.Location = new System.Drawing.Point(305, 3);
            this.nudSolution.Size = new System.Drawing.Size(80, 23);
            this.nudSolution.Minimum = 1;
            this.nudSolution.Maximum = 1;
            this.nudSolution.ValueChanged += new System.EventHandler(this.nudSolution_ValueChanged);
            // 
            // lblBrowse
            // 
            this.lblBrowse.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.lblBrowse.ForeColor = System.Drawing.Color.DimGray;
            this.lblBrowse.Location = new System.Drawing.Point(390, 5);
            this.lblBrowse.Size = new System.Drawing.Size(236, 18);
            this.lblBrowse.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblBrowse.Text = "";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 715);
            this.Controls.Add(this.pnlBrowseControls);
            this.Controls.Add(this.rdoBrowse);
            this.Controls.Add(this.rdoWatch);
            this.Controls.Add(this.lblStatus);
            this.Controls.Add(this.lblElapsed);
            this.Controls.Add(this.lblSolutionsCaption);
            this.Controls.Add(this.lblRoutesCaption);
            this.Controls.Add(this.lblStepsCaption);
            this.Controls.Add(this.cmbPuzzle);
            this.Controls.Add(this.outputPanel);
            this.Controls.Add(this.lblSolutions);
            this.Controls.Add(this.lblRoutes);
            this.Controls.Add(this.lblSteps);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSolve);
            this.Name = "Form1";
            this.Text = "Witness Solver";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Button btnSolve;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label lblSteps;
        private System.Windows.Forms.Label lblRoutes;
        private System.Windows.Forms.Label lblSolutions;
        private System.Windows.Forms.Label lblElapsed;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.Label lblStepsCaption;
        private System.Windows.Forms.Label lblRoutesCaption;
        private System.Windows.Forms.Label lblSolutionsCaption;
        private System.Windows.Forms.Panel outputPanel;
        private System.Windows.Forms.ComboBox cmbPuzzle;
        private System.Windows.Forms.RadioButton rdoWatch;
        private System.Windows.Forms.RadioButton rdoBrowse;
        private System.Windows.Forms.Panel pnlBrowseControls;
        private System.Windows.Forms.TrackBar trkSolution;
        private System.Windows.Forms.NumericUpDown nudSolution;
        private System.Windows.Forms.Label lblBrowse;
    }
}

