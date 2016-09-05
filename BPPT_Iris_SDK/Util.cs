using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BPPT_Iris_SDK
{
    public class Util
    {
        public static int HorizontalProjection(int width, int height, int[,] pixel)
        {
            int[] horizontal = new int[width];
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    horizontal[j] += pixel[i, j];
                }
            }
            int smallest = width * 255 + 900;
            int pos = 0;
            for (int i = 50; i <= 200; i++)
            {
                if (horizontal[i] < smallest)
                {
                    smallest = horizontal[i];
                    pos = i;
                }
            }
            return pos;
        }

        public static int[,] HorizontalROI(int width, int height, int[,] pixel, int pos)
        {
            int pos1 = pos - 70;
            int pos2 = pos + 70;
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    if ((j <= pos1) || (j >= pos2))
                    {
                        pixel[i, j] = 0;
                    }
                }
            }

            return pixel;
        }

        public static int VerticalProjection(int width, int height, int[,] pixel)
        {
            int[] vertical = new int[height];
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    vertical[i] += pixel[i, j];
                }
            }
            int smallest = width * 255 + 900;
            int pos = 0;
            for (int i = 60; i <= 200; i++)
            {
                if (vertical[i] < smallest)
                {
                    smallest = vertical[i];
                    pos = i;
                }
            }
            return pos;
        }

        public static void VerticalROI(int width, int height, int[][] pixel, int pos)
        {
            int pos1 = pos - 70;
            int pos2 = pos + 70;
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    if ((i <= pos1) || (i >= pos2))
                    {
                        pixel[i][j] = 0;
                    }
                }
            }
        }

        public static void GenerateImageBlack(int[,] pixel, String filename)
        {
            try
            {
                PGM_Iris pgm = new PGM_Iris(pixel.GetLength(1), pixel.GetLength(0));
                pgm.Pixels = pixel;
                pgm.WriteToPath(filename+".pgm");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void GenerateImageInitiatePoint(int[,] pixel, String filename, int x1, int y1, int x2, int y2)
        {
            int[,] _pixel = (int[,])pixel.Clone();
            try
            {
                for (int y = 0; y < _pixel.GetLength(0); y++)
                {
                    for (int x = 0; x < _pixel.GetLength(1); x++)
                    {
                        if (((y == y1) && (x == x1)) || ((y == y2) && (x == x2)))
                        {
                            _pixel[y, x] = 0xff;
                        }
                    }
                }
                GenerateImageBlack(pixel, filename);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void GenerateImageReal(int[,] pixel1, int[,] pixel2, String filename)
        {
            try
            {
                for (int y = 0; y < pixel1.GetLength(0); y++)
                {
                    for (int x = 0; x < pixel1.GetLength(1); x++)
                    {
                        if (pixel1[y, x] == 128)
                        {
                            pixel2[y, x] = 0x80;
                        }
                    }
                }
                GenerateImageBlack(pixel2, filename);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static double DistanceCenter(int j, int i, int center_j, int center_i)
        {
            return Math.Sqrt((j - center_j) * (j - center_j) + (i - center_i) * (i - center_i));
        }

        public static double GradMag(int[,] pix2, int j, int i)
        {
            double sobel1 = (1 * pix2[(j - 1), (i - 1)] + 2 * pix2[(j - 1), i] + 1 * pix2[(j - 1), (i + 1)] + 0 * pix2[j, (i - 1)] + 0 * pix2[j, i] + 0 * pix2[j, (i + 1)] - 1 * pix2[(j + 1), (i - 1)] - 2 * pix2[(j + 1), i] - 1 * pix2[(j + 1), (i + 1)]) / 1;
            double sobel2 = (-1 * pix2[(j - 1), (i - 1)] + 0 * pix2[(j - 1), i] + pix2[(j - 1), (i + 1)] + -2 * pix2[j, (i - 1)] + 0 * pix2[j, i] + 2 * pix2[j, (i + 1)] - 1 * pix2[(j + 1), (i - 1)] + 0 * pix2[(j + 1), i] + pix2[(j + 1), (i + 1)]) / 1;
            return Math.Sqrt(sobel1 * sobel1 + sobel2 * sobel2);
        }

        public static int[,] CombineImage(int[,] pixel1, int[,] pixel2, int[,] pixel3)
        {
            int[,] temp_pixel2 = new int[pixel1.GetLength(0), pixel1.GetLength(1)];
            for (int j = 0; j < pixel1.GetLength(0); j++)
            {
                for (int i = 0; i < pixel1.GetLength(1); i++)
                {
                    if ((pixel2[j, i] == 0) || (pixel3[j, i] == 0))
                    {
                        temp_pixel2[j, i] = 'ÿ';
                    }
                    else
                    {
                        temp_pixel2[j, i] = pixel1[j, i];
                    }
                }
            }
            return temp_pixel2;
        }

        public static int[,] CombineImage2(int[,] pixel1, int[,] pixel2, int[,] pixel3)
        {
            int[,] temp_pixel2 = new int[pixel1.GetLength(0), pixel1.GetLength(1)];
            for (int j = 0; j < pixel1.GetLength(0); j++)
            {
                for (int i = 0; i < pixel1.GetLength(1); i++)
                {
                    //if (pixel2[j, i] == pixel3[j, i] && pixel2[j, i] == 128)
                    //{
                    //    temp_pixel2[j, i] = pixel1[j, i];
                    //}
                    //else
                    //{
                        if (pixel2[j, i] == 0)
                        {
                            temp_pixel2[j, i] = 255;
                        }
                        else if (pixel3[j, i] == 128)
                        {
                            temp_pixel2[j, i] = 255;
                        }
                        else
                        {
                            temp_pixel2[j, i] = pixel1[j, i];
                        }
                    //}
                }
            }
            return temp_pixel2;
        }

        public static int[,] CombineImage3(int[,] pixel1, int[,] pixel2)
        {
            int[,] temp_pixel2 = new int[pixel1.GetLength(0), pixel1.GetLength(1)];
            for (int j = 0; j < pixel1.GetLength(0); j++)
            {
                for (int i = 0; i < pixel1.GetLength(1); i++)
                {
                    if (pixel1[j, i] == pixel2[j, i])
                    {
                        temp_pixel2[j, i] = pixel1[j, i];
                    }
                    else
                    {
                        temp_pixel2[j, i] = 0;
                    }
                }
            }
            return temp_pixel2;
        }

        public static byte[,] SingleToMulti(byte[] array, int height, int width)
        {
            int index = 0;
            byte[,] multi = new byte[height, width];
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    multi[y, x] = array[index];
                    index++;
                }
            }
            return multi;
        }

        public static byte[] MultiToSingle(byte[,] array)
        {
            int index = 0;
            int width = array.GetLength(0);
            int height = array.GetLength(1);
            byte[] single = new byte[width * height];
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    single[index] = array[y, x];
                    index++;
                }
            }
            return single;
        }
    }
}
