using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Threading.Tasks;

namespace BPPT_Iris_SDK
{
    public class PGMConverter
    {
        #region Attribute
            private string path;
            private int imageWidth;
            private int imageHeight;

            private string pgmFileName;
            private string pgmFilePath;
        #endregion

        #region Constructor
            public PGMConverter(string path){
                this.path = path;
                this.imageWidth = 320;
                this.imageHeight = 240;
            }
            public PGMConverter(string path, int width, int height)
            {
                this.path = path;
                this.imageWidth = width;
                this.imageHeight = height;
            }

        public PGMConverter(Bitmap bmp)
        {
            this.path = "input_iris.png";
            this.imageWidth = 320;
            this.imageHeight = 240;
        }
        #endregion

        #region Accessor
            public string PgmFilePath
            {
                get
                {
                    return pgmFilePath;
                }
                set
                {
                    this.pgmFilePath = value;
                }
            }

            public string PgmFileName
            {
                get
                {
                    return this.pgmFileName;
                }
                set
                {
                    this.pgmFileName = value;
                }
            }
            #endregion

        #region Method

            public Bitmap ConvertToBitmap()
            {
                Bitmap originalBmpImage = new Bitmap(path);

                Bitmap bmpImage = new Bitmap(imageWidth, imageHeight);
                using (Graphics g = Graphics.FromImage((Image)bmpImage))
                {
                    g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                    g.DrawImage(originalBmpImage, 0, 0, imageWidth, imageHeight);
                }

                return bmpImage;
            }

            public Bitmap ConvertToBitmap(Bitmap originalBmpImage)
            {
                Bitmap bmpImage = new Bitmap(imageWidth, imageHeight);
                using (Graphics g = Graphics.FromImage((Image)bmpImage))
                {
                    g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                    g.DrawImage(originalBmpImage, 0, 0, imageWidth, imageHeight);
                }

                return bmpImage;
            }

            public int[,] ConvertToInt(){
                Bitmap bmpImage = ConvertToBitmap();
                bmpImage.RotateFlip(RotateFlipType.Rotate270FlipY);

                int width = bmpImage.Width;
                int height = bmpImage.Height;

                int[,] pixels = new int[width, height];
                for (int i = 0; i < width; i++)
                {
                    for (int j = 0; j < height; j++)
                    {
                        pixels[i, j] = bmpImage.GetPixel(i, j).ToArgb();
                    }
                }

                pgmFilePath = "C:\\Users\\defianas\\aaaaaaaIris\\";
                pgmFileName = Path.GetFileNameWithoutExtension(path) + ".pgm";

                return pixels;
            }

            public int[,] ConvertToInt(string path)
            {
                Bitmap bmpImage = ConvertToBitmap();
                bmpImage.RotateFlip(RotateFlipType.Rotate270FlipY);

                int width = bmpImage.Width;
                int height = bmpImage.Height;

                int[,] pixels = new int[width, height];
                for (int i = 0; i < width; i++)
                {
                    for (int j = 0; j < height; j++)
                    {
                        pixels[i, j] = bmpImage.GetPixel(i, j).ToArgb();
                    }
                }

                pgmFilePath = path;
                pgmFileName = Path.GetFileNameWithoutExtension(path) + ".pgm";

                return pixels;
            }

            public int[,] ConvertToInt(Bitmap bmp)
            {
                Bitmap bmpImage = ConvertToBitmap(bmp);
                bmpImage.RotateFlip(RotateFlipType.Rotate270FlipY);

                int width = bmpImage.Width;
                int height = bmpImage.Height;

                int[,] pixels = new int[width, height];
                for (int i = 0; i < width; i++)
                {
                    for (int j = 0; j < height; j++)
                    {
                        pixels[i, j] = bmpImage.GetPixel(i, j).ToArgb();
                    }
                }

                pgmFilePath = path;
                pgmFileName = "unwraped_128x8.pgm";

                return pixels;
            }

            public int[,] ConvertToInt(string fileName, Bitmap bmp)
            {
                Bitmap bmpImage = ConvertToBitmap(bmp);
                bmpImage.RotateFlip(RotateFlipType.Rotate270FlipY);

                int width = bmpImage.Width;
                int height = bmpImage.Height;

                int[,] pixels = new int[width, height];
                for (int i = 0; i < width; i++)
                {
                    for (int j = 0; j < height; j++)
                    {
                        pixels[i, j] = bmpImage.GetPixel(i, j).ToArgb();
                    }
                }

                pgmFilePath = path;
                pgmFileName = fileName;

                return pixels;
            }

            public void ConvertToPGM()
            {
                int[,] pixels = ConvertToInt();
                PGM_Iris pgmWriter = new PGM_Iris(pixels.GetLength(1), pixels.GetLength(0));
                pgmWriter.WriteToPath(pgmFilePath+pgmFileName, pixels);
            }

            public void ConvertToPGM(string path)
            {
                int[,] pixels = ConvertToInt(path);
                PGM_Iris pgmWriter = new PGM_Iris(pixels.GetLength(1), pixels.GetLength(0));
                pgmWriter.WriteToPath(pgmFilePath + pgmFileName, pixels);
            }

            public void ConvertToPGM(Bitmap bmp)
            {
                int[,] pixels = ConvertToInt(bmp);
                PGM_Iris pgmWriter = new PGM_Iris(pixels.GetLength(1), pixels.GetLength(0));
                pgmWriter.WriteToPath(pgmFilePath + pgmFileName, pixels);
            }

            public void ConvertToPGM(string fileName, Bitmap bmp)
            {
                int[,] pixels = ConvertToInt(fileName, bmp);
                PGM_Iris pgmWriter = new PGM_Iris(pixels.GetLength(1), pixels.GetLength(0));
                pgmWriter.WriteToPath(pgmFilePath + pgmFileName, pixels);
            }

        public void ConvertToPGM_Awal(string fileName, Bitmap bmp)
        {
            int[,] pixels = ConvertToInt(fileName, bmp);
            PGM_Iris pgmWriter = new PGM_Iris(pixels.GetLength(1), pixels.GetLength(0));
            pgmWriter.WriteToPath(Path.GetFileNameWithoutExtension(fileName) + ".pgm", pixels);
        }
        #endregion

    }
}
