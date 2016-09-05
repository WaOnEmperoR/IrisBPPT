using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BiometriBPPT.bppt.ptik.biometric.entity;
using BiometriBPPT.bppt.ptik.biometric.utility;
using System.Diagnostics;

namespace BiometriBPPT.bppt.ptik.biometric.image
{
    public class SpatialFiltering
    {
        DebugImage debugImage;

        public SpatialFiltering(DebugImage debI)
        {
            debugImage = debI;
        }

        public void gaussian_filter(MyImage ptr_my_image, MyImage ptr_result_image, float gauss_radius, float sigma)
        {
            int height = ptr_my_image.Height;
            int width = ptr_my_image.Width;

            int r = (int)gauss_radius;
            int rows = r * 2 + 1;

            float[,] matrix2D = new float[rows, rows];

            gaussian2D(matrix2D, gauss_radius, sigma);

            debugImage.DebugImg_float2dToTxt("matrix2D.txt", matrix2D);

            int i, j, y, x;

            float sum = 0;
            for (j = 0; j < r; j++)
                for (i = 0; i < r; i++)
                    sum += matrix2D[j, i];
            sum = MathBppt.toFloat(sum);
            Debug.WriteLine("sum gaussian " + sum);

            float[,] new_pixel = new float[height, width];
            for (j = 0; j < height; j++)
                for (i = 0; i < width; i++)
                    new_pixel[j, i] = 0;

            for (j = r; j < height - r; j++)
            {
                for (i = r; i < width - r; i++)
                {
                    //			float new_pixel = 0;
                    for (y = -r; y <= r; y++)
                    {
                        for (x = -r; x <= r; x++)
                        {
                            new_pixel[j, i] += matrix2D[y + r, x + r] * ptr_my_image.UPixel[j + y, i + x];
                        }
                    }
                    //			ptr_result_image.UPixel[j,i] = new_pixel;
                }
            }
            
            IntensityTransform it = new IntensityTransform();
            it.scaling(new_pixel, height, width);

            debugImage.DebugImg_float2dToTxt("new_pixel.txt", new_pixel);

            for (j = 0; j < height; j++)
            {
                for (i = 0; i < width; i++)
                {
                    ptr_result_image.UPixel[j, i] = 0;
                }
            }

            for (j = r; j < height - r; j++)
            {
                for (i = r; i < width - r; i++)
                {
                    ptr_result_image.UPixel[j, i] = (byte)new_pixel[j, i];
                }
            }
            debugImage.DebugImg_ImgToTxt("ptr_result_image.txt", ptr_result_image);
            //	myfree2((void**)matrix2D, rows);
            //myfree2((void**)new_pixel, height);
        }

        private void gaussian2D(float[,] matrix, float radius, float sigma)
        {
            int r = (int)radius;
            int rows = r * 2 + 1;
            if (sigma == 0)
                sigma = radius / 3;

            int col, row;
            float total = 0;

            for (col = -r; col <= r; col++)
            {
                for (row = -r; row <= r; row++)
                {
                    float distance_row = row * row;
                    float distance_col = col * col;
                    if ((row * col) > Math.Pow(radius, 2))//if (distance_row > Math.Pow(radius, 2) || distance_col > Math.Pow(radius, 2))//if (distance > pow(radius, 2))
                        matrix[col + r, row + r] = 0;
                    else
                        matrix[col + r, row + r] = (float)(Math.Exp(-((distance_row) / (2 * sigma * sigma) + (distance_col) / (2 * sigma * sigma))) / Math.Sqrt(2 * Math.PI * sigma));

                    total += matrix[col + r, row + r];
                }
            }

            int i, j;
            //	printf("gaussian2D\n");
            for (j = 0; j < rows; j++)
            {
                for (i = 0; i < rows; i++)
                {
                    matrix[j, i] /= total;
                    //matrix[j, i] = MathBppt.toFloat(matrix[j, i]);
                }
            }
        }

        public void gaussian2Delliptical(float[,] matrix, float radius, float sigmax, float sigmay, int rotation)
        {
            int r = (int)radius;
            int rows = r * 2 + 1;
            //if (sigma == 0)
            //	sigma = radius/3;

            int col, row;
            float total = 0;

            //float sigmax = 3;
            //float sigmay = 2;
            for (col = -r; col <= r; col++)
            {
                for (row = -r; row <= r; row++)
                {
                    float distance_row = row * row;
                    float distance_col = col * col;
                    float xg = (float)(Math.Abs(row) * Math.Cos(rotation * Iris.PI / 180) - Math.Abs(col) * Math.Sin(rotation * Iris.PI / 180));
                    float yg = (float)(Math.Abs(row) * Math.Sin(rotation * Iris.PI / 180) + Math.Abs(col) * Math.Cos(rotation * Iris.PI / 180));

                    if ((row * col) > Math.Pow(radius, 2))//if (distance_row > Math.Pow(radius, 2) || distance_col > Math.Pow(radius, 2))//if (distance > pow(radius, 2))
                        matrix[col + r, row + r] = 0;
                    else
                        matrix[col + r, row + r] = (float)(Math.Exp(-((xg * xg) / (sigmax * sigmax) + (yg * yg) / (sigmay * sigmay))) / (2 * Iris.PI * sigmax * sigmay));

                    total += matrix[col + r, row + r];
                }
            }

            int i, j;
            //printf("gaussian2Delliptical\n");
            for (j = 0; j < rows; j++)
            {
                for (i = 0; i < rows; i++)
                {
                    matrix[j, i] /= total;
                    //printf("%f ", matrix[j][i]);
                }
                //printf("\n");
            }
            //printf("\n");
        }
    }
}
