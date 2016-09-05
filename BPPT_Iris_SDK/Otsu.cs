using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BPPT_Iris_SDK
{
    public class Otsu
    {
        int L_MAX = 256;
        int[,] g_pixel;

        private int threshold;

        public Otsu(int[,] _pixel)
        {
            g_pixel = _pixel;
        }

        public int[,] runOtsu()
        {
            int height = g_pixel.GetLength(0);
            int width = g_pixel.GetLength(1);
            int[,] temp_pixel = new int[height, width];
            double[] pi = new double[256];
            double[] p1 = new double[256];
            double[] p2 = new double[256];
            double[] m1 = new double[256];
            double[] m2 = new double[256];
            double[] bcv = new double[256];
            double temp = 0.0D;
            int temp2 = 0;
            double mg = 0.0D;
            int sum = 0;
            int i, j;
            int[] pixintensity = new int[256];
            for (j = 0; j < height; j++)
            {
                for (i = 0; i < width; i++)
                {
                    temp_pixel[j, i] = g_pixel[j, i];
                    pixintensity[this.g_pixel[j, i]] += 1;
                }
            }
            for (i = 0; i < 256; i++)
            {
                sum += pixintensity[i];
            }
            for (i = 0; i < 256; i++)
            {
                pi[i] = (pixintensity[i] / sum);
            }
            for (i = 0; i < 256; i++)
            {
                mg += i * pi[i];
            }
            for (i = 0; i < 256; i++)
            {
                for (j = 0; j <= i; j++)
                {
                    temp += pi[j];
                }
                p1[i] = temp;
                p2[i] = (1.0D - temp);
                temp = 0.0D;
            }
            temp = 0.0D;
            for (i = 0; i < 256; i++)
            {
                for (j = 0; j <= i; j++)
                {
                    temp += j * pi[j];
                }
                m1[i] = (temp / p1[i]);

                temp = 0.0D;
                for (j = 255; j > i; j--)
                {
                    temp += j * pi[j];
                }
                m2[i] = (temp / p2[i]);
                temp = 0.0D;
            }
            for (i = 0; i < 256; i++)
            {
                bcv[i] = (p1[i] * Math.Pow(m1[i] - mg, 2.0D) + p2[i] * Math.Pow(m2[i] - mg, 2.0D));
            }
            for (i = 1; i < 256; i++)
            {
                if (bcv[i] == bcv[i])
                {
                    temp = bcv[i];
                    temp2 = i;
                    break;
                }
            }
            for (i = temp2; i < 256; i++)
            {
                if (bcv[i] == bcv[i])
                {
                    if (bcv[i] > temp)
                    {
                        temp = bcv[i];
                        temp2 = i;
                    }
                }
            }
            for (j = 0; j < height; j++)
            {
                for (i = 0; i < width; i++)
                {
                    if (temp_pixel[j, i] > temp2)
                    {
                        temp_pixel[j, i] = 'ÿ';
                    }
                    if (temp_pixel[j, i] <= temp2)
                    {
                        temp_pixel[j, i] = 0;
                    }
                }
            }
            return temp_pixel;
        }

        public int[,] OtsuBinarization(int[,] realImagePixels)
        {
            int[] rk = new int[L_MAX];
            int height = g_pixel.GetLength(0);
            int width = g_pixel.GetLength(1);

            for (int j = 0; j < height; j++)
            {
                for (int i = 0; i < width; i++)
                {
                    rk[g_pixel[j, i]]++;
                }
            }

            int totalNk = findTotalNk(rk);
            float[] pk = findPk(rk, totalNk);
            float[] kpk = findKpk(pk);
            float[] p1k = findP1k(pk);
            float[] p2k = findP2k(p1k);
            float[] m1 = findM1(kpk, p1k);
            float[] m2 = findM2(kpk, p2k);
            float mg = findMg(kpk);
            float[] varianceAb = findVarianceAb(p1k, p2k, m1, m2, mg);
            int threshold = compareVariance(varianceAb);

            int y = 0;
            int[,] newPixels = new int[height, width];
            for (int j = 0; j < height; j++)
            {
                for (int i = 0; i < width; i++)
                {
                    y = realImagePixels[j,i];
                    if (y <= threshold)
                    {
                        newPixels[j, i] = 0;
                    }
                    else
                    {
                        newPixels[j, i] = 255;
                    }
                }
            }
            return newPixels;
        }

        public int[,] SpecialOtsuBinarization(int[,] realImagePixels)
        {
            int[] rk = new int[L_MAX];
            int height = g_pixel.GetLength(0);
            int width = g_pixel.GetLength(1);

            for (int j = 0; j < height; j++)
            {
                for (int i = 0; i < width; i++)
                {
                    rk[g_pixel[j, i]]++;
                }
            }

            int totalNk = findTotalNk(rk);
            float[] pk = findPk(rk, totalNk);
            float[] kpk = findKpk(pk);
            float[] p1k = findP1k(pk);
            float[] p2k = findP2k(p1k);
            float[] m1 = findM1(kpk, p1k);
            float[] m2 = findM2(kpk, p2k);
            float mg = findMg(kpk);
            float[] varianceAb = findVarianceAb(p1k, p2k, m1, m2, mg);
            this.threshold = compareVariance(varianceAb);

            int y = 0;
            int[,] newPixels = new int[height, width];
            for (int j = 0; j < height; j++)
            {
                for (int i = 0; i < width; i++)
                {
                    y = realImagePixels[j, i];
                    if (y <= threshold)
                    {
                        newPixels[j, i] = y;
                    }
                    else
                    {
                        newPixels[j, i] = 0;
                    }
                }
            }
            return newPixels;
        }

        public int GetThreshold()
        {
            return threshold;
        }

        public int[,] SpecialWhiteOtsuBinarization(int[,] realImagePixels)
        {
            int[] rk = new int[L_MAX];
            int height = g_pixel.GetLength(0);
            int width = g_pixel.GetLength(1);

            for (int j = 0; j < height; j++)
            {
                for (int i = 0; i < width; i++)
                {
                    rk[g_pixel[j, i]]++;
                }
            }

            int totalNk = findTotalNk(rk);
            float[] pk = findPk(rk, totalNk);
            float[] kpk = findKpk(pk);
            float[] p1k = findP1k(pk);
            float[] p2k = findP2k(p1k);
            float[] m1 = findM1(kpk, p1k);
            float[] m2 = findM2(kpk, p2k);
            float mg = findMg(kpk);
            float[] varianceAb = findVarianceAb(p1k, p2k, m1, m2, mg);
            int threshold = compareVariance(varianceAb);

            int y = 0;
            int[,] newPixels = new int[height, width];
            for (int j = 0; j < height; j++)
            {
                for (int i = 0; i < width; i++)
                {
                    y = realImagePixels[j, i];
                    if (y <= threshold)
                    {
                        newPixels[j, i] = y;
                    }
                    else
                    {
                        newPixels[j, i] = 255;
                    }
                }
            }
            return newPixels;
        }

        public int[,] SpecialWhiteOtsuBinarization(int[,] realImagePixels, int[,] whiteRealImagePixels)
        {
            int[] rk = new int[L_MAX];
            int height = g_pixel.GetLength(0);
            int width = g_pixel.GetLength(1);

            for (int j = 0; j < height; j++)
            {
                for (int i = 0; i < width; i++)
                {
                    rk[g_pixel[j, i]]++;
                }
            }

            int totalNk = findTotalNk(rk);
            float[] pk = findPk(rk, totalNk);
            float[] kpk = findKpk(pk);
            float[] p1k = findP1k(pk);
            float[] p2k = findP2k(p1k);
            float[] m1 = findM1(kpk, p1k);
            float[] m2 = findM2(kpk, p2k);
            float mg = findMg(kpk);
            float[] varianceAb = findVarianceAb(p1k, p2k, m1, m2, mg);
            int threshold = compareVariance(varianceAb);

            int y = 0;
            int[,] newPixels = new int[height, width];
            for (int j = 0; j < height; j++)
            {
                for (int i = 0; i < width; i++)
                {
                    y = realImagePixels[j, i];
                    if (y <= threshold)
                    {
                        if (y == 0)
                        {
                            newPixels[j, i] = whiteRealImagePixels[j, i];
                        }
                        else
                        {
                            newPixels[j, i] = y;
                        }
                    }
                    else
                    {
                        newPixels[j, i] = 255;
                    }
                }
            }
            return newPixels;
        }

        public int[,] SpecialWhiteOtsuBinarization(int[,] inputImagePixels, int[,] realImagePixels, int oldThreshold)
        {
            int[] rk = new int[L_MAX];
            int height = g_pixel.GetLength(0);
            int width = g_pixel.GetLength(1);

            for (int j = 0; j < height; j++)
            {
                for (int i = 0; i < width; i++)
                {
                    rk[g_pixel[j, i]]++;
                }
            }

            int totalNk = findTotalNk(rk);
            float[] pk = findPk(rk, totalNk);
            float[] kpk = findKpk(pk);
            float[] p1k = findP1k(pk);
            float[] p2k = findP2k(p1k);
            float[] m1 = findM1(kpk, p1k);
            float[] m2 = findM2(kpk, p2k);
            float mg = findMg(kpk);
            float[] varianceAb = findVarianceAb(p1k, p2k, m1, m2, mg);
            int threshold = compareVariance(varianceAb);

            int y = 0;
            int[,] newPixels = new int[height, width];
            for (int j = 0; j < height; j++)
            {
                for (int i = 0; i < width; i++)
                {
                    y = realImagePixels[j, i];
                    if (y <= threshold)
                    {
                        if (inputImagePixels[j, i] <= oldThreshold)
                        {
                            newPixels[j, i] = y;
                        }
                        else
                        {
                            newPixels[j, i] = 255;
                        }
                    }
                    else
                    {
                        newPixels[j, i] = 255;
                    }
                }
            }
            return newPixels;
        }

        public int[,] CheckOtsu(int[,] binaryPixels)
        {
            int height = binaryPixels.GetLength(0);
            int width = binaryPixels.GetLength(1);

            int i, j;

            int black = 0;
            int white = 0;

            for (j = 0; j < height; j++)
            {
                for (i = 0; i < width; i++)
                {
                    if (binaryPixels[j, i] == 0)
                    {
                        black++;
                    }
                    else
                    {
                        white++;
                    }
                }
                if (white > black)
                {
                    binaryPixels = Complement(binaryPixels);
                }
            }

            return binaryPixels;
        }

        public int[,] Complement (int[,] binaryPixels)
        {
	        int i, j;

            int height = binaryPixels.GetLength(0);
            int width = binaryPixels.GetLength(1);
            int[,] negoutput = new int[height, width];
	
	        for (j = 0; j < height; j++) {
		        for (i = 0; i < width; i++) {
			        negoutput[j,i] = (int) (L_MAX - 1 - binaryPixels[j,i]);
		        }
	        }

            return negoutput;
        }

        private int findTotalNk(int[] rk)
        {
            int res = 0;

            for (int i = 0; i < rk.Length; i++)
            {
                res += rk[i];
            }

            return res;
        }

        private static float[] findPk(int[] rk, int totalNk)
        {
            float[] pk = new float[rk.Length];

            for (int i = 0; i < rk.Length; i++)
            {
                pk[i] = (float)rk[i] / totalNk;
            }

            return pk;
        }

        private static float[] findKpk(float[] pk)
        {
            float[] kpk = new float[pk.Length];

            for (int i = 0; i < pk.Length; i++)
            {
                kpk[i] = (float)i * pk[i];
            }

            return kpk;
        }

        private static float[] findP1k(float[] pk)
        {
            float[] p1k = new float[pk.Length];

            for (int i = 0; i < pk.Length; i++)
            {
                for (int j = 0; j <= i; j++)
                {
                    p1k[i] += pk[j];
                }
            }

            return p1k;
        }

        private static float[] findP2k(float[] p1k)
        {
            float[] p2k = new float[p1k.Length];

            for (int i = 0; i < p1k.Length; i++)
            {
                p2k[i] = 1 - p1k[i];
            }

            return p2k;
        }

        private static float[] findM1(float[] kpk, float[] p1k)
        {
            float[] m1 = new float[kpk.Length];

            for (int i = 0; i < kpk.Length; i++)
            {
                float temp = 0;
                for (int j = 0; j <= i; j++)
                {
                    temp += kpk[j];
                }
                m1[i] = temp / p1k[i];
            }

            return m1;
        }

        private static float[] findM2(float[] kpk, float[] p2k)
        {
            float[] m2 = new float[kpk.Length];

            for (int i = 0; i < kpk.Length; i++)
            {
                float temp = 0;
                for (int j = i + 1; j < kpk.Length; j++)
                {
                    temp += kpk[j];
                }
                m2[i] = temp / p2k[i];
            }

            return m2;
        }

        private static float findMg(float[] kpk)
        {
            float mg = 0;

            for (int i = 0; i < kpk.Length; i++)
            {
                mg += kpk[i];
            }

            return mg;
        }

        private static float[] findVarianceAb(float[] p1k, float[] p2k, float[] m1, float[] m2, float mg)
        {
            float[] varianceAb = new float[p1k.Length];

            for (int i = 0; i < p1k.Length; i++)
            {
                varianceAb[i] = (float)(p1k[i] * (Math.Pow((m1[i] - mg), 2)) + p2k[i] * (Math.Pow((m2[i] - mg), 2)));
            }

            return varianceAb;

        }

        private static int compareVariance(float[] varianceAb)
        {
            int threshold = 0;
            float temp = 0;
            for (int i = 0; i < 256; i++)
            {
                if (temp < varianceAb[i])
                {
                    temp = varianceAb[i];
                    threshold = i;
                }
            }

            return threshold;
        }
    }
}
