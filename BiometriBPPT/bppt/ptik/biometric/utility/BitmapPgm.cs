using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using BiometriBPPT.bppt.ptik.biometric.entity;

namespace BiometriBPPT.bppt.ptik.biometric.utility
{
    public class BitmapPgm
    {
        public void WriteBitmapToPGM(string file, Bitmap bitmap)
        {
            MyImage img = new MyImage(bitmap.Height, bitmap.Width, MyImgType.UCHAR);
            for (int x = 0; x < bitmap.Height; x++)
                for (int y = 0; y < bitmap.Width; y++)
                {
                    Color color = bitmap.GetPixel(y, x);
                    //writerB.Write(color.R);
                    //writerB.Write(color.G);
                    //writerB.Write(color.B);
                    img.UPixel[x, y] = (byte)((color.R * 0.3) + (color.G * 0.59) + (color.B * 0.11));
                }
            PGM pgm = new PGM(img);
            pgm.Save(file);
        }

        public MyImage bitmapToPGM(Bitmap bitmap)
        {
            MyImage img = new MyImage(bitmap.Height, bitmap.Width, MyImgType.UCHAR);
            for (int x = 0; x < bitmap.Height; x++)
                for (int y = 0; y < bitmap.Width; y++)
                {
                    Color color = bitmap.GetPixel(y, x);
                    //writerB.Write(color.R);
                    //writerB.Write(color.G);
                    //writerB.Write(color.B);
                    img.UPixel[x, y] = (byte)((color.R * 0.3) + (color.G * 0.59) + (color.B * 0.11));
                }
            //PGM pgm = new PGM(img);
            return img;
        }
    }
}
