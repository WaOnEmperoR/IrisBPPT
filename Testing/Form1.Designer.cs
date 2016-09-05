namespace Testing
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
            this.labelEmployeeNumber = new System.Windows.Forms.Label();
            this.textBoxEmployeeNumber = new System.Windows.Forms.TextBox();
            this.labelFormTitle = new System.Windows.Forms.Label();
            this.buttonChooseFile = new System.Windows.Forms.Button();
            this.buttonScanIris = new System.Windows.Forms.Button();
            this.pictureBoxLeftImage = new System.Windows.Forms.PictureBox();
            this.pictureBoxRightImage = new System.Windows.Forms.PictureBox();
            this.StatusTitle = new System.Windows.Forms.Label();
            this.labelResult = new System.Windows.Forms.Label();
            this.buttonProcess = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.buttonEnrollFingerprint = new System.Windows.Forms.Button();
            this.buttonVerifyFingerprint = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.label3 = new System.Windows.Forms.Label();
            this.labelLeftIrisScoreTitle = new System.Windows.Forms.Label();
            this.labelRightIrisScoreTitle = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.labelLeftIrisScore = new System.Windows.Forms.Label();
            this.labelRightIrisScore = new System.Windows.Forms.Label();
            this.labelFingerprintScoreTitle = new System.Windows.Forms.Label();
            this.labelFingerprintScore = new System.Windows.Forms.Label();
            this.labelIrisRecognitionResultTitle = new System.Windows.Forms.Label();
            this.labelFingerprintRecognitionResultTitle = new System.Windows.Forms.Label();
            this.labelFinalDecisionTitle = new System.Windows.Forms.Label();
            this.labelIrisResult = new System.Windows.Forms.Label();
            this.labelFingerprintResult = new System.Windows.Forms.Label();
            this.labelFinalDecision = new System.Windows.Forms.Label();
            this.buttonShowFinalDecision = new System.Windows.Forms.Button();
            this.vScrollBar1 = new System.Windows.Forms.VScrollBar();
            this.buttonSaveFingerprint = new System.Windows.Forms.Button();
            this.buttonChooseFinger = new System.Windows.Forms.Button();
            this.btn_Test = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxLeftImage)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxRightImage)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // labelEmployeeNumber
            // 
            this.labelEmployeeNumber.AutoSize = true;
            this.labelEmployeeNumber.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelEmployeeNumber.Location = new System.Drawing.Point(31, 103);
            this.labelEmployeeNumber.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelEmployeeNumber.Name = "labelEmployeeNumber";
            this.labelEmployeeNumber.Size = new System.Drawing.Size(125, 15);
            this.labelEmployeeNumber.TabIndex = 0;
            this.labelEmployeeNumber.Text = "Employee Number";
            // 
            // textBoxEmployeeNumber
            // 
            this.textBoxEmployeeNumber.Location = new System.Drawing.Point(172, 101);
            this.textBoxEmployeeNumber.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.textBoxEmployeeNumber.Name = "textBoxEmployeeNumber";
            this.textBoxEmployeeNumber.Size = new System.Drawing.Size(45, 20);
            this.textBoxEmployeeNumber.TabIndex = 1;
            // 
            // labelFormTitle
            // 
            this.labelFormTitle.AutoSize = true;
            this.labelFormTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelFormTitle.Location = new System.Drawing.Point(28, 32);
            this.labelFormTitle.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelFormTitle.Name = "labelFormTitle";
            this.labelFormTitle.Size = new System.Drawing.Size(494, 31);
            this.labelFormTitle.TabIndex = 2;
            this.labelFormTitle.Text = "Biometrics Multimodal Authentication";
            // 
            // buttonChooseFile
            // 
            this.buttonChooseFile.Location = new System.Drawing.Point(33, 155);
            this.buttonChooseFile.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.buttonChooseFile.Name = "buttonChooseFile";
            this.buttonChooseFile.Size = new System.Drawing.Size(123, 31);
            this.buttonChooseFile.TabIndex = 3;
            this.buttonChooseFile.Text = "Choose Iris Image";
            this.buttonChooseFile.UseVisualStyleBackColor = true;
            this.buttonChooseFile.Click += new System.EventHandler(this.buttonChooseFile_Click);
            // 
            // buttonScanIris
            // 
            this.buttonScanIris.Location = new System.Drawing.Point(161, 155);
            this.buttonScanIris.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.buttonScanIris.Name = "buttonScanIris";
            this.buttonScanIris.Size = new System.Drawing.Size(123, 31);
            this.buttonScanIris.TabIndex = 4;
            this.buttonScanIris.Text = "Scan Iris";
            this.buttonScanIris.UseVisualStyleBackColor = true;
            this.buttonScanIris.Click += new System.EventHandler(this.buttonScanIris_Click);
            // 
            // pictureBoxLeftImage
            // 
            this.pictureBoxLeftImage.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBoxLeftImage.Location = new System.Drawing.Point(33, 217);
            this.pictureBoxLeftImage.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.pictureBoxLeftImage.Name = "pictureBoxLeftImage";
            this.pictureBoxLeftImage.Size = new System.Drawing.Size(321, 241);
            this.pictureBoxLeftImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBoxLeftImage.TabIndex = 5;
            this.pictureBoxLeftImage.TabStop = false;
            // 
            // pictureBoxRightImage
            // 
            this.pictureBoxRightImage.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBoxRightImage.Location = new System.Drawing.Point(377, 217);
            this.pictureBoxRightImage.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.pictureBoxRightImage.Name = "pictureBoxRightImage";
            this.pictureBoxRightImage.Size = new System.Drawing.Size(321, 241);
            this.pictureBoxRightImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBoxRightImage.TabIndex = 6;
            this.pictureBoxRightImage.TabStop = false;
            // 
            // StatusTitle
            // 
            this.StatusTitle.AutoSize = true;
            this.StatusTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.StatusTitle.Location = new System.Drawing.Point(33, 493);
            this.StatusTitle.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.StatusTitle.Name = "StatusTitle";
            this.StatusTitle.Size = new System.Drawing.Size(51, 15);
            this.StatusTitle.TabIndex = 7;
            this.StatusTitle.Text = "Status:";
            // 
            // labelResult
            // 
            this.labelResult.AutoSize = true;
            this.labelResult.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelResult.ForeColor = System.Drawing.SystemColors.ControlText;
            this.labelResult.Location = new System.Drawing.Point(115, 493);
            this.labelResult.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelResult.Name = "labelResult";
            this.labelResult.Size = new System.Drawing.Size(0, 15);
            this.labelResult.TabIndex = 8;
            // 
            // buttonProcess
            // 
            this.buttonProcess.Location = new System.Drawing.Point(288, 155);
            this.buttonProcess.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.buttonProcess.Name = "buttonProcess";
            this.buttonProcess.Size = new System.Drawing.Size(121, 31);
            this.buttonProcess.TabIndex = 9;
            this.buttonProcess.Text = "Verify Iris";
            this.buttonProcess.UseVisualStyleBackColor = true;
            this.buttonProcess.Click += new System.EventHandler(this.buttonProcess_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(31, 200);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(46, 13);
            this.label1.TabIndex = 10;
            this.label1.Text = "Left Eye";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(374, 200);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 13);
            this.label2.TabIndex = 11;
            this.label2.Text = "Right Eye";
            // 
            // buttonEnrollFingerprint
            // 
            this.buttonEnrollFingerprint.Location = new System.Drawing.Point(413, 155);
            this.buttonEnrollFingerprint.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.buttonEnrollFingerprint.Name = "buttonEnrollFingerprint";
            this.buttonEnrollFingerprint.Size = new System.Drawing.Size(121, 31);
            this.buttonEnrollFingerprint.TabIndex = 12;
            this.buttonEnrollFingerprint.Text = "Enroll Fingerprint";
            this.buttonEnrollFingerprint.UseVisualStyleBackColor = true;
            this.buttonEnrollFingerprint.Click += new System.EventHandler(this.buttonEnrollFingerprint_Click);
            // 
            // buttonVerifyFingerprint
            // 
            this.buttonVerifyFingerprint.Location = new System.Drawing.Point(537, 155);
            this.buttonVerifyFingerprint.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.buttonVerifyFingerprint.Name = "buttonVerifyFingerprint";
            this.buttonVerifyFingerprint.Size = new System.Drawing.Size(121, 31);
            this.buttonVerifyFingerprint.TabIndex = 13;
            this.buttonVerifyFingerprint.Text = "Verify Fingerprint";
            this.buttonVerifyFingerprint.UseVisualStyleBackColor = true;
            this.buttonVerifyFingerprint.Click += new System.EventHandler(this.buttonVerifyFingerprint_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBox1.Location = new System.Drawing.Point(719, 216);
            this.pictureBox1.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(321, 241);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 14;
            this.pictureBox1.TabStop = false;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(717, 200);
            this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(56, 13);
            this.label3.TabIndex = 15;
            this.label3.Text = "Fingerprint";
            // 
            // labelLeftIrisScoreTitle
            // 
            this.labelLeftIrisScoreTitle.AutoSize = true;
            this.labelLeftIrisScoreTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelLeftIrisScoreTitle.Location = new System.Drawing.Point(33, 522);
            this.labelLeftIrisScoreTitle.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelLeftIrisScoreTitle.Name = "labelLeftIrisScoreTitle";
            this.labelLeftIrisScoreTitle.Size = new System.Drawing.Size(163, 15);
            this.labelLeftIrisScoreTitle.TabIndex = 16;
            this.labelLeftIrisScoreTitle.Text = "Left Iris Matching Score:";
            // 
            // labelRightIrisScoreTitle
            // 
            this.labelRightIrisScoreTitle.AutoSize = true;
            this.labelRightIrisScoreTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelRightIrisScoreTitle.Location = new System.Drawing.Point(33, 552);
            this.labelRightIrisScoreTitle.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelRightIrisScoreTitle.Name = "labelRightIrisScoreTitle";
            this.labelRightIrisScoreTitle.Size = new System.Drawing.Size(173, 15);
            this.labelRightIrisScoreTitle.TabIndex = 17;
            this.labelRightIrisScoreTitle.Text = "Right Iris Matching Score:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(169, 524);
            this.label6.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(0, 15);
            this.label6.TabIndex = 18;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(221, 552);
            this.label7.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(0, 15);
            this.label7.TabIndex = 19;
            // 
            // labelLeftIrisScore
            // 
            this.labelLeftIrisScore.AutoSize = true;
            this.labelLeftIrisScore.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelLeftIrisScore.Location = new System.Drawing.Point(221, 524);
            this.labelLeftIrisScore.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelLeftIrisScore.Name = "labelLeftIrisScore";
            this.labelLeftIrisScore.Size = new System.Drawing.Size(0, 15);
            this.labelLeftIrisScore.TabIndex = 20;
            // 
            // labelRightIrisScore
            // 
            this.labelRightIrisScore.AutoSize = true;
            this.labelRightIrisScore.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelRightIrisScore.Location = new System.Drawing.Point(210, 552);
            this.labelRightIrisScore.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelRightIrisScore.Name = "labelRightIrisScore";
            this.labelRightIrisScore.Size = new System.Drawing.Size(0, 15);
            this.labelRightIrisScore.TabIndex = 21;
            // 
            // labelFingerprintScoreTitle
            // 
            this.labelFingerprintScoreTitle.AutoSize = true;
            this.labelFingerprintScoreTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelFingerprintScoreTitle.Location = new System.Drawing.Point(33, 581);
            this.labelFingerprintScoreTitle.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelFingerprintScoreTitle.Name = "labelFingerprintScoreTitle";
            this.labelFingerprintScoreTitle.Size = new System.Drawing.Size(185, 15);
            this.labelFingerprintScoreTitle.TabIndex = 22;
            this.labelFingerprintScoreTitle.Text = "Fingerprint Matching Score:";
            // 
            // labelFingerprintScore
            // 
            this.labelFingerprintScore.AutoSize = true;
            this.labelFingerprintScore.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelFingerprintScore.Location = new System.Drawing.Point(229, 581);
            this.labelFingerprintScore.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelFingerprintScore.Name = "labelFingerprintScore";
            this.labelFingerprintScore.Size = new System.Drawing.Size(0, 15);
            this.labelFingerprintScore.TabIndex = 23;
            // 
            // labelIrisRecognitionResultTitle
            // 
            this.labelIrisRecognitionResultTitle.AutoSize = true;
            this.labelIrisRecognitionResultTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelIrisRecognitionResultTitle.Location = new System.Drawing.Point(33, 610);
            this.labelIrisRecognitionResultTitle.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelIrisRecognitionResultTitle.Name = "labelIrisRecognitionResultTitle";
            this.labelIrisRecognitionResultTitle.Size = new System.Drawing.Size(157, 15);
            this.labelIrisRecognitionResultTitle.TabIndex = 24;
            this.labelIrisRecognitionResultTitle.Text = "Iris Recognition Result:";
            // 
            // labelFingerprintRecognitionResultTitle
            // 
            this.labelFingerprintRecognitionResultTitle.AutoSize = true;
            this.labelFingerprintRecognitionResultTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelFingerprintRecognitionResultTitle.Location = new System.Drawing.Point(33, 636);
            this.labelFingerprintRecognitionResultTitle.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelFingerprintRecognitionResultTitle.Name = "labelFingerprintRecognitionResultTitle";
            this.labelFingerprintRecognitionResultTitle.Size = new System.Drawing.Size(207, 15);
            this.labelFingerprintRecognitionResultTitle.TabIndex = 25;
            this.labelFingerprintRecognitionResultTitle.Text = "Fingerprint Recognition Result:";
            // 
            // labelFinalDecisionTitle
            // 
            this.labelFinalDecisionTitle.AutoSize = true;
            this.labelFinalDecisionTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelFinalDecisionTitle.Location = new System.Drawing.Point(33, 664);
            this.labelFinalDecisionTitle.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelFinalDecisionTitle.Name = "labelFinalDecisionTitle";
            this.labelFinalDecisionTitle.Size = new System.Drawing.Size(103, 15);
            this.labelFinalDecisionTitle.TabIndex = 26;
            this.labelFinalDecisionTitle.Text = "Final Decision:";
            // 
            // labelIrisResult
            // 
            this.labelIrisResult.AutoSize = true;
            this.labelIrisResult.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelIrisResult.Location = new System.Drawing.Point(197, 610);
            this.labelIrisResult.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelIrisResult.Name = "labelIrisResult";
            this.labelIrisResult.Size = new System.Drawing.Size(0, 15);
            this.labelIrisResult.TabIndex = 27;
            // 
            // labelFingerprintResult
            // 
            this.labelFingerprintResult.AutoSize = true;
            this.labelFingerprintResult.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelFingerprintResult.Location = new System.Drawing.Point(245, 636);
            this.labelFingerprintResult.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelFingerprintResult.Name = "labelFingerprintResult";
            this.labelFingerprintResult.Size = new System.Drawing.Size(0, 15);
            this.labelFingerprintResult.TabIndex = 28;
            // 
            // labelFinalDecision
            // 
            this.labelFinalDecision.AutoSize = true;
            this.labelFinalDecision.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelFinalDecision.Location = new System.Drawing.Point(152, 664);
            this.labelFinalDecision.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelFinalDecision.Name = "labelFinalDecision";
            this.labelFinalDecision.Size = new System.Drawing.Size(0, 15);
            this.labelFinalDecision.TabIndex = 29;
            // 
            // buttonShowFinalDecision
            // 
            this.buttonShowFinalDecision.Location = new System.Drawing.Point(911, 155);
            this.buttonShowFinalDecision.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.buttonShowFinalDecision.Name = "buttonShowFinalDecision";
            this.buttonShowFinalDecision.Size = new System.Drawing.Size(121, 31);
            this.buttonShowFinalDecision.TabIndex = 30;
            this.buttonShowFinalDecision.Text = "Show Final Decision";
            this.buttonShowFinalDecision.UseVisualStyleBackColor = true;
            this.buttonShowFinalDecision.Click += new System.EventHandler(this.buttonShowFinalDecision_Click);
            // 
            // vScrollBar1
            // 
            this.vScrollBar1.Location = new System.Drawing.Point(1145, 0);
            this.vScrollBar1.Name = "vScrollBar1";
            this.vScrollBar1.Size = new System.Drawing.Size(26, 699);
            this.vScrollBar1.TabIndex = 31;
            // 
            // buttonSaveFingerprint
            // 
            this.buttonSaveFingerprint.Location = new System.Drawing.Point(662, 155);
            this.buttonSaveFingerprint.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.buttonSaveFingerprint.Name = "buttonSaveFingerprint";
            this.buttonSaveFingerprint.Size = new System.Drawing.Size(121, 31);
            this.buttonSaveFingerprint.TabIndex = 32;
            this.buttonSaveFingerprint.Text = "Save Fingerprint";
            this.buttonSaveFingerprint.UseVisualStyleBackColor = true;
            this.buttonSaveFingerprint.Click += new System.EventHandler(this.button1_Click);
            // 
            // buttonChooseFinger
            // 
            this.buttonChooseFinger.Location = new System.Drawing.Point(787, 155);
            this.buttonChooseFinger.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.buttonChooseFinger.Name = "buttonChooseFinger";
            this.buttonChooseFinger.Size = new System.Drawing.Size(121, 31);
            this.buttonChooseFinger.TabIndex = 33;
            this.buttonChooseFinger.Text = "Choose Fingerprint";
            this.buttonChooseFinger.UseVisualStyleBackColor = true;
            this.buttonChooseFinger.Click += new System.EventHandler(this.buttonChooseFinger_Click);
            // 
            // btn_Test
            // 
            this.btn_Test.Location = new System.Drawing.Point(288, 110);
            this.btn_Test.Name = "btn_Test";
            this.btn_Test.Size = new System.Drawing.Size(120, 27);
            this.btn_Test.TabIndex = 34;
            this.btn_Test.Text = "Tes Extract";
            this.btn_Test.UseVisualStyleBackColor = true;
            this.btn_Test.Click += new System.EventHandler(this.btn_Test_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(908, 482);
            this.Controls.Add(this.btn_Test);
            this.Controls.Add(this.buttonChooseFinger);
            this.Controls.Add(this.buttonSaveFingerprint);
            this.Controls.Add(this.vScrollBar1);
            this.Controls.Add(this.buttonShowFinalDecision);
            this.Controls.Add(this.labelFinalDecision);
            this.Controls.Add(this.labelFingerprintResult);
            this.Controls.Add(this.labelIrisResult);
            this.Controls.Add(this.labelFinalDecisionTitle);
            this.Controls.Add(this.labelFingerprintRecognitionResultTitle);
            this.Controls.Add(this.labelIrisRecognitionResultTitle);
            this.Controls.Add(this.labelFingerprintScore);
            this.Controls.Add(this.labelFingerprintScoreTitle);
            this.Controls.Add(this.labelRightIrisScore);
            this.Controls.Add(this.labelLeftIrisScore);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.labelRightIrisScoreTitle);
            this.Controls.Add(this.labelLeftIrisScoreTitle);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.buttonVerifyFingerprint);
            this.Controls.Add(this.buttonEnrollFingerprint);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.buttonProcess);
            this.Controls.Add(this.labelResult);
            this.Controls.Add(this.StatusTitle);
            this.Controls.Add(this.pictureBoxRightImage);
            this.Controls.Add(this.pictureBoxLeftImage);
            this.Controls.Add(this.buttonScanIris);
            this.Controls.Add(this.buttonChooseFile);
            this.Controls.Add(this.labelFormTitle);
            this.Controls.Add(this.textBoxEmployeeNumber);
            this.Controls.Add(this.labelEmployeeNumber);
            this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.Name = "Form1";
            this.Text = "Attendance System";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxLeftImage)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxRightImage)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
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
        private System.Windows.Forms.Label StatusTitle;
        private System.Windows.Forms.Label labelResult;
        private System.Windows.Forms.Button buttonProcess;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button buttonEnrollFingerprint;
        private System.Windows.Forms.Button buttonVerifyFingerprint;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label labelLeftIrisScoreTitle;
        private System.Windows.Forms.Label labelRightIrisScoreTitle;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label labelLeftIrisScore;
        private System.Windows.Forms.Label labelRightIrisScore;
        private System.Windows.Forms.Label labelFingerprintScoreTitle;
        private System.Windows.Forms.Label labelFingerprintScore;
        private System.Windows.Forms.Label labelIrisRecognitionResultTitle;
        private System.Windows.Forms.Label labelFingerprintRecognitionResultTitle;
        private System.Windows.Forms.Label labelFinalDecisionTitle;
        private System.Windows.Forms.Label labelIrisResult;
        private System.Windows.Forms.Label labelFingerprintResult;
        private System.Windows.Forms.Label labelFinalDecision;
        private System.Windows.Forms.Button buttonShowFinalDecision;
        private System.Windows.Forms.VScrollBar vScrollBar1;
        private System.Windows.Forms.Button buttonSaveFingerprint;
        private System.Windows.Forms.Button buttonChooseFinger;
        private System.Windows.Forms.Button btn_Test;
    }
}

