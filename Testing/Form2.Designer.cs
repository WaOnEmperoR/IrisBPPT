namespace Testing
{
    partial class Form2
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
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.openImageToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.dilationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.representationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.superimposeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.segmentationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.normalizationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.unwrapToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openImageToolStripMenuItem,
            this.dilationToolStripMenuItem,
            this.representationToolStripMenuItem,
            this.superimposeToolStripMenuItem,
            this.segmentationToolStripMenuItem,
            this.normalizationToolStripMenuItem,
            this.unwrapToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(924, 33);
            this.menuStrip1.TabIndex = 4;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // openImageToolStripMenuItem
            // 
            this.openImageToolStripMenuItem.Name = "openImageToolStripMenuItem";
            this.openImageToolStripMenuItem.Size = new System.Drawing.Size(123, 29);
            this.openImageToolStripMenuItem.Text = "Open Image";
            this.openImageToolStripMenuItem.Click += new System.EventHandler(this.openImageToolStripMenuItem_Click);
            // 
            // dilationToolStripMenuItem
            // 
            this.dilationToolStripMenuItem.Name = "dilationToolStripMenuItem";
            this.dilationToolStripMenuItem.Size = new System.Drawing.Size(85, 29);
            this.dilationToolStripMenuItem.Text = "Dilation";
            // 
            // representationToolStripMenuItem
            // 
            this.representationToolStripMenuItem.Name = "representationToolStripMenuItem";
            this.representationToolStripMenuItem.Size = new System.Drawing.Size(143, 29);
            this.representationToolStripMenuItem.Text = "Representation";
            this.representationToolStripMenuItem.Click += new System.EventHandler(this.representationToolStripMenuItem_Click);
            // 
            // superimposeToolStripMenuItem
            // 
            this.superimposeToolStripMenuItem.Name = "superimposeToolStripMenuItem";
            this.superimposeToolStripMenuItem.Size = new System.Drawing.Size(129, 29);
            this.superimposeToolStripMenuItem.Text = "Superimpose";
            // 
            // segmentationToolStripMenuItem
            // 
            this.segmentationToolStripMenuItem.Name = "segmentationToolStripMenuItem";
            this.segmentationToolStripMenuItem.Size = new System.Drawing.Size(135, 29);
            this.segmentationToolStripMenuItem.Text = "Segmentation";
            // 
            // normalizationToolStripMenuItem
            // 
            this.normalizationToolStripMenuItem.Name = "normalizationToolStripMenuItem";
            this.normalizationToolStripMenuItem.Size = new System.Drawing.Size(135, 29);
            this.normalizationToolStripMenuItem.Text = "Normalization";
            // 
            // unwrapToolStripMenuItem
            // 
            this.unwrapToolStripMenuItem.Name = "unwrapToolStripMenuItem";
            this.unwrapToolStripMenuItem.Size = new System.Drawing.Size(85, 29);
            this.unwrapToolStripMenuItem.Text = "Unwrap";
            // 
            // Form2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(924, 469);
            this.Controls.Add(this.menuStrip1);
            this.Name = "Form2";
            this.Text = "Form2";
            this.Load += new System.EventHandler(this.Form2_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem openImageToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem dilationToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem representationToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem superimposeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem segmentationToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem normalizationToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem unwrapToolStripMenuItem;
    }
}