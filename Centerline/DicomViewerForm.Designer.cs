namespace Centerline
{
    partial class DicomViewerForm
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
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.Cut_label = new System.Windows.Forms.Label();
            this.SetSecondPoint_button = new System.Windows.Forms.Button();
            this.SetFirstPoint_button = new System.Windows.Forms.Button();
            this.Done_button = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.Cursor = System.Windows.Forms.Cursors.Default;
            this.pictureBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBox1.Location = new System.Drawing.Point(0, 0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(875, 765);
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.MouseClick += new System.Windows.Forms.MouseEventHandler(this.pictureBox1_MouseClick);
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
            this.splitContainer1.Panel2.Controls.Add(this.Done_button);
            this.splitContainer1.Panel2.Controls.Add(this.Cut_label);
            this.splitContainer1.Panel2.Controls.Add(this.SetSecondPoint_button);
            this.splitContainer1.Panel2.Controls.Add(this.SetFirstPoint_button);
            this.splitContainer1.Size = new System.Drawing.Size(1001, 765);
            this.splitContainer1.SplitterDistance = 875;
            this.splitContainer1.TabIndex = 1;
            // 
            // Cut_label
            // 
            this.Cut_label.AutoSize = true;
            this.Cut_label.Location = new System.Drawing.Point(24, 13);
            this.Cut_label.Name = "Cut_label";
            this.Cut_label.Size = new System.Drawing.Size(25, 17);
            this.Cut_label.TabIndex = 2;
            this.Cut_label.Text = "Z: ";
            // 
            // SetSecondPoint_button
            // 
            this.SetSecondPoint_button.Location = new System.Drawing.Point(6, 101);
            this.SetSecondPoint_button.Name = "SetSecondPoint_button";
            this.SetSecondPoint_button.Size = new System.Drawing.Size(107, 49);
            this.SetSecondPoint_button.TabIndex = 1;
            this.SetSecondPoint_button.Text = "Set Second Point";
            this.SetSecondPoint_button.UseVisualStyleBackColor = true;
            this.SetSecondPoint_button.Click += new System.EventHandler(this.SetSecondPoint_button_Click);
            // 
            // SetFirstPoint_button
            // 
            this.SetFirstPoint_button.Location = new System.Drawing.Point(6, 52);
            this.SetFirstPoint_button.Name = "SetFirstPoint_button";
            this.SetFirstPoint_button.Size = new System.Drawing.Size(107, 43);
            this.SetFirstPoint_button.TabIndex = 0;
            this.SetFirstPoint_button.Text = "Set First Point";
            this.SetFirstPoint_button.UseVisualStyleBackColor = true;
            this.SetFirstPoint_button.Click += new System.EventHandler(this.SetFirstPoint_button_Click);
            // 
            // Done_button
            // 
            this.Done_button.Location = new System.Drawing.Point(6, 156);
            this.Done_button.Name = "Done_button";
            this.Done_button.Size = new System.Drawing.Size(107, 49);
            this.Done_button.TabIndex = 3;
            this.Done_button.Text = "Done";
            this.Done_button.UseVisualStyleBackColor = true;
            this.Done_button.Click += new System.EventHandler(this.Done_button_Click);
            // 
            // DicomViewerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1001, 765);
            this.Controls.Add(this.splitContainer1);
            this.Name = "DicomViewerForm";
            this.Text = "DicomViewerForm";
            this.Load += new System.EventHandler(this.DicomViewerForm_Load);
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
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Label Cut_label;
        private System.Windows.Forms.Button SetSecondPoint_button;
        private System.Windows.Forms.Button SetFirstPoint_button;
        private System.Windows.Forms.Button Done_button;
    }
}