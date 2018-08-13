using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BPPT_Iris_SDK
{
    public class ConditionalDilation
    {
        public static double distanceCenter(int j, int i, int center_j, int center_i)
        {
            return Math.Sqrt(((j - center_j) * (j - center_j)) + ((i - center_i) * (i - center_i)));
        }

        public static double gradMag(int[,] pix2, int j, int i)
        {
            double sobel1 = (1 * pix2[j - 1, i - 1] + 2 * pix2[j - 1, i] + 1 * pix2[j - 1, i + 1] + 0 * pix2[j, i - 1] + 0 * pix2[j, i] + 0 * pix2[j, i + 1] - 1 * pix2[j + 1, i - 1] - 2 * pix2[j + 1, i] - 1 * pix2[j + 1, i + 1]) / 1;
            double sobel2 = (-1 * pix2[j - 1, i - 1] + 0 * pix2[j - 1, i] + pix2[j - 1, i + 1] + -2 * pix2[j, i - 1] + 0 * pix2[j, i] + 2 * pix2[j, i + 1] - 1 * pix2[j + 1, i - 1] + 0 * pix2[j + 1, i] + pix2[j + 1, i + 1]) / 1;
            //System.out.println(Math.atan(sobel1/sobel2));
            return Math.Sqrt((sobel1 * sobel1) + (sobel2 * sobel2));
        }

        public static void dilationContour(int[,] temp_pixel, int[,] pixel2, int iteration, int width_center, int height_center, double threshold, String type, string folderResult, int[,] pixel_analysis)
        {
            PGM_Iris pgmWriter;
            PGM_Iris g_Pgm = new PGM_Iris(folderResult + "\\ori_contrast_stretching.pgm");

            int g_ImageHeight = temp_pixel.GetLength(0);
            int g_ImageWidth = temp_pixel.GetLength(1);

            int[,] temp_pixel2 = new int[g_ImageHeight, g_ImageWidth];
            int[,] temp_pixel3 = new int[g_ImageHeight, g_ImageWidth];

            int[,] temp_pixel4 = g_Pgm.Pixels;
            
            int min_pos_x = 0;
            int max_pos_x = 0;
            int min_pos_y = 0;
            int max_pos_y = 0;

            //Console.WriteLine(type);
            
            int lim1 = 0;
            int lim2 = 0;
            if (string.Equals(type, "iris", StringComparison.OrdinalIgnoreCase))
            {
                temp_pixel2[height_center, width_center] = 128;
                threshold = 45;
                lim1 = 0;
                lim2 = 100;
            }
            else if (string.Equals(type, "pupil", StringComparison.OrdinalIgnoreCase))
            {
                temp_pixel2[height_center, width_center] = 128;
                threshold = 20;
                lim1 = 0;
                lim2 = 50;
            }

            int i_loop = height_center;
            int j_loop = width_center;
            int loop_counter = 0;

            while (gradMag(temp_pixel, i_loop, j_loop) > threshold)
            {
                if ((loop_counter & 1) == 0)
                    i_loop++;
                else
                    j_loop++;

                loop_counter++;
            }

            height_center = i_loop;
            width_center = j_loop;

            for (int i = 10; i < g_ImageHeight - 10; i++)
            {
                for (int j = 10; j < g_ImageWidth - 10; j++)
                {
                    if (temp_pixel2[i, j] == 128)
                    {
                        //Console.WriteLine(i + " - " + j);
                        //Console.WriteLine(gradMag(temp_pixel, i, j));

                        for (int k = i - 1; k <= i + 1; k++)
                        {
                            for (int l = j - 1; l <= j + 1; l++)
                            {
                                //Console.Write(temp_pixel[k, l] + " ");
                            }
                            //Console.WriteLine();
                        }
                    }
                }
            }

            for (int l = 0; l < iteration; l++)
            {
                for (int i = 10; i < g_ImageHeight - 10; i++)
                {
                    for (int j = 10; j < g_ImageWidth - 10; j++)
                    {

                        if (temp_pixel2[i, j] == 128 && gradMag(temp_pixel, i, j) <= threshold && (distanceCenter(i, j, height_center, width_center) >= lim1 && distanceCenter(i, j, height_center, width_center) < lim2))
                        {
                            //if (temp_pixel2[i][j] == 128 && gradMag(temp_pixel, i, j) <= threshold) {
                            temp_pixel3[i - 1, j - 1] = 128;
                            temp_pixel3[i - 1, j] = 128;
                            temp_pixel3[i - 1, j + 1] = 128;
                            temp_pixel3[i, j - 1] = 128;
                            temp_pixel3[i, j] = 128;
                            temp_pixel3[i, j + 1] = 128;
                            temp_pixel3[i + 1, j - 1] = 128;
                            temp_pixel3[i + 1, j] = 128;
                            temp_pixel3[i + 1, j + 1] = 128;

                            if (string.Equals(type, "pupil", StringComparison.OrdinalIgnoreCase))
                            {
                                if (min_pos_x == 0 || min_pos_x > j)
                                {
                                    min_pos_x = j;
                                }

                                if (max_pos_x < j)
                                {
                                    max_pos_x = j;
                                }

                                if (min_pos_y == 0 || min_pos_y > i)
                                {
                                    min_pos_y = i;
                                }

                                if (max_pos_y < i)
                                {
                                    max_pos_y = i;
                                }
                            }
                        }
                    }
                }
                for (int i = 0; i < g_ImageHeight; i++)
                {
                    for (int j = 0; j < g_ImageWidth; j++)
                    {
                        temp_pixel2[i, j] = temp_pixel3[i, j];
                        pixel_analysis[i, j] = temp_pixel3[i, j];
                    }
                }

                if (string.Equals(type, "pupil", StringComparison.OrdinalIgnoreCase))
                {
                    if ((l + 1) >= iteration)
                    {
                        int pupil_diameter = max_pos_y - min_pos_y;
                        int g_PosWidth = (int)(min_pos_x + max_pos_x) / 2;
                        int g_PosHeight = (int)((min_pos_y + max_pos_y) / 2);
                        int g_PosWidth2 = (int)(min_pos_x + max_pos_x) / 2;
                        int g_PosHeight2 = (int)((min_pos_y + max_pos_y) / 2) + (pupil_diameter / 2) + 10;

                        bool finish = false;
                        int i = 1;
                        int tmp_x = g_PosWidth2;
                        int tmp_y = g_PosHeight2;

                        if (gradMag(temp_pixel4, g_PosHeight2, g_PosWidth2) > threshold)
                        {
                            while (!finish)
                            {
                                if ((g_PosHeight2 - i) > (min_pos_y + max_pos_y))
                                {
                                    if (gradMag(temp_pixel4, g_PosHeight2 - i, g_PosWidth2) <= threshold)
                                    {
                                        g_PosHeight2 -= i;
                                        finish = true;
                                        break;
                                    }
                                }
                                if (!finish && (g_PosHeight2 + i) < g_ImageHeight)
                                {
                                    if (gradMag(temp_pixel4, g_PosHeight2 + i, g_PosWidth2) <= threshold)
                                    {
                                        g_PosHeight2 += i;
                                        finish = true;
                                        break;
                                    }
                                }
                                if (!finish && (g_PosWidth2 - i) > (min_pos_x + min_pos_y))
                                {
                                    if (gradMag(temp_pixel4, g_PosHeight2, g_PosWidth2 - i) <= threshold)
                                    {
                                        g_PosWidth2 -= i;
                                        finish = true;
                                        break;
                                    }
                                }
                                if (!finish && (g_PosWidth2 + i) < g_ImageWidth)
                                {
                                    if (gradMag(temp_pixel4, g_PosHeight2, g_PosWidth2 + i) <= threshold)
                                    {
                                        g_PosWidth2 += i;
                                        finish = true;
                                        break;
                                    }
                                }
                                if ((g_PosHeight2 - i) < (min_pos_y + max_pos_y) && (g_PosHeight2 + i) > g_ImageHeight - 10 && (g_PosWidth2 - i) < 10 && (g_PosWidth2 + i) > g_ImageWidth - 10)
                                {
                                    finish = true;
                                }

                                i++;
                            }
                        }
                    }
                }

                
            }
            pgmWriter = new PGM_Iris(g_ImageWidth, g_ImageHeight);

            pgmWriter.WriteToPath(folderResult + "\\anim_black_" + type + ".pgm", temp_pixel2);

            for (int i = 0; i < g_ImageHeight; i++)
            {
                for (int j = 0; j < g_ImageWidth; j++)
                {
                    if (temp_pixel2[i, j] == 128)
                    {
                        temp_pixel[i, j] = 128;
                    }
                }
            }


        }

    }
}
