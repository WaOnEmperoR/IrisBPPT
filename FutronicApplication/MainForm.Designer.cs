namespace FutronicApplication
{
    partial class MainForm
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
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.enrollToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.verifyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.templateToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.compactToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.isoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.bPPTToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.label1 = new System.Windows.Forms.Label();
            this.tbImageQuality = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.tbScore = new System.Windows.Forms.TextBox();
            this.tbTemplateSize = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.cbTemplateName = new System.Windows.Forms.ComboBox();
            this.namaJari = new System.Windows.Forms.TextBox();
            this.template1Tb = new System.Windows.Forms.TextBox();
            this.template2Tb = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new System.Drawing.Point(12, 27);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(229, 238);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.enrollToolStripMenuItem,
            this.verifyToolStripMenuItem,
            this.templateToolStripMenuItem,
            this.tesToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(513, 24);
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // enrollToolStripMenuItem
            // 
            this.enrollToolStripMenuItem.Name = "enrollToolStripMenuItem";
            this.enrollToolStripMenuItem.Size = new System.Drawing.Size(49, 20);
            this.enrollToolStripMenuItem.Text = "&Enroll";
            this.enrollToolStripMenuItem.Click += new System.EventHandler(this.enrollToolStripMenuItem_Click);
            // 
            // verifyToolStripMenuItem
            // 
            this.verifyToolStripMenuItem.Name = "verifyToolStripMenuItem";
            this.verifyToolStripMenuItem.Size = new System.Drawing.Size(49, 20);
            this.verifyToolStripMenuItem.Text = "&Verify";
            this.verifyToolStripMenuItem.Click += new System.EventHandler(this.verifyToolStripMenuItem_Click);
            // 
            // templateToolStripMenuItem
            // 
            this.templateToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.compactToolStripMenuItem,
            this.isoToolStripMenuItem,
            this.bPPTToolStripMenuItem});
            this.templateToolStripMenuItem.Name = "templateToolStripMenuItem";
            this.templateToolStripMenuItem.Size = new System.Drawing.Size(98, 20);
            this.templateToolStripMenuItem.Text = "&Template Type";
            this.templateToolStripMenuItem.Visible = false;
            // 
            // compactToolStripMenuItem
            // 
            this.compactToolStripMenuItem.Name = "compactToolStripMenuItem";
            this.compactToolStripMenuItem.Size = new System.Drawing.Size(126, 22);
            this.compactToolStripMenuItem.Text = " Compact";
            this.compactToolStripMenuItem.Click += new System.EventHandler(this.compactToolStripMenuItem_Click);
            // 
            // isoToolStripMenuItem
            // 
            this.isoToolStripMenuItem.Checked = true;
            this.isoToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.isoToolStripMenuItem.Name = "isoToolStripMenuItem";
            this.isoToolStripMenuItem.Size = new System.Drawing.Size(126, 22);
            this.isoToolStripMenuItem.Text = "Iso";
            this.isoToolStripMenuItem.Click += new System.EventHandler(this.isoToolStripMenuItem_Click);
            // 
            // bPPTToolStripMenuItem
            // 
            this.bPPTToolStripMenuItem.Name = "bPPTToolStripMenuItem";
            this.bPPTToolStripMenuItem.Size = new System.Drawing.Size(126, 22);
            this.bPPTToolStripMenuItem.Text = "BPPT";
            this.bPPTToolStripMenuItem.Click += new System.EventHandler(this.bPPTToolStripMenuItem_Click);
            // 
            // tesToolStripMenuItem
            // 
            this.tesToolStripMenuItem.Name = "tesToolStripMenuItem";
            this.tesToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.tesToolStripMenuItem.Text = "Tes";
            this.tesToolStripMenuItem.Visible = false;
            this.tesToolStripMenuItem.Click += new System.EventHandler(this.tesToolStripMenuItem_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(278, 53);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(71, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Image Quality";
            // 
            // tbImageQuality
            // 
            this.tbImageQuality.Location = new System.Drawing.Point(360, 50);
            this.tbImageQuality.Name = "tbImageQuality";
            this.tbImageQuality.ReadOnly = true;
            this.tbImageQuality.Size = new System.Drawing.Size(110, 20);
            this.tbImageQuality.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(278, 105);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(35, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Score";
            // 
            // tbScore
            // 
            this.tbScore.Location = new System.Drawing.Point(360, 102);
            this.tbScore.Name = "tbScore";
            this.tbScore.ReadOnly = true;
            this.tbScore.Size = new System.Drawing.Size(110, 20);
            this.tbScore.TabIndex = 5;
            // 
            // tbTemplateSize
            // 
            this.tbTemplateSize.Location = new System.Drawing.Point(360, 76);
            this.tbTemplateSize.Name = "tbTemplateSize";
            this.tbTemplateSize.ReadOnly = true;
            this.tbTemplateSize.Size = new System.Drawing.Size(110, 20);
            this.tbTemplateSize.TabIndex = 8;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(278, 79);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(74, 13);
            this.label3.TabIndex = 7;
            this.label3.Text = "Template Size";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(278, 27);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(82, 13);
            this.label4.TabIndex = 9;
            this.label4.Text = "Template Name";
            // 
            // cbTemplateName
            // 
            this.cbTemplateName.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbTemplateName.FormattingEnabled = true;
            this.cbTemplateName.Items.AddRange(new object[] {
            "template1",
            "template2"});
            this.cbTemplateName.Location = new System.Drawing.Point(360, 24);
            this.cbTemplateName.MaxDropDownItems = 2;
            this.cbTemplateName.Name = "cbTemplateName";
            this.cbTemplateName.Size = new System.Drawing.Size(110, 21);
            this.cbTemplateName.TabIndex = 10;
            this.cbTemplateName.SelectedIndexChanged += new System.EventHandler(this.cbTemplateName_SelectedIndexChanged);
            // 
            // namaJari
            // 
            this.namaJari.Location = new System.Drawing.Point(360, 129);
            this.namaJari.Name = "namaJari";
            this.namaJari.Size = new System.Drawing.Size(110, 20);
            this.namaJari.TabIndex = 11;
            this.namaJari.Visible = false;
            // 
            // template1Tb
            // 
            this.template1Tb.Location = new System.Drawing.Point(264, 177);
            this.template1Tb.Name = "template1Tb";
            this.template1Tb.Size = new System.Drawing.Size(110, 20);
            this.template1Tb.TabIndex = 12;
            this.template1Tb.Text = "template1";
            this.template1Tb.Visible = false;
            // 
            // template2Tb
            // 
            this.template2Tb.Location = new System.Drawing.Point(391, 177);
            this.template2Tb.Name = "template2Tb";
            this.template2Tb.Size = new System.Drawing.Size(110, 20);
            this.template2Tb.TabIndex = 13;
            this.template2Tb.Text = "template2";
            this.template2Tb.Visible = false;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(513, 301);
            this.Controls.Add(this.template2Tb);
            this.Controls.Add(this.template1Tb);
            this.Controls.Add(this.namaJari);
            this.Controls.Add(this.cbTemplateName);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.tbTemplateSize);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.tbScore);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.tbImageQuality);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "AFIS";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem enrollToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem verifyToolStripMenuItem;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tbImageQuality;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tbScore;
        private System.Windows.Forms.TextBox tbTemplateSize;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox cbTemplateName;
        private System.Windows.Forms.ToolStripMenuItem templateToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem compactToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem isoToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem bPPTToolStripMenuItem;
        private System.Windows.Forms.TextBox namaJari;
        private System.Windows.Forms.ToolStripMenuItem tesToolStripMenuItem;
        private System.Windows.Forms.TextBox template1Tb;
        private System.Windows.Forms.TextBox template2Tb;
    }
}

