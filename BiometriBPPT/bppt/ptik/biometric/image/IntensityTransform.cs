using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BiometriBPPT.bppt.ptik.biometric.entity;

namespace BiometriBPPT.bppt.ptik.biometric.image
{
    public class IntensityTransform
    {

        public void scaling(float[,] array, int height, int width)
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

        public void complement(MyImage ptr_my_image)
        {
            int i, j;
            int height = ptr_my_image.Height;
            int width = ptr_my_image.Width;

            byte[,] negoutput = new byte[height, width];

            for (j = 0; j < height; j++)
            {
                for (i = 0; i < width; i++)
                {
                    negoutput[j, i] = (byte)((Const.L_MAX - 1) - ptr_my_image.UPixel[j, i]);
                }
            }

            for (j = 0; j < height; j++)
            {
                for (i = 0; i < width; i++)
                {
                    ptr_my_image.UPixel[j, i] = negoutput[j, i];
                }
            }
            //printf("complement\n");
        }
    }
}
