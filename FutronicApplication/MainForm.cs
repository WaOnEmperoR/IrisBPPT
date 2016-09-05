using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using FutronicDrv;
using System.IO;
using SourceAFIS.Simple;
using BPPTAfis.bppt.ptik.biometri.afis.utility;
using BPPTAfis.bppt.ptik.biometri.afis.main;
using BPPTAfis.bppt.ptik.biometri.afis.entity;
//using Nbis;
using System.Drawing.Imaging;

namespace FutronicApplication
{
    public partial class MainForm : Form
    {
        string templateName = "template1";
        string templateType = "Iso";
        static Timer _timer;

        public MainForm()
        {
            InitializeComponent();
            cbTemplateName.SelectedIndex = 0;
        }

        private void enrollToolStripMenuItem_Click(object sender, EventArgs e)
        {
            emptyTB();
            //_timer.Enabled = false;

            MainDrv mdr = new MainDrv(templateType);

            if (templateType.Equals("BPPT"))
            {
                Bitmap b0 = mdr.capture();
                pictureBox1.Image = b0;

                BitmapPgm bPgm = new BitmapPgm();
                MyImage myImage;

                b0 = bpp.CopyToBpp(b0, 8);
                //int q0 = Nfiq.FromBitmap(b0, 500);
                //tbImageQuality.Text = q0.ToString() + " of 5 (1 is best)";

                myImage = bPgm.bitmapToPGM(b0);

                BpptEngine bEngine = new BpptEngine(false);
                //bEngine.extractMinutiaeLama(myImage, templateName);
                templateName = namaJari.Text;
                //MatrixBppt matb = new MatrixBppt(); //baru
                MyImage flipImage = MatrixBppt.Mat_FlipV(myImage, null); //baru
                bEngine.extractMinutiaeIso(flipImage, templateName);
            }
            else
            {
                Fingerprint fp = mdr.getFingerprint();
                if (fp == null)
                {
                    return;
                }
                Bitmap b0 = fp.AsBitmap;
                b0.Save("jari.bmp", ImageFormat.Bmp);
                pictureBox1.Image = b0;

                //BitmapPgm bPgm = new BitmapPgm();
                //templateName = namaJari.Text;
                //bPgm.WriteBitmapToPGM("jari\\" + templateName + ".pgm", b0);

                b0 = bpp.CopyToBpp(b0, 8);

                //int q0 = Nfiq.FromBitmap(b0, 500);
                //tbImageQuality.Text = q0.ToString() + " of 5 (1 is best)";

                Person person = new Person();//buat objek Person untuk kandidat verifikasi
                person.Fingerprints.Add(fp);

                if (templateType.Equals("Compact"))
                {
                    tbTemplateSize.Text = fp.Template.Length.ToString() + " bytes";
                }
                else if (templateType.Equals("Iso"))
                {
                    tbTemplateSize.Text = fp.AsIsoTemplate.Length.ToString() + " bytes";
                }
                //_timer.Enabled = true;
                mdr.saveTemplate(fp, templateName);
            }
        }

        private void verifyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            emptyTB();
            //_timer.Enabled = false;
            MainDrv mdr = new MainDrv(templateType);
            if (templateType.Equals("BPPT"))
            {
                Bitmap b0 = mdr.capture();
                pictureBox1.Image = b0;

                b0 = bpp.CopyToBpp(b0, 8);
                //int q0 = Nfiq.FromBitmap(b0, 500);
                //tbImageQuality.Text = q0.ToString() + " of 5 (1 is best)";

                BitmapPgm bPgm = new BitmapPgm();
                MyImage myImage;
                myImage = bPgm.bitmapToPGM(b0);

                BpptEngine bEngine = new BpptEngine(false);
                //Minutiae[] mntFilter;
                //bEngine.extractMyImage2Minutiae(myImage, "", out mntFilter);
                bEngine.matchMinutiae(myImage, "FingerPrintsBppt\\template2");
                //tbScore.Text = score.ToString();
            }
            else
            {
                try
                {
                    AfisEngine Afis = new AfisEngine();
                    
                    Fingerprint fp;
                    Person candidate, probe;


                    List<Person> myPersons = new List<Person>();

                    candidate = new Person();//buat objek Person untuk kandidat verifikasi

                    fp = new Fingerprint();
                    fp = mdr.setTemplate2Fingerprint(template1Tb.Text);//fp = mdr.setTemplate2Fingerprint("template1");//ambil template1
                    //fp.Finger = SourceAFIS.Simple.Finger.RightIndex;
                    candidate.Fingerprints.Add(fp);

                    fp = new Fingerprint();
                    fp = mdr.setTemplate2Fingerprint(template2Tb.Text);//fp = mdr.setTemplate2Fingerprint("template2");//ambil template2
                    //fp.Finger = SourceAFIS.Simple.Finger.LeftIndex;
                    candidate.Fingerprints.Add(fp);

                    myPersons.Add(candidate);


                    fp = new Fingerprint();
                    fp = mdr.getFingerprint();//ambil data baru dr scanner

                    Bitmap b0 = fp.AsBitmap;
                    b0 = bpp.CopyToBpp(b0, 8);
                    pictureBox1.Image = b0;

                    //int q0 = Nfiq.FromBitmap(b0, 500);
                    //tbImageQuality.Text = q0.ToString() + " of 5 (1 is best)";

                    probe = new Person();//buat objek Person untuk verifikasi

                    probe.Fingerprints.Add(fp);

                    // Look up the probe using Threshold = 10
                    Afis.Threshold = 25;

                    Person match = Afis.Identify(probe, myPersons).FirstOrDefault() as Person;

                    // Null result means that there is no candidate with similarity score above threshold
                    if (match == null)
                    {
                        MessageBox.Show("No matching person found.");
                        return;
                    }
                    // Print out any non-null result

                    // Compute similarity score
                    float score = Afis.Verify(probe, match);
                    tbScore.Text = score.ToString();
                    //_timer.Enabled = true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void cbTemplateName_SelectedIndexChanged(object sender, EventArgs e)
        {
            templateName = cbTemplateName.SelectedItem.ToString();
        }

        private void isoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (compactToolStripMenuItem.Checked || bPPTToolStripMenuItem.Checked)
            {
                bPPTToolStripMenuItem.Checked = false;
                isoToolStripMenuItem.Checked = true;
                compactToolStripMenuItem.Checked = false;
                templateType = "Iso";
            }
        }

        private void compactToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (isoToolStripMenuItem.Checked || bPPTToolStripMenuItem.Checked)
            {
                bPPTToolStripMenuItem.Checked = false;
                isoToolStripMenuItem.Checked = false;
                compactToolStripMenuItem.Checked = true;
                templateType = "Compact";
            }
        }

        private void emptyTB()
        {
            tbScore.Text = "";
            tbImageQuality.Text = "";
            tbTemplateSize.Text = "";
        }

        private void Form1_Load(object sender, EventArgs e)
        {

            _timer = new Timer();
            _timer.Interval = 500;
            //_timer.Enabled = true;
            //_timer.Tick += getBmp;
            //_timer.Start(); 
        }

        private void getBmp(object sender, EventArgs e)
        {
            MainDrv mdr = new MainDrv(templateType);
            pictureBox1.Image = mdr.capture();
        }

        private void bPPTToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (isoToolStripMenuItem.Checked || compactToolStripMenuItem.Checked)
            {
                bPPTToolStripMenuItem.Checked = true;
                isoToolStripMenuItem.Checked = false;
                compactToolStripMenuItem.Checked = false;
                templateType = "BPPT";
            }
        }

        private void tesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            emptyTB();
            //_timer.Enabled = false;
            MainDrv mdr = new MainDrv(templateType);
            try
            {
                AfisEngine Afis = new AfisEngine();
                Fingerprint fp;
                Person candidate, probe;


                List<Person> myPersons = new List<Person>();

                candidate = new Person();//buat objek Person untuk kandidat verifikasi

                fp = new Fingerprint();
                //fp = mdr.setTemplate2Fingerprint("telunjukKananRully1bppt");//ambil template1
                fp = mdr.setTemplate2Fingerprint(template1Tb.Text);//ambil template1
                candidate.Fingerprints.Add(fp);

                myPersons.Add(candidate);

                fp = new Fingerprint();
                //fp = mdr.setTemplate2Fingerprint("telunjukKananRully2bppt");//ambil template2
                fp = mdr.setTemplate2Fingerprint(template2Tb.Text);//ambil template2

                probe = new Person();//buat objek Person untuk verifikasi

                probe.Fingerprints.Add(fp);

                // Look up the probe using Threshold = 10
                Afis.Threshold = 1;

                Person match = Afis.Identify(probe, myPersons).FirstOrDefault() as Person;

                // Null result means that there is no candidate with similarity score above threshold
                if (match == null)
                {
                    MessageBox.Show("No matching person found.");
                    return;
                }
                // Print out any non-null result

                // Compute similarity score
                float score = Afis.Verify(probe, match);
                tbScore.Text = score.ToString();
                //_timer.Enabled = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
