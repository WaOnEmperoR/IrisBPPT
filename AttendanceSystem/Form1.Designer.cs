namespace AttendanceSystem
{
    partial class AttendanceSystem
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
            this.labelEmployeeNumber = new System.Windows.Forms.Label();
            this.textBoxEmployeeNumber = new System.Windows.Forms.TextBox();
            this.labelFormTitle = new System.Windows.Forms.Label();
            this.buttonChooseFile = new System.Windows.Forms.Button();
            this.buttonScanIris = new System.Windows.Forms.Button();
            this.pictureBoxLeftImage = new System.Windows.Forms.PictureBox();
            this.pictureBoxRightImage = new System.Windows.Forms.PictureBox();
            this.labelResultTitle = new System.Windows.Forms.Label();
            this.labelResult = new System.Windows.Forms.Label();
            this.buttonProcess = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxLeftImage)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxRightImage)).BeginInit();
            this.SuspendLayout();
            // 
            // labelEmployeeNumber
            // 
            this.labelEmployeeNumber.AutoSize = true;
            this.labelEmployeeNumber.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelEmployeeNumber.Location = new System.Drawing.Point(46, 158);
            this.labelEmployeeNumber.Name = "labelEmployeeNumber";
            this.labelEmployeeNumber.Size = new System.Drawing.Size(172, 22);
            this.labelEmployeeNumber.TabIndex = 0;
            this.labelEmployeeNumber.Text = "Employee Number";
            // 
            // textBoxEmployeeNumber
            // 
            this.textBoxEmployeeNumber.Location = new System.Drawing.Point(258, 155);
            this.textBoxEmployeeNumber.Name = "textBoxEmployeeNumber";
            this.textBoxEmployeeNumber.Size = new System.Drawing.Size(66, 26);
            this.textBoxEmployeeNumber.TabIndex = 1;
            // 
            // labelFormTitle
            // 
            this.labelFormTitle.AutoSize = true;
            this.labelFormTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelFormTitle.Location = new System.Drawing.Point(42, 50);
            this.labelFormTitle.Name = "labelFormTitle";
            this.labelFormTitle.Size = new System.Drawing.Size(504, 46);
            this.labelFormTitle.TabIndex = 2;
            this.labelFormTitle.Text = "BPPT Attendance System";
            // 
            // buttonChooseFile
            // 
            this.buttonChooseFile.Location = new System.Drawing.Point(50, 238);
            this.buttonChooseFile.Name = "buttonChooseFile";
            this.buttonChooseFile.Size = new System.Drawing.Size(185, 47);
            this.buttonChooseFile.TabIndex = 3;
            this.buttonChooseFile.Text = "Choose Iris Image";
            this.buttonChooseFile.UseVisualStyleBackColor = true;
            this.buttonChooseFile.Click += new System.EventHandler(this.buttonChooseFile_Click);
            // 
            // buttonScanIris
            // 
            this.buttonScanIris.Location = new System.Drawing.Point(241, 238);
            this.buttonScanIris.Name = "buttonScanIris";
            this.buttonScanIris.Size = new System.Drawing.Size(185, 47);
            this.buttonScanIris.TabIndex = 4;
            this.buttonScanIris.Text = "Scan Iris";
            this.buttonScanIris.UseVisualStyleBackColor = true;
            this.buttonScanIris.Click += new System.EventHandler(this.buttonScanIris_Click);
            // 
            // pictureBoxLeftImage
            // 
            this.pictureBoxLeftImage.Location = new System.Drawing.Point(50, 334);
            this.pictureBoxLeftImage.Name = "pictureBoxLeftImage";
            this.pictureBoxLeftImage.Size = new System.Drawing.Size(480, 369);
            this.pictureBoxLeftImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBoxLeftImage.TabIndex = 5;
            this.pictureBoxLeftImage.TabStop = false;
            // 
            // pictureBoxRightImage
            // 
            this.pictureBoxRightImage.Location = new System.Drawing.Point(565, 334);
            this.pictureBoxRightImage.Name = "pictureBoxRightImage";
            this.pictureBoxRightImage.Size = new System.Drawing.Size(480, 369);
            this.pictureBoxRightImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBoxRightImage.TabIndex = 6;
            this.pictureBoxRightImage.TabStop = false;
            // 
            // labelResultTitle
            // 
            this.labelResultTitle.AutoSize = true;
            this.labelResultTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelResultTitle.Location = new System.Drawing.Point(50, 758);
            this.labelResultTitle.Name = "labelResultTitle";
            this.labelResultTitle.Size = new System.Drawing.Size(73, 22);
            this.labelResultTitle.TabIndex = 7;
            this.labelResultTitle.Text = "Result:";
            // 
            // labelResult
            // 
            this.labelResult.AutoSize = true;
            this.labelResult.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelResult.ForeColor = System.Drawing.SystemColors.ControlText;
            this.labelResult.Location = new System.Drawing.Point(172, 758);
            this.labelResult.Name = "labelResult";
            this.labelResult.Size = new System.Drawing.Size(0, 22);
            this.labelResult.TabIndex = 8;
            // 
            // buttonProcess
            // 
            this.buttonProcess.Location = new System.Drawing.Point(433, 238);
            this.buttonProcess.Name = "buttonProcess";
            this.buttonProcess.Size = new System.Drawing.Size(181, 47);
            this.buttonProcess.TabIndex = 9;
            this.buttonProcess.Text = "Run Process";
            this.buttonProcess.UseVisualStyleBackColor = true;
            this.buttonProcess.Click += new System.EventHandler(this.buttonProcess_Click);
            // 
            // AttendanceSystem
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1396, 865);
            this.Controls.Add(this.buttonProcess);
            this.Controls.Add(this.labelResult);
            this.Controls.Add(this.labelResultTitle);
            this.Controls.Add(this.pictureBoxRightImage);
            this.Controls.Add(this.pictureBoxLeftImage);
            this.Controls.Add(this.buttonScanIris);
            this.Controls.Add(this.buttonChooseFile);
            this.Controls.Add(this.labelFormTitle);
            this.Controls.Add(this.textBoxEmployeeNumber);
            this.Controls.Add(this.labelEmployeeNumber);
            this.Name = "AttendanceSystem";
            this.Text = "Attendance System";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxLeftImage)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxRightImage)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelEmployeeNumber;
        private System.Windows.Forms.TextBox textBoxEmployeeNumber;
        private System.Windows.Forms.Label labelFormTitle;
        private System.Windows.Forms.Button buttonChooseFile;
        private System.Windows.Forms.Button buttonScanIris;
        private System.Windows.Forms.PictureBox pictureBoxLeftImage;
        private System.Windows.Forms.PictureBox pictureBoxRightImage;
        private System.Windows.Forms.Label labelResultTitle;
        private System.Windows.Forms.Label labelResult;
        private System.Windows.Forms.Button buttonProcess;
    }
}

