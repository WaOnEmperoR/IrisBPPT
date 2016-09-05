using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BiometriBPPT.bppt.ptik.biometric.entity
{
    public class MyImage
    {
        private int height;
        private int width;
        private byte[,] uPixel;
        private float[,] fPixel;
        private int[,] iPixel;
        private double[,] dPixel;
        private MyImgType typeData = MyImgType.UCHAR;

        public MyImage(int height, int width) {
            Height = height;
            Width = width;
            UPixel = new byte[Height, Width];
        }

        public MyImage(int height, int width, MyImgType typeData)
        {
            Height = height;
            Width = width;
            TypeData = typeData;
            if (typeData == MyImgType.FLOAT)
            {
                FPixel = new float[Height,Width];
            }
            else if (typeData.Equals(MyImgType.UCHAR))
            {
                UPixel = new byte[Height, Width];
            }
            else if (typeData.Equals(MyImgType.INT))
            {
                IPixel = new int[Height, Width];
            }
            else if (typeData.Equals(MyImgType.DOUBLE))
            {
                DPixel = new double[Height, Width];
            }
        }

        public int Height
        {
            get { return height; }
            set { height = value; }
        }

        public int Width
        {
            get { return width; }
            set { width = value; }
        }

        public MyImgType TypeData
        {
            get { return typeData; }
            set { typeData = value; }
        }

        public byte[,] UPixel
        {
            get { return uPixel; }
            set { uPixel = value; }
        }

        public float[,] FPixel
        {
            get { return fPixel; }
            set { fPixel = value; }
        }

        public int[,] IPixel
        {
            get { return iPixel; }
            set { iPixel = value; }
        }

        public double[,] DPixel
        {
            get { return dPixel; }
            set { dPixel = value; }
        }
    }
}
