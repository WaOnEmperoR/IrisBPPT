using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Drawing;
using System.IO;
using SourceAFIS.Simple;
using FutronicDrv;
using System.Windows.Forms;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;

namespace FutronicApplication
{
    public class MainDrv
    {
        String templateType;
        static AfisEngine Afis = new AfisEngine();
        
        public MainDrv()
        {
            templateType = "Iso";
        }

        public MainDrv(String templateType)
        {
            TemplateType = templateType;
        }

        public String TemplateType
        {
            get { return templateType; }
            set { templateType = value; }
        }

        public Bitmap capture()
        {

            Device device = new Device();
            Bitmap b0;
            
            if (!device.Init())
            {
                MessageBox.Show("The Fingerprint scanner not available");
                return null;
            }

            bool green = true;
            bool red = true;
            device.GetDiodesStatus(out green, out red);
            device.SetDiodesStatus(true, true);
            device.SetDiodesStatus(false, false);

            b0 = device.ExportBitMap();

            device.Dispose();

            return b0;
        }

        public Fingerprint getFingerprint()
        {

            Device device = new Device();
            Bitmap finger;
            Bitmap finger0;
            

            if (!device.Init())
            {
                MessageBox.Show("The Fingerprint scanner not available");
                return null;
            }

            bool green = true;
            bool red = true;
            device.GetDiodesStatus(out green, out red);
            device.SetDiodesStatus(true, true);
            device.SetDiodesStatus(false, false);

            finger = device.ExportBitMap();
            finger0 = bpp.CopyToBpp(finger, 8);

            Fingerprint fp = new Fingerprint();
            fp.AsBitmap = finger0;

            Person person = new Person();

            // Add fingerprint to the person
            person.Fingerprints.Add(fp);

            // Execute extraction in order to initialize fp.Template
            Afis.Extract(person);
            device.Dispose();

            return fp;
        }

        public Fingerprint getFingerprintFromBmp(Bitmap finger)
        {

            Bitmap finger0 = bpp.CopyToBpp(finger, 8);

            Fingerprint fp = new Fingerprint();
            fp.AsBitmap = finger0;

            Person person = new Person();

            // Add fingerprint to the person
            person.Fingerprints.Add(fp);

            // Execute extraction in order to initialize fp.Template
            Afis.Extract(person);
           
            return fp;
        }

        public void saveTemplate(Fingerprint fp, String templateName)
        {
            FileStream fileStream = null;
            if (templateName == "")
            {
                templateName = "template";
            }

            string subPath = "";

            if (TemplateType.Equals("Compact"))
            {
                subPath = "FingerPrints";
            }
            else if (TemplateType.Equals("Iso"))
            {
                subPath = "FingerPrintsIso";
            }
            bool IsExists = System.IO.Directory.Exists(subPath);

            if (!IsExists)
                System.IO.Directory.CreateDirectory(subPath);

            fileStream = new FileStream(subPath + "\\" + templateName, FileMode.Create);

            if (TemplateType.Equals("Compact"))
            {
                for (int x = 0; x < fp.Template.Length; x++)
                {
                    fileStream.WriteByte(fp.Template[x]);
                }
            }
            else if (TemplateType.Equals("Iso"))
            {
                for (int x = 0; x < fp.AsIsoTemplate.Length; x++)
                {
                    fileStream.WriteByte(fp.AsIsoTemplate[x]);
                }
            }

            fileStream.Close();
        }

        public Fingerprint setTemplate2Fingerprint(String templateName)
        {
            FileStream fileStream = null;
            Fingerprint fp = new Fingerprint();

            string subPath = "";

            if (TemplateType.Equals("Compact"))
            {
                subPath = "FingerPrints";
            }
            else if (TemplateType.Equals("Iso"))
            {
                subPath = "FingerPrintsIso";
            }

            fileStream = new FileStream(subPath+"\\"+templateName, FileMode.Open);
            byte[] templateFinger = new byte[fileStream.Length];
            fileStream.Seek(0, SeekOrigin.Begin);
            for (int i = 0; i < fileStream.Length; i++)
            {
                templateFinger[i] = (byte)fileStream.ReadByte();
            }
            //fp.AsIsoTemplate = templateFinger;

            try
            {
                if (TemplateType.Equals("Compact"))
                {
                    fp.Template = templateFinger;
                }
                else if (TemplateType.Equals("Iso"))
                {
                    fp.AsIsoTemplate = templateFinger;
                }
            }
            catch (Exception ex)
            {
                return null;

            }
            fileStream.Close();
            return fp;
        }

        public Bitmap resizeImage(int newWidth, int newHeight, Bitmap imgPhoto)
        {
            int sourceWidth = imgPhoto.Width;
            int sourceHeight = imgPhoto.Height;

            //Consider vertical pics
            if (sourceWidth < sourceHeight)
            {
                int buff = newWidth;

                newWidth = newHeight;
                newHeight = buff;
            }

            int sourceX = 0, sourceY = 0, destX = 0, destY = 0;
            float nPercent = 0, nPercentW = 0, nPercentH = 0;

            nPercentW = ((float)newWidth / (float)sourceWidth);
            nPercentH = ((float)newHeight / (float)sourceHeight);
            if (nPercentH < nPercentW)
            {
                nPercent = nPercentH;
                destX = System.Convert.ToInt16((newWidth -
                          (sourceWidth * nPercent)) / 2);
            }
            else
            {
                nPercent = nPercentW;
                destY = System.Convert.ToInt16((newHeight -
                          (sourceHeight * nPercent)) / 2);
            }

            int destWidth = (int)(sourceWidth * nPercent);
            int destHeight = (int)(sourceHeight * nPercent);


            Bitmap bmPhoto = new Bitmap(newWidth, newHeight,
                          PixelFormat.Format24bppRgb);

            bmPhoto.SetResolution(imgPhoto.HorizontalResolution,
                         imgPhoto.VerticalResolution);

            Graphics grPhoto = Graphics.FromImage(bmPhoto);
            grPhoto.Clear(Color.Black);
            grPhoto.InterpolationMode =
                InterpolationMode.HighQualityBicubic;

            grPhoto.DrawImage(imgPhoto,
                new Rectangle(destX, destY, destWidth, destHeight),
                new Rectangle(sourceX, sourceY, sourceWidth, sourceHeight),
                GraphicsUnit.Pixel);

            grPhoto.Dispose();
            imgPhoto.Dispose();
            return bmPhoto;
        }

    }
}
