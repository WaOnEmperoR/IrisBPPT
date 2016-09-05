using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BPPT_Iris_SDK
{
    public class ExtraProcessing
    {
        public static float[,] Scaling(float[,] pixels, int height, int width)
        {
            float max = 0;
            float min = 0;

            float[,] temp = new float[height, width];

            for (int j = 0; j < height; j++)
            {
                for (int i = 0; i < width; i++)
                {
                    if (pixels[j, i] > max)
                        max = pixels[j, i];
                    if (pixels[j, i] < min)
                        min = pixels[j, i];
                }
            }

            float max_fm = 0;
            for (int j = 0; j < height; j++)
            {
                for (int i = 0; i < width; i++)
                {
                    temp[j, i] = pixels[j, i] - min;
                    if (temp[j, i] > max_fm)
                        max_fm = temp[j, i];
                }
            }

            float[,] newPixels = new float[height, width];

            for (int j = 0; j < height; j++)
            {
                for (int i = 0; i < width; i++)
                {
                    temp[j, i] = 255 * (temp[j, i] / max_fm);
                    newPixels[j, i] = temp[j, i];
                }
            }

            return newPixels;
        }

        public static int[,] Scaling(int[,] pixels, int height, int width)
        {
            float max = 0;
            float min = 0;

            float[,] temp = new float[height, width];

            for (int j = 0; j < height; j++)
            {
                for (int i = 0; i < width; i++)
                {
                    if (pixels[j, i] > max)
                        max = pixels[j, i];
                    if (pixels[j, i] < min)
                        min = pixels[j, i];
                }
            }

            float max_fm = 0;
            for (int j = 0; j < height; j++)
            {
                for (int i = 0; i < width; i++)
                {
                    temp[j, i] = pixels[j, i] - min;
                    if (temp[j, i] > max_fm)
                        max_fm = temp[j, i];
                }
            }

            int[,] newPixels = new int[height, width];

            for (int j = 0; j < height; j++)
            {
                for (int i = 0; i < width; i++)
                {
                    temp[j, i] = 255 * (temp[j, i] / max_fm);
                    newPixels[j, i] = (int)Math.Round(temp[j, i]);
                }
            }

            return newPixels;
        }
    }
}
