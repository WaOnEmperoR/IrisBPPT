using BiometriBPPT.bppt.ptik.biometric.entity;
using BiometriBPPT.bppt.ptik.biometric.image;
using BiometriBPPT.bppt.ptik.biometric.iris;
using BiometriBPPT.bppt.ptik.biometric.utility;
using BPPT_Iris_SDK;
using BPPT_Iris_SDK.id.go.bppt.biometri.iris.image;
using BPPT_Iris_SDK.id.go.bppt.biometri.iris.processing;
using libMobileEyes.Net;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;
namespace Testing
{
    public partial class frmCapture : Form
    {
        private delegate void CaptureCompleteDelegate(RME_Error result);
        private delegate RME_EyeType WhichEyeCallback();
        private IContainer components;
        private PictureBox pbLeft;
        private PictureBox pbRight;
        private Label label1;
        private Button bnStart;
        private Button bnStop;
        private TextBox tbLFrameCount;
        private TextBox tbRightFrameCount;
        private TextBox tbRightQual;
        private TextBox tbLeftQual;
        private Label label2;
        private Label label3;
        private Label label4;
        private Label label5;
        private Label lbLeftQuality;
        private Label lbRightQuality;
        private Button bnLeftShow;
        private Button bnRightShow;
        private Label lbError;
        private ComboBox eyeSelectBox;
        private Button btnSaveREye;
        private Button btnSaveLEye;
        private CheckBox m_beepCheck;
        private Label m_progLabel;
        private Label label6;
        private Label label7;
        private NumericUpDown m_timeoutBox;
        private Label label8;
        private Label tbSN;
        private Label m_modelText;
        private libMobileEyes.Net.libMobileEyes camera;
        private IrisImageQualityInfo LeftEyeQuality;
        private IrisImageQualityInfo RightEyeQuality;
        private bool showLeftOverlay;
        private bool showRightOverlay;
        private int lFrameCount;
        private int rFrameCount;
        private Bitmap rEye;
        private Bitmap lEye;
        private Bitmap rEyeOverlay;
        private Bitmap lEyeOverlay;
        private RME_Error cameraError;
        private SaveFileDialog m_sfd = new SaveFileDialog();
        private Thread m_captureThread;        
        
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }
        private void InitializeComponent()
        {
            this.pbLeft = new System.Windows.Forms.PictureBox();
            this.pbRight = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            this.bnStart = new System.Windows.Forms.Button();
            this.bnStop = new System.Windows.Forms.Button();
            this.tbLFrameCount = new System.Windows.Forms.TextBox();
            this.tbRightFrameCount = new System.Windows.Forms.TextBox();
            this.tbRightQual = new System.Windows.Forms.TextBox();
            this.tbLeftQual = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.lbLeftQuality = new System.Windows.Forms.Label();
            this.lbRightQuality = new System.Windows.Forms.Label();
            this.bnLeftShow = new System.Windows.Forms.Button();
            this.bnRightShow = new System.Windows.Forms.Button();
            this.lbError = new System.Windows.Forms.Label();
            this.eyeSelectBox = new System.Windows.Forms.ComboBox();
            this.btnSaveREye = new System.Windows.Forms.Button();
            this.btnSaveLEye = new System.Windows.Forms.Button();
            this.m_beepCheck = new System.Windows.Forms.CheckBox();
            this.m_progLabel = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.m_timeoutBox = new System.Windows.Forms.NumericUpDown();
            this.label8 = new System.Windows.Forms.Label();
            this.tbSN = new System.Windows.Forms.Label();
            this.m_modelText = new System.Windows.Forms.Label();
            this.btnUseImage = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pbLeft)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbRight)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.m_timeoutBox)).BeginInit();
            this.SuspendLayout();
            // 
            // pbLeft
            // 
            this.pbLeft.BackColor = System.Drawing.Color.White;
            this.pbLeft.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pbLeft.Location = new System.Drawing.Point(354, 197);
            this.pbLeft.Name = "pbLeft";
            this.pbLeft.Size = new System.Drawing.Size(320, 240);
            this.pbLeft.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pbLeft.TabIndex = 0;
            this.pbLeft.TabStop = false;
            // 
            // pbRight
            // 
            this.pbRight.BackColor = System.Drawing.Color.White;
            this.pbRight.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pbRight.Location = new System.Drawing.Point(9, 197);
            this.pbRight.Name = "pbRight";
            this.pbRight.Size = new System.Drawing.Size(320, 240);
            this.pbRight.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pbRight.TabIndex = 1;
            this.pbRight.TabStop = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(113, 20);
            this.label1.TabIndex = 2;
            this.label1.Text = "Serial Number:";
            // 
            // bnStart
            // 
            this.bnStart.Enabled = false;
            this.bnStart.Location = new System.Drawing.Point(9, 444);
            this.bnStart.Name = "bnStart";
            this.bnStart.Size = new System.Drawing.Size(135, 44);
            this.bnStart.TabIndex = 4;
            this.bnStart.Text = "Start Capture";
            this.bnStart.UseVisualStyleBackColor = true;
            this.bnStart.Click += new System.EventHandler(this.bnStart_Click);
            // 
            // bnStop
            // 
            this.bnStop.Enabled = false;
            this.bnStop.Location = new System.Drawing.Point(194, 443);
            this.bnStop.Name = "bnStop";
            this.bnStop.Size = new System.Drawing.Size(135, 44);
            this.bnStop.TabIndex = 5;
            this.bnStop.Text = "Stop Capture";
            this.bnStop.UseVisualStyleBackColor = true;
            this.bnStop.Click += new System.EventHandler(this.bnStop_Click);
            // 
            // tbLFrameCount
            // 
            this.tbLFrameCount.Location = new System.Drawing.Point(364, 174);
            this.tbLFrameCount.Name = "tbLFrameCount";
            this.tbLFrameCount.Size = new System.Drawing.Size(65, 26);
            this.tbLFrameCount.TabIndex = 6;
            // 
            // tbRightFrameCount
            // 
            this.tbRightFrameCount.Location = new System.Drawing.Point(256, 174);
            this.tbRightFrameCount.Name = "tbRightFrameCount";
            this.tbRightFrameCount.Size = new System.Drawing.Size(63, 26);
            this.tbRightFrameCount.TabIndex = 7;
            // 
            // tbRightQual
            // 
            this.tbRightQual.Location = new System.Drawing.Point(10, 174);
            this.tbRightQual.Name = "tbRightQual";
            this.tbRightQual.Size = new System.Drawing.Size(63, 26);
            this.tbRightQual.TabIndex = 10;
            // 
            // tbLeftQual
            // 
            this.tbLeftQual.Location = new System.Drawing.Point(610, 174);
            this.tbLeftQual.Name = "tbLeftQual";
            this.tbLeftQual.Size = new System.Drawing.Size(63, 26);
            this.tbLeftQual.TabIndex = 11;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(476, 133);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(107, 29);
            this.label2.TabIndex = 12;
            this.label2.Text = "Left Eye";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(126, 133);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(125, 29);
            this.label3.TabIndex = 13;
            this.label3.Text = "Right Eye";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(351, 158);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(134, 20);
            this.label4.TabIndex = 14;
            this.label4.Text = "Live Frame Count";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(242, 158);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(134, 20);
            this.label5.TabIndex = 15;
            this.label5.Text = "Live Frame Count";
            // 
            // lbLeftQuality
            // 
            this.lbLeftQuality.AutoSize = true;
            this.lbLeftQuality.Location = new System.Drawing.Point(606, 158);
            this.lbLeftQuality.Name = "lbLeftQuality";
            this.lbLeftQuality.Size = new System.Drawing.Size(103, 20);
            this.lbLeftQuality.TabIndex = 16;
            this.lbLeftQuality.Text = "Quality Score";
            // 
            // lbRightQuality
            // 
            this.lbRightQuality.AutoSize = true;
            this.lbRightQuality.Location = new System.Drawing.Point(6, 158);
            this.lbRightQuality.Name = "lbRightQuality";
            this.lbRightQuality.Size = new System.Drawing.Size(103, 20);
            this.lbRightQuality.TabIndex = 17;
            this.lbRightQuality.Text = "Quality Score";
            // 
            // bnLeftShow
            // 
            this.bnLeftShow.Location = new System.Drawing.Point(447, 158);
            this.bnLeftShow.Name = "bnLeftShow";
            this.bnLeftShow.Size = new System.Drawing.Size(135, 36);
            this.bnLeftShow.TabIndex = 18;
            this.bnLeftShow.Text = "Show Overlay";
            this.bnLeftShow.UseVisualStyleBackColor = true;
            this.bnLeftShow.Click += new System.EventHandler(this.bnLeftShow_Click);
            // 
            // bnRightShow
            // 
            this.bnRightShow.Location = new System.Drawing.Point(102, 158);
            this.bnRightShow.Name = "bnRightShow";
            this.bnRightShow.Size = new System.Drawing.Size(135, 36);
            this.bnRightShow.TabIndex = 19;
            this.bnRightShow.Text = "Show Overlay";
            this.bnRightShow.UseVisualStyleBackColor = true;
            this.bnRightShow.Click += new System.EventHandler(this.bnRightShow_Click);
            // 
            // lbError
            // 
            this.lbError.AutoSize = true;
            this.lbError.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbError.ForeColor = System.Drawing.Color.Red;
            this.lbError.Location = new System.Drawing.Point(228, 9);
            this.lbError.Name = "lbError";
            this.lbError.Size = new System.Drawing.Size(19, 29);
            this.lbError.TabIndex = 20;
            this.lbError.Text = " ";
            this.lbError.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // eyeSelectBox
            // 
            this.eyeSelectBox.FormattingEnabled = true;
            this.eyeSelectBox.Location = new System.Drawing.Point(92, 61);
            this.eyeSelectBox.Name = "eyeSelectBox";
            this.eyeSelectBox.Size = new System.Drawing.Size(113, 28);
            this.eyeSelectBox.TabIndex = 21;
            // 
            // btnSaveREye
            // 
            this.btnSaveREye.Enabled = false;
            this.btnSaveREye.Location = new System.Drawing.Point(452, 444);
            this.btnSaveREye.Name = "btnSaveREye";
            this.btnSaveREye.Size = new System.Drawing.Size(108, 44);
            this.btnSaveREye.TabIndex = 29;
            this.btnSaveREye.Text = "Save - Right Eye";
            this.btnSaveREye.UseVisualStyleBackColor = true;
            this.btnSaveREye.Click += new System.EventHandler(this.btnSaveREye_Click);
            // 
            // btnSaveLEye
            // 
            this.btnSaveLEye.Enabled = false;
            this.btnSaveLEye.Location = new System.Drawing.Point(566, 444);
            this.btnSaveLEye.Name = "btnSaveLEye";
            this.btnSaveLEye.Size = new System.Drawing.Size(108, 44);
            this.btnSaveLEye.TabIndex = 30;
            this.btnSaveLEye.Text = "Save - Left Eye";
            this.btnSaveLEye.UseVisualStyleBackColor = true;
            this.btnSaveLEye.Click += new System.EventHandler(this.btnSaveLEye_Click);
            // 
            // m_beepCheck
            // 
            this.m_beepCheck.AutoSize = true;
            this.m_beepCheck.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.m_beepCheck.Checked = true;
            this.m_beepCheck.CheckState = System.Windows.Forms.CheckState.Checked;
            this.m_beepCheck.Location = new System.Drawing.Point(559, 11);
            this.m_beepCheck.Name = "m_beepCheck";
            this.m_beepCheck.Size = new System.Drawing.Size(174, 24);
            this.m_beepCheck.TabIndex = 31;
            this.m_beepCheck.Text = "Beep when finished";
            this.m_beepCheck.UseVisualStyleBackColor = true;
            // 
            // m_progLabel
            // 
            this.m_progLabel.AutoSize = true;
            this.m_progLabel.Location = new System.Drawing.Point(259, 11);
            this.m_progLabel.Name = "m_progLabel";
            this.m_progLabel.Size = new System.Drawing.Size(0, 20);
            this.m_progLabel.TabIndex = 32;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(11, 37);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(108, 20);
            this.label6.TabIndex = 33;
            this.label6.Text = "Device Model:";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(11, 90);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(105, 20);
            this.label7.TabIndex = 35;
            this.label7.Text = "Timeout (sec)";
            // 
            // m_timeoutBox
            // 
            this.m_timeoutBox.Location = new System.Drawing.Point(92, 88);
            this.m_timeoutBox.Name = "m_timeoutBox";
            this.m_timeoutBox.Size = new System.Drawing.Size(60, 26);
            this.m_timeoutBox.TabIndex = 36;
            this.m_timeoutBox.Value = new decimal(new int[] {
            30,
            0,
            0,
            0});
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(11, 64);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(84, 20);
            this.label8.TabIndex = 37;
            this.label8.Text = "Which Eye";
            // 
            // tbSN
            // 
            this.tbSN.AutoSize = true;
            this.tbSN.Location = new System.Drawing.Point(93, 12);
            this.tbSN.Name = "tbSN";
            this.tbSN.Size = new System.Drawing.Size(97, 20);
            this.tbSN.TabIndex = 38;
            this.tbSN.Text = "NO DEVICE";
            // 
            // m_modelText
            // 
            this.m_modelText.AutoSize = true;
            this.m_modelText.Location = new System.Drawing.Point(93, 37);
            this.m_modelText.Name = "m_modelText";
            this.m_modelText.Size = new System.Drawing.Size(0, 20);
            this.m_modelText.TabIndex = 39;
            // 
            // btnUseImage
            // 
            this.btnUseImage.Location = new System.Drawing.Point(354, 444);
            this.btnUseImage.Name = "btnUseImage";
            this.btnUseImage.Size = new System.Drawing.Size(92, 44);
            this.btnUseImage.TabIndex = 40;
            this.btnUseImage.Text = "Use Image";
            this.btnUseImage.UseVisualStyleBackColor = true;
            this.btnUseImage.Click += new System.EventHandler(this.btnUseImage_Click);
            // 
            // frmCapture
            // 
            this.ClientSize = new System.Drawing.Size(1599, 899);
            this.Controls.Add(this.btnUseImage);
            this.Controls.Add(this.m_modelText);
            this.Controls.Add(this.tbSN);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.m_timeoutBox);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.m_progLabel);
            this.Controls.Add(this.m_beepCheck);
            this.Controls.Add(this.btnSaveLEye);
            this.Controls.Add(this.btnSaveREye);
            this.Controls.Add(this.eyeSelectBox);
            this.Controls.Add(this.lbError);
            this.Controls.Add(this.bnRightShow);
            this.Controls.Add(this.bnLeftShow);
            this.Controls.Add(this.lbRightQuality);
            this.Controls.Add(this.lbLeftQuality);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.tbLeftQual);
            this.Controls.Add(this.tbRightQual);
            this.Controls.Add(this.tbRightFrameCount);
            this.Controls.Add(this.tbLFrameCount);
            this.Controls.Add(this.bnStop);
            this.Controls.Add(this.bnStart);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.pbRight);
            this.Controls.Add(this.pbLeft);
            this.DoubleBuffered = true;
            this.Name = "frmCapture";
            this.Text = "Iris Capture";
            this.Load += new System.EventHandler(this.frmCapture_Load_1);
            ((System.ComponentModel.ISupportInitialize)(this.pbLeft)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbRight)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.m_timeoutBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        public frmCapture()
        {
            this.InitializeComponent();
            this.m_sfd.Filter = "png Files (*.png)|*.png";
            base.Load += new EventHandler(this.frmCapture_Load);
            base.FormClosing += new FormClosingEventHandler(this.frmCapture_FormClosing);
            this.eyeSelectBox.Items.Add("Right Eye");
            this.eyeSelectBox.Items.Add("Left Eye");
            this.eyeSelectBox.Items.Add("Both Eyes");
            this.eyeSelectBox.SelectedIndex = 2;
            this.btnSaveREye.Enabled = false;
            this.btnSaveLEye.Enabled = false;
        }
        private void frmCapture_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.bnStart.Enabled = false;
            this.bnStop.Enabled = false;
            this.camera.RME_Cancel();
            this.camera.RME_Close();
        }
        private void frmCapture_Load(object sender, EventArgs e)
        {
            this.CameraDisconnected();
            this.camera = new libMobileEyes.Net.libMobileEyes();
            this.camera.DeviceChangedHandler = new DeviceChangedCallBack(this.camera_DeviceChangedHandler);
            this.cameraError = this.camera.RME_Initialize();
        }
        private void camera_DeviceChangedHandler(RME_DeviceChangeEvent deviceChanged)
        {
            if (base.InvokeRequired)
            {
                base.BeginInvoke(new DeviceChangedCallBack(this.camera_DeviceChangedHandler), new object[]
				{
					deviceChanged
				});
                return;
            }
            this.m_progLabel.Text = "";
            if (deviceChanged == RME_DeviceChangeEvent.DEVICE_REMOVED)
            {
                this.CameraDisconnected();
                return;
            }
            if (deviceChanged == RME_DeviceChangeEvent.DEVICE_INSERTED)
            {
                this.CameraConnected();
                return;
            }
            if (deviceChanged == RME_DeviceChangeEvent.DEVICE_PROGRAMMING)
            {
                this.m_progLabel.Text = "Updating device firmware...";
            }
        }
        private void bnStart_Click(object sender, EventArgs e)
        {
            this.cameraError = RME_Error.RC_SUCCESS;
            this.bnStart.Enabled = false;
            this.bnStop.Enabled = true;
            this.bnLeftShow.Enabled = false;
            this.bnRightShow.Enabled = false;
            this.btnSaveREye.Enabled = false;
            this.btnSaveLEye.Enabled = false;
            this.tbLeftQual.Text = "";
            this.tbRightQual.Text = "";
            this.lbLeftQuality.Text = "Focus Score";
            this.lbRightQuality.Text = "Focus Score";
            this.pbLeft.Image = null;
            this.pbLeft.Invalidate();
            this.pbRight.Image = null;
            this.pbRight.Invalidate();
            this.m_captureThread = new Thread(new ThreadStart(this.captureImages));
            this.m_captureThread.Name = "captureImages";
            this.m_captureThread.Start();
        }
        private void bnStop_Click(object sender, EventArgs e)
        {
            this.cameraError = RME_Error.RC_SUCCESS;
            this.bnStop.Enabled = false;
            this.btnSaveREye.Enabled = false;
            this.btnSaveLEye.Enabled = false;
            this.camera.RME_Cancel();
        }
        private void CameraConnected()
        {
            MobileEyesDevInfo2 mobileEyesDevInfo = default(MobileEyesDevInfo2);
            if (this.camera.RME_GetDeviceInfo(ref mobileEyesDevInfo) == RME_Error.RC_SUCCESS)
            {
                if (mobileEyesDevInfo.SerialNumberMe == 0u)
                {
                    this.tbSN.Text = mobileEyesDevInfo.SerialNumberMex;
                }
                else
                {
                    this.tbSN.Text = mobileEyesDevInfo.SerialNumberMe.ToString();
                }
                this.m_modelText.Text = mobileEyesDevInfo.Model.ToString();
            }
            else
            {
                this.tbSN.Text = "SN FAIL";
                this.m_modelText.Text = "MODEL FAIL";
            }
            this.bnStart.Enabled = true;
            this.bnStop.Enabled = false;
            this.btnSaveREye.Enabled = false;
            this.btnSaveLEye.Enabled = false;
        }
        private void CameraDisconnected()
        {
            this.bnStart.Enabled = false;
            this.bnStop.Enabled = false;
            this.tbSN.Text = "NO DEVICE";
            this.m_modelText.Text = "";
            this.btnSaveREye.Enabled = false;
            this.btnSaveLEye.Enabled = false;
        }
        private void CaptureComplete(RME_Error result)
        {
            if (base.InvokeRequired)
            {
                base.BeginInvoke(new frmCapture.CaptureCompleteDelegate(this.CaptureComplete), new object[]
				{
					result
				});
                return;
            }
            this.bnStop.Enabled = false;
            this.bnLeftShow.Enabled = true;
            this.bnRightShow.Enabled = true;
            if (result == RME_Error.RC_SUCCESS)
            {
                this.btnSaveREye.Enabled = (this.btnSaveLEye.Enabled = true);

                /*PGMConverter pgmConverter = new PGMConverter("lEye");
                pgmConverter.ConvertToPGM("leftEye", lEye);

                pgmConverter = new PGMConverter("rEye");
                pgmConverter.ConvertToPGM("rightEye", rEye);*/
                BitmapPgm convert = new BitmapPgm();
                convert.WriteBitmapToPGM("lEye.pgm", lEye);
                convert.WriteBitmapToPGM("rEye.pgm", rEye);
            }
            else
            {
                this.btnSaveREye.Enabled = (this.btnSaveLEye.Enabled = false);
            }
            this.lbLeftQuality.Text = "Quality Score";
            this.lbRightQuality.Text = "Quality Score";
            this.tbLeftQual.Text = ((int)this.LeftEyeQuality.IrisQualityScore).ToString();
            this.tbLFrameCount.Text = this.lFrameCount.ToString();
            this.tbRightQual.Text = ((int)this.RightEyeQuality.IrisQualityScore).ToString();
            this.tbRightFrameCount.Text = this.rFrameCount.ToString();
            this.UpdateIrisImages();
            if (this.camera != null && this.camera.RME_IsCameraConnected())
            {
                this.bnStart.Enabled = true;
            }
        }
        private RME_EyeType WhichEye()
        {
            RME_EyeType result;
            if (this.eyeSelectBox.InvokeRequired)
            {
                frmCapture.WhichEyeCallback method = new frmCapture.WhichEyeCallback(this.WhichEye);
                result = (RME_EyeType)base.Invoke(method);
            }
            else
            {
                switch (this.eyeSelectBox.SelectedIndex)
                {
                    case 0:
                        result = RME_EyeType.RC_RIGHT_EYE;
                        break;
                    case 1:
                        result = RME_EyeType.RC_LEFT_EYE;
                        break;
                    default:
                        result = RME_EyeType.RC_BOTH_EYES;
                        break;
                }
            }
            return result;
        }
        private void UpdateIrisImages()
        {
            if (this.showRightOverlay)
            {
                this.pbRight.Image = this.rEyeOverlay;
                this.bnRightShow.Text = "Show Image";
            }
            else
            {
                this.pbRight.Image = this.rEye;
                this.bnRightShow.Text = "Show Overlay";
            }
            if (this.showLeftOverlay)
            {
                this.pbLeft.Image = this.lEyeOverlay;
                this.bnLeftShow.Text = "Show Image";
                return;
            }
            this.pbLeft.Image = this.lEye;
            this.bnLeftShow.Text = "Show Overlay";
        }
        private void bnLeftShow_Click(object sender, EventArgs e)
        {
            this.showLeftOverlay = !this.showLeftOverlay;
            this.UpdateIrisImages();
        }
        private void bnRightShow_Click(object sender, EventArgs e)
        {
            this.showRightOverlay = !this.showRightOverlay;
            this.UpdateIrisImages();
        }
        private void captureImages()
        {
            IrisImage irisImage = default(IrisImage);
            IrisImage irisImage2 = default(IrisImage);
            this.LeftEyeQuality = default(IrisImageQualityInfo);
            this.RightEyeQuality = default(IrisImageQualityInfo);
            this.lFrameCount = 0;
            this.rFrameCount = 0;
            uint captureTimeoutPeriod = (uint)this.m_timeoutBox.Value;
            this.cameraError = this.camera.RME_StartCapture(new FrameCallBack(this.frame), ref irisImage, ref this.LeftEyeQuality, ref irisImage2, ref this.RightEyeQuality, captureTimeoutPeriod, RME_CapturePurpose.RC_PURPOSE_ENROLL, this.WhichEye());
            if (this.cameraError == RME_Error.RC_SUCCESS)
            {
                if (this.m_beepCheck.Checked)
                {
                    Console.Beep(2000, 250);
                }
            }
            else
            {
                if (this.m_beepCheck.Checked)
                {
                    Console.Beep(1000, 250);
                    Thread.Sleep(100);
                    Console.Beep(1000, 250);
                }
            }
            this.rEye = irisImage2.Image;
            this.rEyeOverlay = irisImage2.Overlay;
            this.lEye = irisImage.Image;
            this.lEyeOverlay = irisImage.Overlay;
            this.CaptureComplete(this.cameraError);
        }
        private void frame(Bitmap bmp, IrisCaptureState state)
        {
            if (base.InvokeRequired)
            {
                if (bmp != null)
                {
                    base.BeginInvoke(new FrameCallBack(this.frame), new object[]
					{
						bmp,
						state
					});
                    return;
                }
            }
            else
            {
                switch (state.WhichEye)
                {
                    case RME_EyeType.RC_NEITHER_EYE:
                        this.bnStop.PerformClick();
                        return;
                    case RME_EyeType.RC_RIGHT_EYE:
                        this.rFrameCount++;
                        this.pbRight.Image = bmp;
                        this.tbRightFrameCount.Text = this.rFrameCount.ToString();
                        this.tbRightQual.Text = state.Focus.ToString();
                        return;
                    case RME_EyeType.RC_LEFT_EYE:
                        this.lFrameCount++;
                        this.pbLeft.Image = bmp;
                        this.tbLFrameCount.Text = this.lFrameCount.ToString();
                        this.tbLeftQual.Text = state.Focus.ToString();
                        break;
                    default:
                        return;
                }
            }
        }
        private void frmCapture_Load_1(object sender, EventArgs e)
        {
            Version version = Assembly.GetExecutingAssembly().GetName().Version;
            string text = version.Major + "." + version.Minor;
            if (version.Build != 0)
            {
                text = text + "." + version.Build;
            }
            this.Text = this.Text + " " + text;
        }
        private void btnSaveREye_Click(object sender, EventArgs e)
        {
            if (this.m_sfd.ShowDialog() == DialogResult.OK)
            {
                this.rEye.Save(this.m_sfd.FileName, ImageFormat.Png);
            }
        }
        private void btnSaveLEye_Click(object sender, EventArgs e)
        {
            if (this.m_sfd.ShowDialog() == DialogResult.OK)
            {
                this.lEye.Save(this.m_sfd.FileName, ImageFormat.Png);
            }
        }

        private void btnUseImage_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
