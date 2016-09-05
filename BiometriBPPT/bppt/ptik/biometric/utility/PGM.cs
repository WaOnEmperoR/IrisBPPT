using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using BiometriBPPT.bppt.ptik.biometric.entity;

namespace BiometriBPPT.bppt.ptik.biometric.utility
{
    public class PGM
    {
        private int mWidth;
        private int mHeight;
        private int mColor;
        private string mType;
        private byte[,] mData;
        private string mComments;
        private Bitmap bitmap;

        public Bitmap BitMap
        {
            get
            {
                CreateBitMap();
                return bitmap;
            }
        }

        public string Comment
        {
            get { return this.mComments; }
        }

        public int Width
        {
            get { return this.mWidth; }
        }

        public int Height
        {
            get { return this.mHeight; }
        }

        public int ColorSize
        {
            get { return this.mColor; }
        }
        public string Type
        {
            get
            {
                return this.mType;
            }
        }

        public string Header
        {
            get
            {
                return this.Type + Convert.ToChar(10) +
                    // '#' + this.Comment + Convert.ToChar(10) +
                        "# Created by BPPTAfis" + Convert.ToChar(10) +
                        this.mWidth.ToString() + " " + this.mHeight.ToString() + Convert.ToChar(10) +
                        this.mColor.ToString() + Convert.ToChar(10);
            }
        }

        public byte[,] Data
        {
            get { return this.mData; }
            set { this.mData = value; }
        }


        public PGM(string _filePath)
        {
            ReadPGM(_filePath);
        }

        public PGM(MyImage myimage)
        {
            mHeight = myimage.Height;
            mWidth = myimage.Width;
            mColor = 255;
            mType = "P5";
            Data = new byte[this.mHeight, this.mWidth];
            if (myimage.TypeData == MyImgType.UCHAR)
            {
                System.Array.Copy(myimage.UPixel, Data, Data.Length);
            }else if (myimage.TypeData == MyImgType.FLOAT)
            {
                MatrixBppt mat = new MatrixBppt();
                System.Array.Copy(mat.Mat_FloatToChar(myimage).UPixel, Data, Data.Length);

            } 
            //Data = myimage.Pixel;

        }

        public void Save(string _filePath)
        {
            WritePGM(_filePath);
        }

        private void ReadPGM(string _filePath)
        {
            FileStream InputStream = File.OpenRead(_filePath);
            BinaryReader PGMReader = new BinaryReader(InputStream);
            char[] Seperators = { ' ', '\n' };

            byte NewLineAsciiCode = 10;
            byte DiezAsciiCode = 35;
            byte SpaceAsciiCode = 32;
            byte[] TempArray = new byte[1000];
            int i = 0;

            string TempS;
            byte TempByte;


            /* Sample PGM :
             * 
             * 
             * P5
             * # Created by ...
             * 512 512
             * 255
             * [data]
             */


            //read PGM Type P5
            TempArray[0] = PGMReader.ReadByte();
            TempArray[1] = PGMReader.ReadByte();
            this.mType = System.Text.ASCIIEncoding.Default.GetString(TempArray, 0, 2);

            if (this.mType == "‰P")
            {
                ReadBitmap(_filePath);
                return;
            }

            //read until new line
            while (PGMReader.ReadByte() != NewLineAsciiCode) { ;}

            //read comments if exists. Only one comment line supported!!
            i = 0;
            TempArray[i] = PGMReader.ReadByte();
            if (TempArray[i] == DiezAsciiCode)
            {
                TempByte = PGMReader.ReadByte();
                while (TempByte != NewLineAsciiCode)
                {
                    TempArray[i++] = TempByte;
                    TempByte = PGMReader.ReadByte();
                }
                this.mComments = System.Text.ASCIIEncoding.Default.GetString(TempArray, 0, i);
                i = 0;
            }
            else
            {
                i = 1;
            }


            //read width

            TempByte = PGMReader.ReadByte();
            while (TempByte != SpaceAsciiCode)
            {
                TempArray[i++] = TempByte;
                TempByte = PGMReader.ReadByte();
            }

            TempS = System.Text.ASCIIEncoding.Default.GetString(TempArray, 0, i);
            this.mWidth = Convert.ToInt32(TempS);

            //read length
            i = 0;
            TempByte = PGMReader.ReadByte();
            while (TempByte != NewLineAsciiCode)
            {
                TempArray[i++] = TempByte;
                TempByte = PGMReader.ReadByte();
            }

            TempS = System.Text.ASCIIEncoding.Default.GetString(TempArray, 0, i);
            this.mHeight = Convert.ToInt32(TempS);

            //read color
            i = 0;
            TempByte = PGMReader.ReadByte();
            while (TempByte != NewLineAsciiCode)
            {
                TempArray[i++] = TempByte;
                TempByte = PGMReader.ReadByte();
            }

            TempS = System.Text.ASCIIEncoding.Default.GetString(TempArray, 0, i);
            this.mColor = Convert.ToInt32(TempS);

            //read image data
            byte[] PGMDataBuffer = new byte[this.mWidth * this.mHeight];
            //int k = 0;
            if (this.mType == "P5")
            {
                //If file is binary, read every byte
                byte[] ReadedByte = PGMReader.ReadBytes(PGMDataBuffer.Length);
                Array.Copy(ReadedByte, PGMDataBuffer, ReadedByte.Length);
            }
            else
            {
                //Magic number is not recognized
                return;
            }
            this.mData = new byte[this.mHeight, this.mWidth];
            Buffer.BlockCopy(PGMDataBuffer, 0, this.mData, 0, PGMDataBuffer.Length);
            //this.mData = PGMDataBuffer;

            PGMReader.Close();
            InputStream.Close();
        }

        private void WritePGM(string _filePath)
        {
            FileStream OutputStream = File.Create(_filePath);
            BinaryWriter PGMWriter = new BinaryWriter(OutputStream);

            string PGMInfo = this.Header;
            byte[] PGMInfoBuffer = System.Text.ASCIIEncoding.Default.GetBytes(PGMInfo);
            PGMWriter.Write(PGMInfoBuffer);
            if (this.mType == "P5")
            {
                //File is binary, write complete data
                byte[] PGMDataBuffer = new byte[this.mWidth * this.mHeight];
                Buffer.BlockCopy(this.mData, 0, PGMDataBuffer, 0, this.mData.Length);
                //PGMWriter.Write(this.mData);
                PGMWriter.Write(PGMDataBuffer);
            }
            else
            {
                //Magic number is not recognized
                return;
            }
            PGMWriter.Close();

        }

        public void ReadBitmap(string _filePath)
        {
            Bitmap gambar = new Bitmap(_filePath);

            this.mWidth = gambar.Width;
            this.mHeight = gambar.Height;
            this.mType = "P5";
            this.mColor = 255;

            //string PGMInfo = this.Header;
            //byte[] PGMInfoBuffer = System.Text.ASCIIEncoding.Default.GetBytes(PGMInfo);
            byte[] PGMDataBuffer = new byte[this.mWidth * this.mHeight];

            for (int x = 0; x < gambar.Height; x++)
                for (int y = 0; y < gambar.Width; y++)
                {
                    Color color = gambar.GetPixel(y, x);
                    //writerB.Write(color.R);
                    PGMDataBuffer[(x * gambar.Width) + y] = color.R;
                    //writerB.Write(color.B);
                }
            this.mData = new byte[this.mHeight, this.mWidth];
            Buffer.BlockCopy(PGMDataBuffer, 0, this.mData, 0, PGMDataBuffer.Length);

        }

        private Bitmap CreateBitMap()
        {
            bitmap = new Bitmap(Width, Height);
            for (int j = 0; j < Height; j++)
                for (int i = 0; i < Width; i++)
                {
                    int a = Data[j, i];
                    bitmap.SetPixel(i, j, Color.FromArgb(a, a, a));
                }
            return bitmap;
        }


        public void PrintPGMInfo()
        {
            Console.WriteLine("Type       = " + this.Type.ToString());
            Console.WriteLine("ColorSize  = " + this.ColorSize.ToString());
            Console.WriteLine("Comment    = " + this.Comment);
            Console.WriteLine("Height     = " + this.Height.ToString());
            Console.WriteLine("Width      = " + this.Width.ToString());
        }
        /*
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
         * */

    }
}
