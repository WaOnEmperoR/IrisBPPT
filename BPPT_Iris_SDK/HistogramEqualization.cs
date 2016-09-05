using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BPPT_Iris_SDK
{
    public class HistogramEqualization
    {
        #region Attributes

        #region Image Pixels
        private int[,] pixels;
        #endregion

        #region Height
        private int height;
        #endregion

        #region Width
        private int width;
        #endregion

        #region Histogram
        private int[] rk;
        #endregion

        #region Area
        private int area;
        #endregion

        #endregion

        #region Constructors

        public HistogramEqualization(int[,] pixels, int[] rk)
        {
            this.pixels = pixels;
            this.height = pixels.GetLength(0);
            this.width = pixels.GetLength(1);
            this.rk = rk;

            CountArea();
        }

        #endregion

        #region Methods

        #region Method Used to Count Total Area
        private void CountArea()
        {
            this.area = 0;

            for (int i = 0; i < this.rk.Length; i++)
            {
                this.area += this.rk[i];
            }
        }
        #endregion

        #region Method Used to Count P(rk)
        public float[] countPrk()
        {
            float[] prk = new float[this.rk.Length];
            for (int i = 0; i < this.rk.Length; i++)
            {
                prk[i] = (float)rk[i] / area;
            }

            return prk;
        }
        #endregion

        #region Method Used to Count S(k)
        public int[] countSk(float[] prk)
        {
            int length = 1;
            int[] sk = new int[prk.Length];

            for (int i = 0; i < prk.Length; i++)
            {
                float amount_prk = 0;
                for (int j = 0; j < length; j++)
                {
                    amount_prk += prk[j];
                }
                sk[i] = (int)(Math.Round(255 * amount_prk));
                length++;
            }

            return sk;
        }
        #endregion

        #region Method Used to Count P(sk)
        public float[] countPsk(int[] sk, float[] prk)
        {
            float[] psk = new float[sk.Length];

            for (int i = 0; i < sk.Length; i++)
            {
                float temp = 0;

                for (int j = 0; j < 256; j++)
                {
                    if (i == sk[j])
                    {
                        temp += prk[j];
                    }
                }

                psk[i] = temp;
            }

            return psk;
        }
        #endregion

        #region Method Used to Generate New Pixels
        public int[,] GenerateNewPixels(int[] sk)
        {
            int[,] newPixels = new int[height, width];
            int y = 0;
            for (int j = 0; j < height; j++)
            {
                for (int i = 0; i < width; i++)
                {
                    y = pixels[j, i];
                    newPixels[j, i] = sk[y];
                }
            }

            return newPixels;
        }
        #endregion

        #endregion
    }
}
