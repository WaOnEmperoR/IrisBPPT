using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BiometriBPPT.bppt.ptik.biometric.entity;

namespace BiometriBPPT.bppt.ptik.biometric.image
{
    public class HoughTransform
    {
        public void hough_circle(MyImage ptr_binary_image, Iris.pupil_info ptr_pupil_info)
        {
            int height = ptr_binary_image.Height;
            int width = ptr_binary_image.Width;

            int radius_limit_min = 15;
            int radius_limit_max = 50;
            int c_theta_limit = 360;

            int i, j, k;
            int t, r;

            double[,] r_sin_array = new double[radius_limit_max - radius_limit_min, c_theta_limit];
            double[,] r_cos_array = new double[radius_limit_max - radius_limit_min, c_theta_limit];

            int center_a = 0;
            int center_b = 0;

            for (r = radius_limit_min; r < radius_limit_max; r++)
            {
                for (t = 0; t < c_theta_limit; t++)
                {
                    r_sin_array[r - radius_limit_min, t] = r * Math.Sin(t * Math.PI / 180);
                    r_cos_array[r - radius_limit_min, t] = r * Math.Cos(t * Math.PI / 180);
                }
            }

            int[, ,] hough_circle_accu = new int[height, width, radius_limit_max - radius_limit_min];
            /*	
                int height_start = height/2 - 55;
                int height_stop = height/2 + 55;
	
                int width_start = width/2 - 55;
                int width_stop = width/2 + 55;
            */

            int height_start = 0;
            int height_stop = height;

            int width_start = 0;
            int width_stop = width;

            for (j = 0; j < height; j++)
                for (i = 0; i < width; i++)
                    for (k = 0; k < (radius_limit_max - radius_limit_min); k++)
                        hough_circle_accu[j, i, k] = 0;

            for (j = height_start; j < height_stop; j++)
            {
                for (i = width_start; i < width_stop; i++)
                {
                    if (ptr_binary_image.UPixel[j, i] == Iris.HIGH)
                    {
                        for (r = radius_limit_min; r < radius_limit_max; r++)
                        {
                            for (t = 0; t < c_theta_limit; t++)
                            {
                                center_a = (int)(i - r_cos_array[r - radius_limit_min, t]);
                                center_b = (int)(j - r_sin_array[r - radius_limit_min, t]);
                                //printf("a,b = %d %d\n", center_a, center_b);
                                if (center_a >= 0 && center_a < height && center_b >= 0 && center_b < height)
                                {
                                    hough_circle_accu[center_b, center_a, r - radius_limit_min]++;
                                }
                            }
                        }
                    }
                }
            }

            int maxr = 0;
            int maxa = 0;
            int maxb = 0;
            int max = 0;
            for (j = 0; j < height; j++)
            {
                for (i = 0; i < width; i++)
                {
                    for (r = 0; r < (radius_limit_max - radius_limit_min); r++)
                    {
                        if (hough_circle_accu[j, i, r] > max)
                        {
                            max = hough_circle_accu[j, i, r];
                            maxr = r + radius_limit_min;
                            maxa = i;
                            maxb = j;
                        }
                    }
                }
            }
            //printf("PUPIL TRIPLETS: %d %d %d\n", maxr, maxa, maxb);

            ptr_pupil_info.A = maxa;
            ptr_pupil_info.B = maxb;
            ptr_pupil_info.Radius = maxr;

            //    myfree2((void**)r_sin_array, radius_limit_max - radius_limit_min);
            //    myfree2((void**)r_cos_array, radius_limit_max - radius_limit_min);
            //    myfree3((void***)hough_circle_accu, height, width);

        }
    }
}
