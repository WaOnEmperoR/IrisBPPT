using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BiometriBPPT.bppt.ptik.biometric.entity;
using BiometriBPPT.bppt.ptik.biometric.utility;
using System.IO;
using BiometriBPPT.bppt.ptik.biometric.image;
using System.Diagnostics;

namespace BiometriBPPT.bppt.ptik.biometric.iris
{
    public class IrisProcessing
    {
        DebugImage debugImage;

        public IrisProcessing(DebugImage debI)
        {
            debugImage = debI;
        }
        
        public void find_pupillary_limbus_boundary(MyImage ptr_my_image, MyImage ptr_roi_image, MyImage ptr_ori_image, MyImage ptr_ori_roi_image,
                                                                Iris.pupil_info ptr_pupil_info, Iris.iris_boundary ptr_iris_boundary)
        {
            int j, i;

            int height = ptr_my_image.Height;
            int width = ptr_my_image.Width;

            int roi_height = ptr_roi_image.Height;
            int roi_width = ptr_roi_image.Width;

            int radius_pupil = ptr_pupil_info.Radius;

            int sum = 0;

            int startx = ptr_pupil_info.A - radius_pupil - 50;
            int stopx = ptr_pupil_info.A + radius_pupil + 50;

            int starty = ptr_pupil_info.B - radius_pupil - 50;
            int stopy = ptr_pupil_info.B + radius_pupil + 50;

            //printf("startx, starty = %d %d\n", startx, starty);
            //printf("stopx, stopy = %d %d\n", stopx, stopy);

            //if (startx < 0) startx = 0;
            //if (starty < 0) starty = 0;

            for (j = starty; j < stopy; j++)
            {
                for (i = startx; i < stopx; i++)
                {
                    ptr_roi_image.UPixel[j - starty, i - startx] = ptr_my_image.UPixel[j, i];
                    //			ptr_ori_roi_image->pixel[j-starty][i-startx] = ptr_ori_image->pixel[j][i];
                    if (Math.Pow(i - ptr_pupil_info.A, 2) + Math.Pow(j - ptr_pupil_info.B, 2) <= Math.Pow(radius_pupil, 2))
                        ptr_roi_image.UPixel[j - starty, i - startx] = 0;
                }
            }
            //path = build_result_path(result_dir_path, "gaussian-roi.pgm");
            //write_output(path, ptr_roi_image);
            //sprintf(roi_path, "%s", path);
            debugImage.DebugImg_WriteImg("gaussian-roi.pgm", ptr_roi_image);


            int[] HIP = new int[roi_width];
            int[] HIP_mirror_sum = new int[roi_width / 2 - 1];

            //FileStream fp_roi_HIP, fp_roi_mirror_HIP, fp_roi_mirror_dHIP;
            //StreamWriter sp_roi_HIP, sp_roi_mirror_HIP, sp_roi_mirror_dHIP;

            for (i = 0; i < roi_width; i++)
                HIP[i] = 0;

            for (i = 0; i < roi_width; i++)
            {
                sum = 0;
                for (j = -3; j <= 3; j++)
                {
                    sum += ptr_roi_image.UPixel[roi_height / 2 + j, i];
                }
                HIP[i] = sum / 7;
            }
            histo_smoothing(HIP, roi_width);

            //fp_roi_HIP = new FileStream("fp_roi_HIP.txt", FileMode.Create);
            //sp_roi_HIP = new StreamWriter(fp_roi_HIP);
            // for (j = 1; j < roi_width; j++)
            // {
            //     sp_roi_HIP.WriteLine(j + " " + HIP[j]);
            //     fprintf(fp_roi_HIP, "%d %d\n", j, HIP[j]);
            //  }
            // sp_roi_HIP.Dispose();
            // sp_roi_HIP.Close();
            //  fp_roi_HIP.Close();

            debugImage.DebugImg_WriteIrisTxt("fp_roi_HIP.txt", HIP, 1, roi_width);



            for (i = 0; i < roi_width / 2 - 1; i++)
                HIP_mirror_sum[i] = 0;

            for (i = 1; i < roi_width / 2; i++)
            {
                HIP_mirror_sum[i - 1] = (int)(Math.Pow(HIP[roi_width / 2 - i], 2) + Math.Pow(HIP[roi_width / 2 + i], 2));
            }

            int[] d_HIP_mirror_sum = new int[roi_width / 2 - 2];

            //int max_dHIP_peak = 0;
            int radius_iris = 0;
            for (i = 1; i < roi_width / 2 - 1; i++)
            {
                d_HIP_mirror_sum[i - 1] = Math.Abs(HIP_mirror_sum[i] - HIP_mirror_sum[i - 1]);
                //		if (d_HIP_mirror_sum[i-1] > max_dHIP_peak) {
                //			max_dHIP_peak = d_HIP_mirror_sum[i-1];
                //			radius_iris = i;
                //		}
            }

            histo_smoothing(d_HIP_mirror_sum, roi_width / 2 - 2);

            Iris.peak_loc dHIP_peak = new Iris.peak_loc();
            dHIP_peak.First = radius_pupil;
            dHIP_peak.Second = find_second_peak(d_HIP_mirror_sum, roi_width / 2 - 2, radius_pupil);
            Iris.peak_loc ptr_dHIP_peak = dHIP_peak;

            //	find_two_peaks(d_HIP_mirror_sum, roi_width/2 - 2, ptr_dHIP_peak);

            radius_iris = ptr_dHIP_peak.Second;

            //fp_roi_mirror_HIP = new FileStream("fp_roi_mirror_HIP.txt", FileMode.Create);
            //sp_roi_mirror_HIP = new StreamWriter(fp_roi_mirror_HIP);
            // for (j = 0; j < roi_width / 2 - 1; j++)
            // {
            //     sp_roi_mirror_HIP.WriteLine(j + " " + HIP_mirror_sum[j]);
            //     fprintf(fp_roi_mirror_HIP, "%d %d\n", j, HIP_mirror_sum[j]);
            // }
            // sp_roi_mirror_HIP.Dispose();
            // sp_roi_mirror_HIP.Close();
            // fp_roi_mirror_HIP.Close();
            debugImage.DebugImg_WriteIrisTxt("fp_roi_mirror_HIP.txt", HIP_mirror_sum, 0, (roi_width / 2) - 1);


            //fp_roi_mirror_dHIP = new FileStream("fp_roi_mirror_dHIP.txt", FileMode.Create);
            //sp_roi_mirror_dHIP = new StreamWriter(fp_roi_mirror_dHIP);
            //for (j = 0; j < roi_width / 2 - 2; j++)
            //{
            //    sp_roi_mirror_dHIP.WriteLine(j + " " + d_HIP_mirror_sum[j]);
            //     fprintf(fp_roi_mirror_dHIP, "%d %d\n", j, d_HIP_mirror_sum[j]);
            // }
            //sp_roi_mirror_dHIP.Dispose();
            //sp_roi_mirror_dHIP.Close();
            //fp_roi_mirror_dHIP.Close();

            debugImage.DebugImg_WriteIrisTxt("fp_roi_mirror_dHIP.txt", d_HIP_mirror_sum, 0, (roi_width / 2) - 2);

            ptr_iris_boundary.Pupil = radius_pupil;
            ptr_iris_boundary.Limbus = radius_iris;

            for (j = starty; j < stopy; j++)
            {
                for (i = startx; i < stopx; i++)
                {
                    if (Math.Pow(i - ptr_pupil_info.A, 2) + Math.Pow(j - ptr_pupil_info.B, 2) > Math.Pow(radius_pupil, 2) &&
                            Math.Pow(i - ptr_pupil_info.A, 2) + Math.Pow(j - ptr_pupil_info.B, 2) < Math.Pow(radius_iris, 2))
                    {
                        ptr_ori_roi_image.UPixel[j - starty, i - startx] = ptr_ori_image.UPixel[j, i];
                    }
                }
            }

            //path = build_result_path(result_dir_path, "ori-roi-image.pgm");
            //write_output(path, ptr_ori_roi_image);
            //sprintf(localized_iris_path, "%s", path);
            //printf("localized_iris_path = %s\n", localized_iris_path);
            debugImage.DebugImg_WriteImg("ori-roi-image.pgm", ptr_ori_roi_image);
            debugImage.DebugImg_ImgToTxt("ori-roi-image.txt", ptr_ori_roi_image);
            //	GtkTextviewAppend(text_field, "Iris localization . . . OK\n");	
            //path[0] = '\0';

            //	write_output("img/ori_roi_image.pgm", ptr_ori_roi_image);
            //	myfree2((void **)ptr_roi_image->pixel, ptr_roi_image->height);*/
        }

        void histo_smoothing(int[] histo, int size)
        {
            int i, sum;

            int[] avg = new int[size];

            for (i = 0; i < size; i++)
                avg[i] = 0;

            for (i = 1; i < size - 1; i++)
            {
                sum = histo[i - 1] + histo[i] + histo[i + 1];
                avg[i] = sum / 3;
            }

            for (i = 1; i < size - 1; i++)
                histo[i] = avg[i];

        }

        int find_second_peak(int[] histo, int size, int radius_pupil)
        {
            int i = 0;
            int max_peak = 0;
            int n_peak = 0;
            byte b_peak = 0;
            int j = 0;
            int threshold = 500;
            for (i = radius_pupil + 10; i < size - 2; i++)
            {
                for (j = 1; j <= 8; j++)
                {
                    if ((i + j) >= size || (i - j) < 0)
                    {
                        b_peak = 0;
                        break;
                    }
                    if (histo[i] >= histo[i + j] && histo[i] >= histo[i - j])
                        b_peak = 1;
                    else
                    {
                        b_peak = 0;
                        break;
                    }
                }
                if (b_peak == 1 && histo[i] > max_peak && histo[i] > threshold)
                {
                    max_peak = histo[i];
                    n_peak = i;
                }
            }
            //printf("n_peak = %d\n", n_peak);

            if (n_peak == 0)
            {
                for (i = radius_pupil + 10; i < size - 2; i++)
                {
                    for (j = 1; j <= 8; j++)
                    {
                        if ((i + j) >= size || (i - j) < 0)
                        {
                            b_peak = 0;
                            break;
                        }
                        if (histo[i] >= histo[i + j] && histo[i] >= histo[i - j])
                            b_peak = 1;
                        else
                        {
                            b_peak = 0;
                            break;
                        }
                    }
                    if (b_peak == 1 && histo[i] > max_peak && histo[i] > threshold - 300)
                    {
                        max_peak = histo[i];
                        n_peak = i;
                    }
                }
                //printf("n_peak 2 = %d\n", n_peak);
            }

            if (n_peak == 0)
            {
                for (i = radius_pupil + 10; i < size - 2; i++)
                {
                    for (j = 1; j <= 5; j++)
                    {
                        if (histo[i] >= histo[i + j] && histo[i] >= histo[i - j])
                            b_peak = 1;
                        else
                        {
                            b_peak = 0;
                            break;
                        }
                    }
                    if (b_peak == 1 && histo[i] > max_peak && histo[i] > threshold)
                    {
                        max_peak = histo[i];
                        n_peak = i;
                    }
                }
                //printf("n_peak 2 = %d\n", n_peak);
            }



            return n_peak;
        }

        public void iris_unwrapping(MyImage ptr_ori_roi_image, Iris.iris_boundary ptr_iris_boundary, Iris.pupil_info ptr_pupil_info, MyImage ptr_unwrapped_image)
        {
            int j, i;
            int Rpupil = ptr_iris_boundary.Pupil;
            int Rlimbus = ptr_iris_boundary.Limbus;
            int a = ptr_pupil_info.A;
            int b = ptr_pupil_info.B;

            Debug.WriteLine("Rpupil " + Rpupil + " Rlimbus " + Rlimbus + " a " + a + " b " + b);

           // int x, y;
            int theta = ptr_unwrapped_image.Width;
            int radius = ptr_unwrapped_image.Height;
            int x_r_theta, y_r_theta, xp_theta, yp_theta, xl_theta, yl_theta;//, x_center, y_center;
            byte[,] temp_unwrapped = new byte[radius, theta * 2];
            //printf("theta %d radius %d\n", theta, radius);

            for (j = 0; j < radius; j++)
            {
                for (i = 0; i < theta * 2; i++)
                {
                    // find coordinate of the pupillary boundary point at angle = theta
                    xp_theta = a + (int)((float)Rpupil * MathBppt.toFloat(Math.Cos(Math.PI * (float)i / 180)));
                    yp_theta = b - (int)((float)Rpupil * MathBppt.toFloat(Math.Sin(Math.PI * (float)i / 180)));

                    // find coordinate of the limbus boundary point  at angle = theta
                    xl_theta = a + (int)((float)Rlimbus * MathBppt.toFloat(Math.Cos(Math.PI * (float)i / 180)));
                    yl_theta = b - (int)((float)Rlimbus * MathBppt.toFloat(Math.Sin(Math.PI * (float)i / 180)));

                    // find the intensity of pixel located at ( x_r_theta, y_r_theta) in the origin
                    x_r_theta = (int)((1.0 - (float)j / radius) * xp_theta + (float)j / radius * xl_theta);
                    y_r_theta = (int)((1.0 - (float)j / radius) * yp_theta + (float)j / radius * yl_theta);
                    //if (y_r_theta < 0) y_r_theta = 0;
                    //if (x_r_theta < 0) x_r_theta = 0;
                    temp_unwrapped[j, i] = ptr_ori_roi_image.UPixel[y_r_theta, x_r_theta];
                }
            }
            debugImage.DebugImg_ImgToTxt("ptr_ori_roi_image.txt", ptr_ori_roi_image);
            debugImage.DebugImg_byte2dToTxt("temp_unwrapped.txt", temp_unwrapped);

            // Get only the lower half of iris region
            for (j = 0; j < radius; j++)
            {
                for (i = 0; i < theta; i++)
                {
                    ptr_unwrapped_image.UPixel[j, i] = temp_unwrapped[j, i + theta];
                }
            }

            debugImage.DebugImg_ImgToTxt("ptr_unwrapped_image.txt", ptr_unwrapped_image);

            //	FILE *fp_unwrapped_histo;
            //	fp_unwrapped_histo = fopen("fp_unwrapped_histo.txt", "w");
            Preprocessing preprocessing = new Preprocessing(debugImage);
            preprocessing.contrast_stretching(ptr_unwrapped_image, ptr_unwrapped_image, 100);

            //	int *histo = malloc(L_MAX * sizeof(int));
            //	histo = histogram(ptr_unwrapped_image);

            //  for(i = 0; i < L_MAX; i++)    
            //		fprintf(fp_unwrapped_histo,"%d %d\n",i,histo[i]);

            //myfree2((void **)temp_unwrapped, radius);
            //	fclose(fp_unwrapped_histo);
            //	free(histo);
        }

        public void check_otsu(MyImage ptr_binary_image)
        {
            int height = ptr_binary_image.Height;
            int width = ptr_binary_image.Width;

            int i, j;

            int black = 0;
            int white = 0;

            for (j = 0; j < height; j++)
            {
                for (i = 0; i < width; i++)
                {
                    if (ptr_binary_image.UPixel[j, i] == 0)
                        black++;
                    else
                        white++;
                }
            }
            //	printf("white %d black %d\n", white, black);
            IntensityTransform intensityTransform = new IntensityTransform();
            if (white > black)
                intensityTransform.complement(ptr_binary_image);
        }

        public void gabor_filtering(MyImage ptr_unwrapped_image, MyImage ptr_gabor_real_image, MyImage ptr_gabor_imag_image, Iris.iriscode ptr_iriscode, MyImage ptr_otsu_unwrapped_image)
        {
            int radius = ptr_unwrapped_image.Height;
            int theta = ptr_unwrapped_image.Width;

            int x, y;

            //float sigma;
            float u0 = 8;
            float v0 = 1;

            int j, i;//, k, l;

            int g_size = 15;

            int x0 = (g_size - 1) / 2;//theta/2;
            int y0 = (g_size - 1) / 2;//radius/2;

            float gauss_radius = (g_size - 1) / 2;
            int r = (int)gauss_radius;
            int rows = r * 2 + 1;

            float[,] matrix2D = new float[rows, rows];
            for (j = 0; j < rows; j++)
                for (i = 0; i < rows; i++)
                    matrix2D[j, i] = 0;

            SpatialFiltering spatialFiltering = new SpatialFiltering(debugImage);
            spatialFiltering.gaussian2Delliptical(matrix2D, gauss_radius, 5, 4, 0);

            float[,] real = new float[rows, rows];
            float[,] imag = new float[rows, rows];
            float calc = 0;

            //	float rotation = 45 * M_PI / 180;
            for (j = 0; j < g_size; j++)
            {
                for (i = 0; i < g_size; i++)
                {
                    calc = (float)(-2 * Math.PI * (u0 * (i - x0) + v0 * (j - y0)) * Math.PI / 180);
                    //		printf("calc %f\n", calc);
                    real[j, i] = (float)Math.Cos(calc);
                    imag[j, i] = (float)Math.Sin(calc);
                }
            }

            float[,] real_gabor = new float[rows, rows];
            float[,] imag_gabor = new float[rows, rows];

            //printf("real\n");
            for (j = 0; j < g_size; j++)
            {
                for (i = 0; i < g_size; i++)
                {
                    real_gabor[j, i] = real[j, i] * matrix2D[j, i];
                    //		printf("%f ", real_gabor[j][i]);
                }
                //	printf("\n");
            }

            //printf("imaginary\n");
            for (j = 0; j < g_size; j++)
            {
                for (i = 0; i < g_size; i++)
                {
                    imag_gabor[j, i] = imag[j, i] * matrix2D[j, i];
                    //		printf("%f ", imag_gabor[j][i]);
                }
                //	printf("\n");
            }

            float[,] real_gabor_scaled = new float[rows, rows];
            float[,] imag_gabor_scaled = new float[rows, rows];

            for (j = 0; j < g_size; j++)
                for (i = 0; i < g_size; i++)
                {
                    real_gabor_scaled[j, i] = real_gabor[j, i];
                    imag_gabor_scaled[j, i] = imag_gabor[j, i];
                }

            IntensityTransform intensityTransform = new IntensityTransform();
            intensityTransform.scaling(real_gabor_scaled, g_size, g_size);
            intensityTransform.scaling(imag_gabor_scaled, g_size, g_size);


            MyImage real_image = new MyImage(rows, rows);
            MyImage ptr_real_image = real_image;

            MyImage imag_image = new MyImage(rows, rows);
            MyImage ptr_imag_image = imag_image;

            for (j = 0; j < g_size; j++)
                for (i = 0; i < g_size; i++)
                {
                    real_image.UPixel[j, i] = (byte)real_gabor_scaled[j, i];
                    imag_image.UPixel[j, i] = (byte)imag_gabor_scaled[j, i];
                }

            float[,] new_pixel_real = new float[radius, theta];
            float[,] new_pixel_imag = new float[radius, theta];
            for (j = 0; j < radius; j++)
                for (i = 0; i < theta; i++)
                {
                    new_pixel_real[j, i] = 0;
                    new_pixel_imag[j, i] = 0;
                }

            MyImage padded_image = new MyImage(radius + r * 2, theta + r * 2);
            MyImage ptr_padded_image = padded_image;

            Padding padd = new Padding();
            padd.padding(ptr_unwrapped_image, r, ptr_padded_image);

            for (j = r; j < padded_image.Height - r; j++)
            {
                for (i = r; i < padded_image.Width - r; i++)
                {
                    for (y = -r; y <= r; y++)
                    {
                        for (x = -r; x <= r; x++)
                        {
                            new_pixel_real[j - r, i - r] += real_gabor[y + r, x + r] * ptr_padded_image.UPixel[j + y, i + x];
                            new_pixel_imag[j - r, i - r] += imag_gabor[y + r, x + r] * ptr_padded_image.UPixel[j + y, i + x];
                        }
                    }
                }
            }

            for (j = 0; j < radius; j++)
                for (i = 0; i < theta; i++)
                {
                    if (new_pixel_real[j, i] >= 0)
                        ptr_gabor_real_image.UPixel[j, i] = 255;
                    else
                        ptr_gabor_real_image.UPixel[j, i] = 0;
                    //	printf("%f\n", new_pixel_real[j][i]);
                }

            for (j = 0; j < radius; j++)
                for (i = 0; i < theta; i++)
                {
                    if (new_pixel_imag[j, i] >= 0)
                    {
                        ptr_gabor_imag_image.UPixel[j, i] = 255;
                    }
                    else
                    {
                        ptr_gabor_imag_image.UPixel[j, i] = 0;
                    }
                    //	printf("%f\n", new_pixel_imag[j][i]);
                }

            byte[] bit_string = new byte[ptr_iriscode.Size + 1];
            byte[] bit_mask_string = new byte[ptr_iriscode.Size + 1];

            for (i = 0; i < ptr_iriscode.Size + 1; i++)
            {
                bit_string[i] = 0;
                bit_mask_string[i] = 0;
            }


            int c = 0;

            //printf("iriscode :\n");
            for (j = 0; j < radius; j++)
            {
                for (i = 0; i < theta; i++)
                {
                    if (new_pixel_real[j, i] >= 0)
                    {
                        ptr_gabor_real_image.UPixel[j, i] = 255;
                        ptr_iriscode.Bit[c] = 1;
                        bit_string[c] = 1;
                    }
                    else
                    {
                        ptr_gabor_real_image.UPixel[j, i] = 0;
                        ptr_iriscode.Bit[c] = 0;
                        bit_string[c] = 0;
                    }
                    //			printf("%d ",ptr_iriscode->bit[c]);
                    c++;
                    if (new_pixel_imag[j, i] >= 0)
                    {
                        ptr_gabor_imag_image.UPixel[j, i] = 255;
                        ptr_iriscode.Bit[c] = 1;
                        bit_string[c] = 1;
                    }
                    else
                    {
                        ptr_gabor_imag_image.UPixel[j, i] = 0;
                        ptr_iriscode.Bit[c] = 0;
                        bit_string[c] = 0;
                    }
                    c++;
                    //			printf("%d ",ptr_iriscode->bit[c]);
                }
            }
            //printf("%s\n", ptr_iriscode->bit);

            //	for (j = 0; j < ptr_iriscode->size; j++)
            //		printf("%d", ptr_iriscode->bit[j]);
            //	printf("\n");

            c = 0;
            for (j = 0; j < radius; j++)
            {
                for (i = 0; i < theta; i++)
                {
                    if (ptr_otsu_unwrapped_image.UPixel[j, i] == 0)
                    {
                        ptr_iriscode.Mask[c] = 0;
                        bit_mask_string[c] = 0;
                        c++;
                        ptr_iriscode.Mask[c] = 0;
                        bit_mask_string[c] = 0;
                        c++;
                    }
                    else
                    {
                        ptr_iriscode.Mask[c] = 1;
                        bit_mask_string[c] = 0;
                        c++;
                        ptr_iriscode.Mask[c] = 1;
                        bit_mask_string[c] = 0;
                        c++;
                    }
                }
            }
            //	for (j = 0; j < 2 * radius * theta; j++) {
            //		printf("%d\n", ptr_iriscode->bit[j]);
            //	}
            
            debugImage.DebugImg_WriteImg("ptr_real_image.pgm", ptr_real_image);
            debugImage.DebugImg_WriteImg("ptr_imag_image.pgm", ptr_imag_image);
            debugImage.DebugImg_WriteImg("ptr_gabor_real_image.pgm", ptr_gabor_real_image);
            debugImage.DebugImg_WriteImg("ptr_gabor_imag_image.pgm", ptr_gabor_imag_image);

        }

        public void WriteIrisObj(string fileName, Iris.iris_obj irisObj, FileMode fileMode)
        {
            FileStream fs = new FileStream(fileName, fileMode);
            StreamWriter sw = new StreamWriter(fs);
            sw.WriteLine(irisObj.Class_no + "_" + irisObj.Side + "_" + irisObj.Name + "_" + irisObj.Bit + "_" + irisObj.Bit_mask);
            sw.Dispose();
            sw.Close();
            fs.Close();
        }

        
    }
}
