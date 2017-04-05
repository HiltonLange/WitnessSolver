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
            this.lblSteps = new System.Windows.Forms.Label();
            this.lblRoutes = new System.Windows.Forms.Label();
            this.lblSolutions = new System.Windows.Forms.Label();
            this.outputPanel = new System.Windows.Forms.Panel();
            this.cmbPuzzle = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // btnSolve
            // 
            this.btnSolve.Location = new System.Drawing.Point(140, 13);
            this.btnSolve.Name = "btnSolve";
            this.btnSolve.Size = new System.Drawing.Size(75, 23);
            this.btnSolve.TabIndex = 0;
            this.btnSolve.Text = "Solve";
            this.btnSolve.UseVisualStyleBackColor = true;
            this.btnSolve.Click += new System.EventHandler(this.btnSolve_Click);
            // 
            // lblSteps
            // 
            this.lblSteps.AutoSize = true;
            this.lblSteps.Location = new System.Drawing.Point(221, 18);
            this.lblSteps.Name = "lblSteps";
            this.lblSteps.Size = new System.Drawing.Size(44, 13);
            this.lblSteps.TabIndex = 1;
            this.lblSteps.Text = "lblSteps";
            // 
            // lblRoutes
            // 
            this.lblRoutes.AutoSize = true;
            this.lblRoutes.Location = new System.Drawing.Point(294, 18);
            this.lblRoutes.Name = "lblRoutes";
            this.lblRoutes.Size = new System.Drawing.Size(51, 13);
            this.lblRoutes.TabIndex = 2;
            this.lblRoutes.Text = "lblRoutes";
            // 
            // lblSolutions
            // 
            this.lblSolutions.AutoSize = true;
            this.lblSolutions.Location = new System.Drawing.Point(387, 18);
            this.lblSolutions.Name = "lblSolutions";
            this.lblSolutions.Size = new System.Drawing.Size(60, 13);
            this.lblSolutions.TabIndex = 3;
            this.lblSolutions.Text = "lblSolutions";
            // 
            // outputPanel
            // 
            this.outputPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.outputPanel.Location = new System.Drawing.Point(13, 42);
            this.outputPanel.Name = "outputPanel";
            this.outputPanel.Size = new System.Drawing.Size(844, 558);
            this.outputPanel.TabIndex = 4;
            // 
            // cmbPuzzle
            // 
            this.cmbPuzzle.FormattingEnabled = true;
            this.cmbPuzzle.Location = new System.Drawing.Point(13, 12);
            this.cmbPuzzle.Name = "cmbPuzzle";
            this.cmbPuzzle.Size = new System.Drawing.Size(121, 21);
            this.cmbPuzzle.TabIndex = 5;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(869, 612);
            this.Controls.Add(this.cmbPuzzle);
            this.Controls.Add(this.outputPanel);
            this.Controls.Add(this.lblSolutions);
            this.Controls.Add(this.lblRoutes);
            this.Controls.Add(this.lblSteps);
            this.Controls.Add(this.btnSolve);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnSolve;
        private System.Windows.Forms.Label lblSteps;
        private System.Windows.Forms.Label lblRoutes;
        private System.Windows.Forms.Label lblSolutions;
        private System.Windows.Forms.Panel outputPanel;
        private System.Windows.Forms.ComboBox cmbPuzzle;
    }
}

