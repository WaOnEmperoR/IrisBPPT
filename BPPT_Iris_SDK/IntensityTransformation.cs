using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BPPT_Iris_SDK
{
    public class IntensityTransformation
    {
        public static void scaling(float[,] array, int height, int width)
        {
            int i, j;

            float max = 0;
            float min = 0;

            float[,] temp = new float[height, width];

            for (j = 0; j < height; j++)
            {
                for (i = 0; i < width; i++)
                {
                    if (array[j, i] > max)
                        max = array[j, i];
                    if (array[j, i] < min)
                        min = array[j, i];
                }
            }

            float max_fm = 0;
            for (j = 0; j < height; j++)
            {
                for (i = 0; i < width; i++)
                {
                    temp[j, i] = array[j, i] - min;
                    if (temp[j, i] > max_fm)
                        max_fm = temp[j, i];
                }
            }

            for (j = 0; j < height; j++)
            {
                for (i = 0; i < width; i++)
                {
                    temp[j, i] = 255 * (temp[j, i] / max_fm);
                    array[j, i] = (byte)temp[j, i];
                }
            }
        }

        public static int[,] ContrastStretching(int[,] pixels, int threshold) 
        {
	        int[] my_histo = new int[256];
	        int[] my_histo_cs = new int[256];
	
	        my_histo = histogram(pixels);

	        int width = pixels.GetLength(1);
	        int height = pixels.GetLength(0);
	
            //	int threshold = 50;
	        int min_intensity = 0;
	        int max_intensity = 0;
	
	        int i, j;
	
	        for (i = 1; i < 256; i++) {
		        if (my_histo[i] > threshold) 
			        max_intensity = i;
	        }

	        for (i = 256 - 1; i >= 1; i--) {
		        if (my_histo[i] > threshold)
			        min_intensity = i;
	        }
	        //printf("min_intensity %d\n", min_intensity);
	
	        int[,] my_pixel = new int[height, width];
	
	        int max_cs_intensity = 0;
	        int min_cs_intensity = 0;
	        for (j = 0; j < height; j++) {
		        for (i = 0; i < width; i++) {
                    //my_pixel[j,i] = (int) (pixels[j,i] - min_intensity) * (255 / (max_intensity - min_intensity));
                    int pembilang = (pixels[j, i] - min_intensity) * 255;
                    int penyebut = max_intensity - min_intensity;
                    float hasil = (float)pembilang / (float)penyebut;
                    my_pixel[j, i] = (int)hasil;

                    if (my_pixel[j, i] >= max_cs_intensity)
                    {
                        max_cs_intensity = my_pixel[j,i];
                    }
                    if (my_pixel[j, i] <= min_cs_intensity)
                    {
                        min_cs_intensity = my_pixel[j,i];
                    }
		        }
	        }
	
	        //my_pixel = ExtraProcessing.Scaling(my_pixel, height, width);

            return my_pixel;
	        
        }

        public static int[] histogram(int[,] pixels)
        {
            //  int histo[L_MAX];
            int[] histo = new int[256];
            int i, j;

            for (i = 0; i < 256; i++)
                histo[i] = 0;

            for (j = 0; j < pixels.GetLength(0); j++)
            {
                for (i = 0; i < pixels.GetLength(1); i++)
                {
                    histo[pixels[j, i]]++;
                }
            }

            return histo;
        }

        public static int[,] DiagonalSharpening(int[,] pixels)
        {
            int w = pixels.GetLength(1);
            int h = pixels.GetLength(0);
            
            int width = w + 2;
            int height = h + 2;
            int[,] paddedPixels = new int[height, width];
            int[,] tmpPixels = new int[h, w];
            int[,] newPixels = new int[h, w];

            int posY = 0;
            int posX = 0;
            int z = 0;
            for (int j = 0; j < height; j++)
            {
                for (int i = 0; i < width; i++)
                {
                    if (i == 0 || j == 0 || i == width - 1 || j == height - 1)
                    {
                        z = 0;
                    }
                    else
                    {
                        z = pixels[posY, posX];
                        if (i == width - 2)
                        {
                            posY++;
                            posX = 0;
                        }
                        else
                        {
                            posX++;
                        }
                    }
                    paddedPixels[j, i] = z;
                }
            }
      
            for(int x = 0; x<h; x++){
                for(int y = 0; y<w; y++){
                    int temp = 0;
                    int old = 0;
                    for(int j=x; j<(x+3); j++){
                       for(int i=y; i<(y+3); i++){
                          if(j==(x+1)&&i==(y+1)){
                             temp += (-8*paddedPixels[j,i]);
                             old = paddedPixels[j, i];
                          }
                          else{
                             temp += paddedPixels[j, i];
                          }
                       }
                    }
            
                    tmpPixels[x, y] = old-temp;
                }
            }

            for (int j = 0; j < h; j++)
            {
                for (int i = 0; i < w; i++)
                {
                    if (tmpPixels[j, i] < 0)
                    {
                        newPixels[j, i] = 0;
                    }
                    else if (tmpPixels[j, i] > 255)
                    {
                        newPixels[j, i] = 255;
                    }
                    else
                    {
                        newPixels[j, i] = tmpPixels[j, i];
                    }
                }
            }

            return newPixels;
        }

        public static int[,] PerformHistogramEqualization(int[,] pixels)
        {
            int[] rk = histogram(pixels);
            HistogramEqualization histogramEqualization = new HistogramEqualization(pixels, rk);
            float[] prk = histogramEqualization.countPrk();
            int[] sk = histogramEqualization.countSk(prk);
            int[,] newPixels = histogramEqualization.GenerateNewPixels(sk);

            return newPixels;
        }

        public static int[,] PerformSpecialHistogramEqualization(int[,] pixels)
        {
            int[] rk = histogram(pixels);
            rk[255] = 0;
            HistogramEqualization histogramEqualization = new HistogramEqualization(pixels, rk);
            float[] prk = histogramEqualization.countPrk();
            int[] sk = histogramEqualization.countSk(prk);
            int[,] newPixels = histogramEqualization.GenerateNewPixels(sk);

            return newPixels;
        }

        public static int[,] GammaCorrection(int[,] pixels, float gamma)
        {
            int[,] newPixels = new int[pixels.GetLength(0), pixels.GetLength(1)];

            int x = 0;
            int c = 1;
            for (int j = 0; j < pixels.GetLength(0); j++)
            {
                for (int i = 0; i < pixels.GetLength(1); i++)
                {
                    x = pixels[j,i];
                    double intensity = (double)x / 255;
                    double s = (c * Math.Pow(intensity, gamma));
                    int w = (int)Math.Round(s * 255);
                    newPixels[j, i] = w;
                }
            }

            return newPixels;
        }

        private static void bubbleSort(int[] x, int datanum)
        {
            int i, j;
            int tmp;
            for (i = 0; i < datanum - 1; i++)
                for (j = datanum - 1; j > i; j--)
                {
                    if (x[j - 1] < x[j])
                    {
                        tmp = x[j];
                        x[j] = x[j - 1];
                        x[j - 1] = tmp;
                    }
                }
        }

        /// <summary>
        /// Performs median filter operation on an image (<see cref="BPPTAfis.bppt.ptik.biometri.afis.entity.MyImage"/>)
        /// Source: Digital Image Processing (3rd Edition), Rafael C. Gonzales, Richard E. Woods, Pearson, 2008.
        /// </summary>
        public static int[,] MedianFilter(int[,] pixels)
        {
            int i, j, k, l, x = 0;
            int[] s = new int[9];
            for (j = 1; j < pixels.GetLength(0) - 1; j++)
            {
                for (i = 1; i < pixels.GetLength(1) - 1; i++)
                {
                    x = 0;
                    for (l = j - 1; l < j + 2; l++)
                    {
                        for (k = i - 1; k < i + 2; k++)
                        {
                            try
                            {
                                s[x] = pixels[l, k];
                            }
                            catch (Exception)
                            {
                                s[x] = (byte)255;
                            }
                            x++;
                        }
                    }
                    bubbleSort(s, 9);
                    try
                    {
                        pixels[j, i] = s[4];
                    }
                    catch (Exception)
                    {

                        //throw;
                    }
                }
            }
            return pixels;
        }

        public static int[,] Average3x3(int[,] pixels)
        {
            int i, j;
            int[,] tmp = pixels;

            // averaging with 3x3 mask

            for (j = 1; j < pixels.GetLength(0) - 1; j++)
                for (i = 1; i < pixels.GetLength(1) - 1; i++)
                    pixels[j, i] = ((
                                             tmp[j - 1, i - 1] + tmp[j - 1, i] + tmp[j - 1, i + 1] +
                                             tmp[j, i - 1] + tmp[j, i] + tmp[j, i + 1] +
                                             tmp[j + 1, i - 1] + tmp[j + 1, i] + tmp[j + 1, i + 1]) / 9);

            return pixels;
        }
    }
}
