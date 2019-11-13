namespace Solver
{
    partial class Maze
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
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.label_StartPos = new System.Windows.Forms.Label();
            this.button_solveMaze = new System.Windows.Forms.Button();
            this.label_endPos = new System.Windows.Forms.Label();
            this.buttonReset = new System.Windows.Forms.Button();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pictureBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBox1.Image = global::Solver.Properties.Resources.Maze;
            this.pictureBox1.InitialImage = null;
            this.pictureBox1.Location = new System.Drawing.Point(0, 0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(669, 592);
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.MouseClick += new System.Windows.Forms.MouseEventHandler(this.pictureBox1_MouseClick);
            this.pictureBox1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pictureBox1_MouseMove);
            // 
            // label_StartPos
            // 
            this.label_StartPos.AutoSize = true;
            this.label_StartPos.Location = new System.Drawing.Point(12, 73);
            this.label_StartPos.Name = "label_StartPos";
            this.label_StartPos.Size = new System.Drawing.Size(30, 13);
            this.label_StartPos.TabIndex = 1;
            this.label_StartPos.Text = "X: Y:";
            // 
            // button_solveMaze
            // 
            this.button_solveMaze.Location = new System.Drawing.Point(15, 3);
            this.button_solveMaze.Name = "button_solveMaze";
            this.button_solveMaze.Size = new System.Drawing.Size(75, 23);
            this.button_solveMaze.TabIndex = 2;
            this.button_solveMaze.Text = "Solve";
            this.button_solveMaze.UseVisualStyleBackColor = true;
            this.button_solveMaze.Click += new System.EventHandler(this.button_solveMaze_Click);
            // 
            // label_endPos
            // 
            this.label_endPos.AutoSize = true;
            this.label_endPos.Location = new System.Drawing.Point(12, 106);
            this.label_endPos.Name = "label_endPos";
            this.label_endPos.Size = new System.Drawing.Size(30, 13);
            this.label_endPos.TabIndex = 3;
            this.label_endPos.Text = "X: Y:";
            // 
            // buttonReset
            // 
            this.buttonReset.Location = new System.Drawing.Point(15, 35);
            this.buttonReset.Name = "buttonReset";
            this.buttonReset.Size = new System.Drawing.Size(75, 23);
            this.buttonReset.TabIndex = 4;
            this.buttonReset.Text = "Reset";
            this.buttonReset.UseVisualStyleBackColor = true;
            this.buttonReset.Click += new System.EventHandler(this.buttonReset_Click);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.pictureBox1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.label_StartPos);
            this.splitContainer1.Panel2.Controls.Add(this.buttonReset);
            this.splitContainer1.Panel2.Controls.Add(this.label_endPos);
            this.splitContainer1.Panel2.Controls.Add(this.button_solveMaze);
            this.splitContainer1.Size = new System.Drawing.Size(791, 592);
            this.splitContainer1.SplitterDistance = 669;
            this.splitContainer1.TabIndex = 5;
            // 
            // Maze
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(791, 592);
            this.Controls.Add(this.splitContainer1);
            this.Name = "Maze";
            this.Text = "Maze";
            this.Load += new System.EventHandler(this.Maze_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label label_StartPos;
        private System.Windows.Forms.Button button_solveMaze;
        private System.Windows.Forms.Label label_endPos;
        private System.Windows.Forms.Button buttonReset;
        private System.Windows.Forms.SplitContainer splitContainer1;
    }
}