using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BiometriBPPT.bppt.ptik.biometric.entity;
using BiometriBPPT.bppt.ptik.biometric.utility;
using System.Diagnostics;

namespace BiometriBPPT.bppt.ptik.biometric.image
{
    public class EdgeDetection
    {
        DebugImage debugImage;

        public EdgeDetection(DebugImage debI) {
            debugImage = debI;
        }

        public void canny(MyImage ptr_my_image, MyImage ptr_canny_image, int tH, int tL)
        {//canny(T_IMAGE *ptr_my_image, T_IMAGE *ptr_canny_image, int tH, int tL) {
            int height = ptr_my_image.Height;//int height = ptr_my_image->height;
            int width = ptr_my_image.Width;//int width = ptr_my_image->width;

            int[,] arrmagnitude = new int[height, width];//int **arrmagnitude = (int**)mymalloc2(height, width, sizeof(int));
            float[,] arrtan = new float[height, width];//float **arrtan = (float**)mymalloc2(height, width, sizeof(float)); 

            byte[,] edgepixel_L = new byte[height, width];//unsigned char **edgepixel_L = (unsigned char**)mymalloc2(height, width, sizeof(unsigned char));
            byte[,] edgepixel_H = new byte[height, width];//unsigned char **edgepixel_H = (unsigned char**)mymalloc2(height, width, sizeof(unsigned char));
            float gx, gy;
            int i, j;
            long sum = 0, mean;

            SpatialFiltering sf = new SpatialFiltering(debugImage);
            debugImage.DebugImg_ImgToTxt("gaussian-image-before.txt", ptr_my_image);
            sf.gaussian_filter(ptr_my_image, ptr_my_image, (float)7, (float)2.5);
            //path = build_result_path(result_dir_path, "gaussian-image.pgm");
            //write_output(path, ptr_my_image);
            debugImage.DebugImg_ImgToTxt("gaussian-image.txt", ptr_my_image);
            debugImage.DebugImg_WriteImg("gaussian-image.pgm", ptr_my_image);

            for (j = 0; j < height; j++)
            {
                for (i = 0; i < width; i++)
                {
                    edgepixel_L[j, i] = 0;
                    edgepixel_H[j, i] = 0;
                    arrmagnitude[j, i] = 0;
                    arrtan[j, i] = 0;
                }
            }

            Preprocessing pre = new Preprocessing(debugImage);
            int[] histo = pre.histogram(ptr_my_image);


            for (i = 0; i < 256; i++)
            {
                sum += i * histo[i];
            }

            mean = sum / (height * width);
            //Debug.WriteLine("mean intensity "+mean);

            for (j = 1; j < height - 1; j++)
            {
                for (i = 1; i < width - 1; i++)
                {
                    gx = MathBppt.toFloat((float)ptr_my_image.UPixel[j + 1, i - 1] +
                        2 * (float)ptr_my_image.UPixel[j + 1, i] +
                         (float)ptr_my_image.UPixel[j + 1, i + 1] -
                         (float)ptr_my_image.UPixel[j - 1, i - 1] -
                        2 * (float)ptr_my_image.UPixel[j - 1, i] -
                         (float)ptr_my_image.UPixel[j - 1, i + 1]);

                    gy = MathBppt.toFloat((float)ptr_my_image.UPixel[j - 1, i + 1] +
                        2 * (float)ptr_my_image.UPixel[j, i + 1] +
                         (float)ptr_my_image.UPixel[j + 1, i + 1] -
                         (float)ptr_my_image.UPixel[j - 1, i - 1] -
                        2 * (float)ptr_my_image.UPixel[j, i - 1] -
                         (float)ptr_my_image.UPixel[j + 1, i - 1]);

                    arrmagnitude[j, i] = (int)Math.Sqrt(gx * gx + gy * gy);

                    arrtan[j, i] = MathBppt.toFloat(MathBppt.toFloat((Math.Atan(gx / gy)) * 180 / MathBppt.toFloat(Math.PI)));
                    //printf("%4.2f\t", round(arrtan[j,i]));
                }
            }

            debugImage.DebugImg_int2dToTxt("arrmagnitude.txt", arrmagnitude);
            debugImage.DebugImg_float2dToTxt("arrtan.txt", arrtan);

            for (j = 1; j < height - 1; j++)
            {
                for (i = 1; i < width - 1; i++)
                {
                    if (arrtan[j, i] >= -22.5 && arrtan[j, i] <= 22.5)
                    {
                        if ((arrmagnitude[j, i] > arrmagnitude[j, i + 1]) && (arrmagnitude[j, i] > arrmagnitude[j, i - 1]))
                        {
                            edgepixel_L[j, i] = 255;
                        }
                    }
                    else if ((arrtan[j, i] >= -90 && arrtan[j, i] <= 67.5) || (arrtan[j, i] >= 67.5 && arrtan[j, i] <= 90))
                    {
                        if ((arrmagnitude[j, i] > arrmagnitude[j + 1, i]) && (arrmagnitude[j, i] > arrmagnitude[j - 1, i]))
                        {
                            edgepixel_L[j, i] = 255;
                        }
                    }
                    else if (arrtan[j, i] >= -67.5 && arrtan[j, i] <= -22.5)
                    {
                        if ((arrmagnitude[j, i] > arrmagnitude[j + 1, i + 1]) && (arrmagnitude[j, i] > arrmagnitude[j - 1, i - 1]))
                        {
                            edgepixel_L[j, i] = 255;
                        }
                    }
                    else if (arrtan[j, i] >= 22.5 && arrtan[j, i] <= 67.5)
                    {
                        if ((arrmagnitude[j, i] > arrmagnitude[j + 1, i - 1]) && (arrmagnitude[j, i] > arrmagnitude[j - 1, i + 1]))
                        {
                            edgepixel_L[j, i] = 255;
                        }
                    }
                    //printf("%d\t", edgepixel[j,i]);
                }
            }

            for (j = 0; j < height; j++)
            {
                for (i = 0; i < width; i++)
                {
                    if (arrmagnitude[j, i] >= tL)
                    {
                        edgepixel_L[j, i] = 255;
                    }
                    if (arrmagnitude[j, i] >= tH)
                    {
                        edgepixel_H[j, i] = 255;
                    }
                }
            }

            debugImage.DebugImg_byte2dToTxt("edgepixel_L1.txt", edgepixel_L);

            /*
            for (j = 0; j < height; j++)
            {
                for (i = 0; i < width; i++)
                {
                    if (edgepixel_L[j,i] == 255 && edgepixel_H[j,i] == 255)
                    {
                        //edgepixel_L[j][i] == 0;
                    }
                }
            } 
            */

            debugImage.DebugImg_byte2dToTxt("edgepixel_L2.txt", edgepixel_L);
            debugImage.DebugImg_byte2dToTxt("edgepixel_H.txt", edgepixel_H);

            for (j = 0; j < height; j++)
            {
                for (i = 0; i < width; i++)
                {
                   ptr_canny_image.UPixel[j, i] = edgepixel_L[j, i];
                }
            }
            

            debugImage.DebugImg_ImgToTxt("ptr_canny_image.txt", ptr_canny_image);

            //printf("halo2\n");

            
        }
    }
}
