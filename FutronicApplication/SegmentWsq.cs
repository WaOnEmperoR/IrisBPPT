/*
 * update 28 Oct 2012
 * menambahkan batasan agar saat proses crop, tidak ada nilai x atau y yang di luar dimensi image aslinya.
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Drawing;
using Nbis;


namespace FutronicDrv
{
    
    class SegmentWsq
    {
        // mode capture:
        private int jumlahJari = 2;                         
        //tingkat kompresi
        float kompresi = (float)0.75;                                   //1:5 atau 0.75 (1:25)
        int DPI = 500;

        public List<byte[]> ProsesAll(Bitmap gambarInput, int modePilihan)
        {
            List<byte[]> hasilB = new List<byte[]>();
            List<Bitmap> hasils = new List<Bitmap>();           // list hasil segmentasi (berurut dari sisi kiri gambar hingga ke kanan gambar)
            SegmenterParameters option = new SegmenterParameters();
            
            jumlahJari = 1;
                if (modePilihan == 1)
                    option.TargetFingers = Finger.RightIndex;
                else if (modePilihan == 2)
                    option.TargetFingers = Finger.LeftIndex;
                
            Segment[] gambarSegment = new Segment[jumlahJari];
            gambarSegment = Segmentation.FromBitmap(gambarInput, option);
            for (int i = 0; i < jumlahJari; i++)
            {
                int x = gambarSegment[i].OriginX - (gambarSegment[i].Width / 2);
                if (x < 0)
                    x = 0;
                if ((x + gambarSegment[i].Width) >= gambarInput.Width)
                    x = gambarInput.Width - gambarSegment[i].Width;

                int y = gambarSegment[i].OriginY - (gambarSegment[i].Height / 2);
                if (y < 0)
                    y = 0;
                if ((y + gambarSegment[i].Height) >= gambarInput.Height)
                    y = gambarInput.Height - gambarSegment[i].Height;

                //// update 28 Oct 2012
                //int x = gambarSegment[i].OriginX - (gambarSegment[i].Width / 2);
                //int y = gambarSegment[i].OriginY - (gambarSegment[i].Height / 2);
                Bitmap hasil = new Bitmap(gambarSegment[i].Width, gambarSegment[i].Height, System.Drawing.Imaging.PixelFormat.Format8bppIndexed);
                hasil = CropBitmap(gambarInput, x, y, gambarSegment[i].Width, gambarSegment[i].Height);
                hasils.Add(hasil);
            }

            // convert to WSQ
           
            for (int i = 0; i < jumlahJari; i++)
            {
                byte[] hasil = new byte[hasils[i].Height * hasils[i].Width];
                hasil = Wsq.FromBitmapToWsq(hasils[i], kompresi, 500, "hasil");
                hasilB.Add(hasil);
            }
            return hasilB;
        }

        public List<Bitmap> ProsesSegmentasi(Bitmap gambarInput, int modePilihan)
        {
            List<Bitmap> hasils = new List<Bitmap>();           // list hasil segmentasi (berurut dari sisi kiri gambar hingga ke kanan gambar)
            SegmenterParameters option = new SegmenterParameters();
            jumlahJari = 1;
            if (modePilihan == 0)
                option.TargetFingers = Finger.RightIndex;
            else if (modePilihan == 1)
                option.TargetFingers = Finger.LeftIndex;

            Segment[] gambarSegment = new Segment[jumlahJari];
            gambarSegment = Segmentation.FromBitmap(gambarInput, option);
            for (int i = 0; i < jumlahJari; i++)
            {
                int x = gambarSegment[i].OriginX - (gambarSegment[i].Width / 2);
                if (x < 0)
                    x = 0;
                if ((x + gambarSegment[i].Width) >= gambarInput.Width)
                    x = gambarInput.Width - gambarSegment[i].Width;

                int y = gambarSegment[i].OriginY - (gambarSegment[i].Height / 2);
                if (y < 0)
                    y = 0;
                if ((y + gambarSegment[i].Height) >= gambarInput.Height)
                    y = gambarInput.Height - gambarSegment[i].Height;


                //int x = gambarSegment[i].OriginX - (gambarSegment[i].Width / 2);
                //int y = gambarSegment[i].OriginY - (gambarSegment[i].Height / 2);
                Bitmap hasil = new Bitmap(gambarSegment[i].Width, gambarSegment[i].Height, System.Drawing.Imaging.PixelFormat.Format8bppIndexed);
                hasil = CropBitmap(gambarInput, x, y, gambarSegment[i].Width, gambarSegment[i].Height);
                hasils.Add(hasil);
            }

            return hasils;
        }

        public Bitmap ProsesSegmentasi2(Bitmap gambarInput, int modePilihan)
        {
            SegmenterParameters option = new SegmenterParameters();
            jumlahJari = 1;
            if (modePilihan == 0)
                option.TargetFingers = Finger.RightIndex;
            else if (modePilihan == 1)
                option.TargetFingers = Finger.LeftIndex;
            
            Segment[] gambarSegment = new Segment[1];
            gambarSegment = Segmentation.FromBitmap(gambarInput, option);
            int x = gambarSegment[0].OriginX - (gambarSegment[0].Width / 2);
                if (x < 0)
                    x = 0;
                if ((x + gambarSegment[0].Width) >= gambarInput.Width)
                    x = gambarInput.Width - gambarSegment[0].Width;

                int y = gambarSegment[0].OriginY - (gambarSegment[0].Height / 2);
                if (y < 0)
                    y = 0;
                if ((y + gambarSegment[0].Height) >= gambarInput.Height)
                    y = gambarInput.Height - gambarSegment[0].Height;


                //int x = gambarSegment[i].OriginX - (gambarSegment[i].Width / 2);
                //int y = gambarSegment[i].OriginY - (gambarSegment[i].Height / 2);
                Bitmap hasil = new Bitmap(gambarSegment[0].Width, gambarSegment[0].Height, System.Drawing.Imaging.PixelFormat.Format8bppIndexed);
                hasil = CropBitmap(gambarInput, x, y, gambarSegment[0].Width, gambarSegment[0].Height);
                //Console.WriteLine(x + " " + y + " " + gambarSegment[0].Width + " " + gambarSegment[0].Height);

            return hasil;
        }

        public List<byte[]> Bmp2Wsq(List<Bitmap> gambarInput, int modePilihan)
        {
            ////wsq converter purposes
            //string filename1 = null;
            //string filename2 = null;
            //string filename3 = null;
            //string filename4 = null;

            List<byte[]> hasils = new List<byte[]>();
           

                jumlahJari = 1;

            for (int i = 0; i < jumlahJari; i++)
            {
                byte[] hasil = new byte[gambarInput[i].Height * gambarInput[i].Width];
                hasil = Wsq.FromBitmapToWsq(gambarInput[i], kompresi, 500, "hasil");
                hasils.Add(hasil);
            }

            //FileStream filestream0 = new FileStream(filename1, FileMode.Create);
            //for (int i = 0; i < hasils[0].Length; i++)
            //{
            //    filestream0.WriteByte(hasils[0][i]);
            //}
            //if (jumlahJari != 1)
            //{
            //    FileStream filestream1 = new FileStream(filename2, FileMode.Create);
            //    for (int i = 0; i < hasils[1].Length; i++)
            //    {
            //        filestream1.WriteByte(hasils[1][i]);
            //    }
            //    if (jumlahJari != 2)
            //    {
            //        FileStream filestream2 = new FileStream(filename3, FileMode.Create);
            //        FileStream filestream3 = new FileStream(filename4, FileMode.Create);
            //        for (int i = 0; i < hasils[2].Length; i++)
            //        {
            //            filestream2.WriteByte(hasils[2][i]);
            //        }
            //        for (int i = 0; i < hasils[3].Length; i++)
            //        {
            //            filestream3.WriteByte(hasils[3][i]);
            //        }
            //    }
            //}
            return hasils;
        }

        public byte[] Bmp2Wsq2(Bitmap gambarInput, int modePilihan)
        {
         
         
            byte[] hasil = new byte[gambarInput.Height * gambarInput.Width];
                hasil = Wsq.FromBitmapToWsq(gambarInput, kompresi, 500, "hasil");


                return hasil;
        }

        public byte[] Bmp2Wsq(Bitmap gambarInput)
        {
            Bitmap b0 = bpp.CopyToBpp(gambarInput, 8);
            byte[] hasil = new byte[gambarInput.Height * gambarInput.Width];
            hasil = Wsq.FromBitmapToWsq(b0, kompresi, DPI, "hasil");
            return hasil;
        }

        public Bitmap Wsq2Bmp(byte[] wsqInput)
        {
            Bitmap hasil = Wsq.FromWsqToBitmap(wsqInput);
            return hasil;
        }

        public Bitmap CropBitmap(Bitmap bitmap, int cropX, int cropY, int cropWidth, int cropHeight)
        {
            Rectangle rect = new Rectangle(cropX, cropY, cropWidth, cropHeight);
            Bitmap cropped = bitmap.Clone(rect, bitmap.PixelFormat);
            return cropped;
        }

    }
}
