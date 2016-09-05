using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BiometriBPPT.bppt.ptik.biometric.entity;
using BiometriBPPT.bppt.ptik.biometric.utility;

namespace BiometriBPPT.bppt.ptik.biometric.image
{
    public class Preprocessing
    {
        DebugImage debugImage;

        public Preprocessing(DebugImage debI)
        {
            debugImage = debI;
        }

        MatrixBppt mat = new MatrixBppt();

        private void bubbleSort(byte[] x, int datanum)
        {
            int i, j;
            byte tmp;
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
        public void MedianFilter(MyImage InputImage)
        {
            int i, j, k, l, x = 0;
            byte[] s = new byte[9];
            for (j = 1; j < InputImage.Height - 1; j++)
            {
                for (i = 1; i < InputImage.Width - 1; i++)
                {
                    x = 0;
                    for (l = j - 1; l < j + 2; l++)
                    {
                        for (k = i - 1; k < i + 2; k++)
                        {
                            try
                            {
                                s[x] = InputImage.UPixel[l, k];
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
                        InputImage.UPixel[j, i] = (byte)s[4];
                    }
                    catch (Exception)
                    {

                        //throw;
                    }
                }
            }
        }

        public int[] histogram(MyImage ptr_my_image)
        {
            //  int histo[L_MAX];
            int[] histo = new int[Const.L_MAX];
            int i, j;

            for (i = 0; i < Const.L_MAX; i++)
                histo[i] = 0;

            for (j = 0; j < ptr_my_image.Height; j++)
                for (i = 0; i < ptr_my_image.Width; i++)
                    histo[ptr_my_image.UPixel[j, i]]++;

            return histo;
        }

        public void HistogramEqualization(MyImage InputImage)
        {
            int i, j = 0;
            byte[] s = new byte[Const.L_MAX];
            float[] p = new float[Const.L_MAX];
            float sum_p = 0;

            int[] histo = histogram(InputImage);
            for (i = 0; i < Const.L_MAX; i++)
                histo[i] = 0;

            for (j = 0; j < InputImage.Height; j++)
                for (i = 0; i < InputImage.Width; i++)
                    histo[InputImage.UPixel[j, i]]++;

            /* probability of each intensity value */
            for (i = 0; i < Const.L_MAX; i++)
                p[i] = ((float)histo[i]) / (InputImage.Height * InputImage.Width);

            for (i = 0; i < Const.L_MAX; i++)
            {
                sum_p += p[i];
                s[i] = (byte)(((Const.L_MAX - 1) * sum_p) + 0.5);
            }

            /* mapping */
            for (j = 0; j < InputImage.Height; j++)
                for (i = 0; i < InputImage.Width; i++)
                    InputImage.UPixel[j, i] = s[InputImage.UPixel[j, i]];
        }

        public void LaplacianSharpening(MyImage ptr_my_image)
        {
            int i, j;
            //float min, max;
            float[,] tmppixel;
            int height = ptr_my_image.Height;
            int width = ptr_my_image.Width;
            int[,] filter = new int[3, 3]{   
    // Filter 3x3 di bawah bisa diganti dengan berbagai filter Laplacian yang lain
    {0, 1, 0},
    {1,-4, 1},
    {0, 1, 0}


    /* Jenis filter laplacian yang lain, menurut buku Gonzales Fig.3.37
// filter-a
    {0, 1, 0},
    {1,-4, 1},
    {0, 1, 0}

// filter-b      
    {1, 1, 1},
    {1,-8, 1},
    {1, 1, 1},

// filter-c
    { 0, -1,  0},
    {-1,  4, -1},
    { 0, -1,  0},

// filter-d
    {-1,-1,-1},
    {-1, 8,-1},
    {-1,-1,-1},

     */
  };


            tmppixel = new float[height, width];

            for (j = 0; j < height; j++)
                for (i = 0; i < width; i++)
                    tmppixel[j, i] = (float)ptr_my_image.UPixel[j, i];

            for (j = 1; j < height - 1; j++)
                for (i = 1; i < width - 1; i++)
                    tmppixel[j, i] = (
                            filter[0, 0] * (float)ptr_my_image.UPixel[j - 1, i - 1] +
                            filter[0, 1] * (float)ptr_my_image.UPixel[j - 1, i] +
                            filter[0, 2] * (float)ptr_my_image.UPixel[j - 1, i + 1] +

                            filter[1, 0] * (float)ptr_my_image.UPixel[j, i - 1] +
                            filter[1, 1] * (float)ptr_my_image.UPixel[j, i] +
                            filter[1, 2] * (float)ptr_my_image.UPixel[j, i + 1] +

                            filter[2, 0] * (float)ptr_my_image.UPixel[j + 1, i - 1] +
                            filter[2, 1] * (float)ptr_my_image.UPixel[j + 1, i] +
                            filter[2, 2] * (float)ptr_my_image.UPixel[j + 1, i + 1]
                    );

            for (j = 0; j < height; j++)
                for (i = 0; i < width; i++)
                    if (tmppixel[j, i] < 0)
                        tmppixel[j, i] = 0;

            // 7 baris berikutnya adalah menambahkan mask yang diperoleh kepada citra asli untuk membuat sharp. apabila anda ingin menampilkan mask-nya saja, maka bagian sharpening-dari sini sampai sharpening-sampai sini harap dibuat sebagai comment/di nonaktifkan

            // sharpening-dari sini  

            for (j = 0; j < height; j++)
                for (i = 0; i < width; i++)
                {
                    tmppixel[j, i] = (float)ptr_my_image.UPixel[j, i] - tmppixel[j, i]; // use - if a or b filter is used
                    //      tmppixel[j][i]=(float)image_pixel[j][i]+tmppixel[j][i]; // use + if c or d filter is used
                    if (tmppixel[j, i] < 0)
                        tmppixel[j, i] = 0;
                    if (tmppixel[j, i] > 255)
                        tmppixel[j, i] = 255;
                }

            // sharpening-sampai sini

            for (j = 0; j < height; j++)
                for (i = 0; i < width; i++)
                    ptr_my_image.UPixel[j, i] = (byte)tmppixel[j, i];



        }

        public byte GetOtsuThreshold(MyImage img)
        {
            int i, j;
            int[] histo = new int[Const.L_MAX];
            float[] nhisto = new float[Const.L_MAX];
            float[] P1 = new float[Const.L_MAX];
            float[] m = new float[Const.L_MAX];
            float[] betweenClassVar = new float[Const.L_MAX];
            float sum_p, sum_m, mG, max, etaSeparabilityMeasure, globalVar;
            byte threshold = 0x00;

            //for (i = 0; i < L_MAX; i++)
            // {
            //     histo[i] = 0;
            //    nhisto[i] = 0;
            //}

            for (j = 0; j < img.Height; j++)
                for (i = 0; i < img.Width; i++)
                    histo[img.UPixel[j, i]]++;

            for (i = 0; i < Const.L_MAX; i++)
                nhisto[i] = (float)histo[i] / (img.Height * img.Width); /* normalized histogram */

            sum_p = 0;
            for (i = 0; i < Const.L_MAX; i++)
            {
                sum_p += nhisto[i];
                P1[i] = sum_p; /* P1[i] is the probability that a UPixel to be assigned to class C1 */
            }

            sum_m = 0;
            for (i = 0; i < Const.L_MAX; i++)
            {
                sum_m += i * nhisto[i];
                m[i] = sum_m; /* cumulative mean (average intensity) up to level k Eq.(10.3-8) */
            }

            for (mG = 0, i = 0; i < Const.L_MAX; i++)
                mG += i * nhisto[i]; /* the average intensity (global mean) Eq.(10.3-9) */

            for (globalVar = 0, i = 0; i < Const.L_MAX; i++)
                globalVar += ((float)i - mG) * ((float)i - mG) * nhisto[i]; /* global variance Eq.(10.3-13) */

            max = -1;
            for (i = 0; i < Const.L_MAX; i++)
            {
                if (P1[i] > 0 && P1[i] < 1)
                {
                    betweenClassVar[i] = (mG * P1[i] - m[i]) * (mG * P1[i] - m[i]) / (P1[i] * (1 - P1[i]));
                    if (betweenClassVar[i] > max)
                    {
                        max = betweenClassVar[i];
                        threshold = (byte)i;
                    }
                }
            }

            etaSeparabilityMeasure = betweenClassVar[threshold] / globalVar;
            // Log(DEBUG, "Binarization threshold by Otsu method: %d, eta: %f\n", threshold, etaSeparabilityMeasure);

            return threshold;
        }

        public void Binarize(MyImage img, byte THRESHOLD, byte MAX_VAL, byte MIN_VAL)
        {
            int i, j;
            for (j = 0; j < img.Height; j++)
            {
                for (i = 0; i < img.Width; i++)
                {
                    img.UPixel[j, i] = (img.UPixel[j, i] > THRESHOLD) ? MAX_VAL : MIN_VAL;
                }
            }
        }

        public void Dilation(MyImage InputImage)
        {
            int i, j;
            byte[] tmp = new byte[InputImage.Height * InputImage.Width];
            for (j = 0; j < InputImage.Height; j++)
            {
                for (i = 0; i < InputImage.Width; i++)
                {
                    tmp[(j * InputImage.Width) + i] = InputImage.UPixel[j, i];
                }
            }

            for (j = 1; j < InputImage.Height - 1; j++)
            {
                for (i = 1; i < InputImage.Width - 1; i++)
                {
                    if (tmp[((j - 1) * InputImage.Width) + (i - 1)] == Const.FG || tmp[((j - 1) * InputImage.Width) + (i)] == Const.FG || tmp[((j - 1) * InputImage.Width) + (i + 1)] == Const.FG
                        || tmp[((j) * InputImage.Width) + (i - 1)] == Const.FG || tmp[((j) * InputImage.Width) + (i + 1)] == Const.FG
                        || tmp[((j + 1) * InputImage.Width) + (i - 1)] == Const.FG || tmp[((j + 1) * InputImage.Width) + (i)] == Const.FG || tmp[((j + 1) * InputImage.Width) + (i + 1)] == Const.FG)
                        InputImage.UPixel[j, i] = Const.FG;
                }
            }
        }

        public void Erosion(MyImage InputImage)
        {
            int i, j;
            /*
            byte[] tmp = new byte[InputImage.Height * InputImage.Width];

            for (j = 0; j < InputImage.Height; j++)
            {
                for (i = 0; i < InputImage.Width; i++)
                {
                    tmp[(j * InputImage.Width) + i] = InputImage.UPixel[j, i];
                }
            }

            for (j = 1; j < InputImage.Height - 1; j++)
            {
                for (i = 1; i < InputImage.Width - 1; i++)
                {
                    if (tmp[((j - 1) * InputImage.Width) + (i - 1)] == Iris.HIGH || tmp[((j - 1) * InputImage.Width) + (i)] == Iris.HIGH || tmp[((j - 1) * InputImage.Width) + (i + 1)] == Iris.HIGH
                        || tmp[((j) * InputImage.Width) + (i - 1)] == Iris.HIGH || tmp[((j) * InputImage.Width) + (i + 1)] == Iris.HIGH
                        || tmp[((j + 1) * InputImage.Width) + (i - 1)] == Iris.HIGH || tmp[((j + 1) * InputImage.Width) + (i)] == Iris.HIGH || tmp[((j + 1) * InputImage.Width) + (i + 1)] == Iris.HIGH)
                        InputImage.UPixel[j, i] = Iris.HIGH;
                }
            }
             * */
            byte[,] tmppixel = new byte[InputImage.Height, InputImage.Width];
            for (j = 0; j < InputImage.Height; j++)
                for (i = 0; i < InputImage.Width; i++)
                    tmppixel[j, i] = InputImage.UPixel[j, i];
            for (j = 1; j < InputImage.Height - 1; j++)
                for (i = 1; i < InputImage.Width - 1; i++)
                {
                    if (tmppixel[j - 1, i - 1] == Iris.HIGH ||
                       tmppixel[j - 1, i] == Iris.HIGH ||
                       tmppixel[j - 1, i + 1] == Iris.HIGH ||
                   tmppixel[j, i - 1] == Iris.HIGH ||
                       tmppixel[j, i + 1] == Iris.HIGH ||
                   tmppixel[j + 1, i - 1] == Iris.HIGH ||
                       tmppixel[j + 1, i] == Iris.HIGH ||
                       tmppixel[j + 1, i + 1] == Iris.HIGH)

                        InputImage.UPixel[j, i] = Iris.HIGH;
                }
        }

        public void Opening(MyImage img)
        {
            Erosion(img);
            Dilation(img);
        }

        public void Closing(MyImage img)
        {
            Dilation(img);
            Erosion(img);
        }

        public int cconc(int[] inb)
        {
            int i, icn;
            icn = 0;

            for (i = 0; i < 8; i += 2)
            {
                if (inb[i] == 0)
                    if (inb[i + 1] == Const.MAXVAL || inb[i + 2] == Const.MAXVAL)
                        icn++;
            }

            return icn;
        }

        public void Thinning(MyImage img, double amp)
        {
            int it, jt;

            for (jt = 0; jt < img.Height; jt++)
            {
                for (it = 0; it < img.Width; it++)
                {
                    if (img.UPixel[jt, it] == 255)
                        img.UPixel[jt, it] = 0;
                    else
                        img.UPixel[jt, it] = 255;
                }
            }

            int[] ia = new int[9];
            int[] ic = new int[9];
            int i, ix, iy, m, ir, iv, iw = 0;

            MyImage outMyImage = new MyImage(img.Height, img.Width, MyImgType.UCHAR);

            /*for (iy = 0; iy < img.Height; iy++) {
                for (ix = 0; ix < img.Width; ix++) {
                    outMyImage.UPixel[iy,ix] = img.UPixel[iy,ix];
                }
            }*/

            Buffer.BlockCopy(img.UPixel, 0, outMyImage.UPixel, 0, img.UPixel.Length);

            m = 100; ir = 1;
            while (ir != 0)
            {
                ir = 0;
                for (iy = 1; iy < img.Height - 1; iy++)
                {
                    for (ix = 1; ix < img.Width - 1; ix++)
                    {
                        if (outMyImage.UPixel[iy, ix] != Const.MAXVAL)
                            continue;

                        ia[0] = outMyImage.UPixel[iy, ix + 1];
                        ia[1] = outMyImage.UPixel[iy - 1, ix + 1];
                        ia[2] = outMyImage.UPixel[iy - 1, ix];
                        ia[3] = outMyImage.UPixel[iy - 1, ix - 1];
                        ia[4] = outMyImage.UPixel[iy, ix - 1];
                        ia[5] = outMyImage.UPixel[iy + 1, ix - 1];
                        ia[6] = outMyImage.UPixel[iy + 1, ix];
                        ia[7] = outMyImage.UPixel[iy + 1, ix + 1];

                        for (i = 0; i < 8; i++)
                        {
                            if (ia[i] == m)
                            {
                                ia[i] = Const.MAXVAL;
                                ic[i] = 0;
                            }
                            else
                            {
                                if (ia[i] < Const.MAXVAL) ia[i] = 0;
                                ic[i] = ia[i];
                            }
                        }

                        ia[8] = ia[0];
                        ic[8] = ic[0];

                        if (ia[0] + ia[2] + ia[4] + ia[6] == Const.MAXVAL * 4)
                            continue;

                        iv = 0; iw = 0;

                        for (i = 0; i < 8; i++)
                        {
                            if (ia[i] == Const.MAXVAL) iv++;
                            if (ic[i] == Const.MAXVAL) iw++;
                        }

                        if (iv <= 1)
                            continue;
                        if (iw == 0)
                            continue;
                        if (cconc(ia) != 1)
                            continue;

                        if (outMyImage.UPixel[iy - 1, ix] == (byte)m)
                        {
                            ia[2] = 0;
                            if (cconc(ia) != 1)
                                continue;
                            ia[2] = Const.MAXVAL;
                        }

                        if (outMyImage.UPixel[iy, ix - 1] == (byte)m)
                        {
                            ia[4] = 0;
                            if (cconc(ia) != 1)
                                continue;
                            ia[4] = Const.MAXVAL;
                        }

                        outMyImage.UPixel[iy, ix] = (byte)m;
                        ir++;
                    }
                }
                m++;
            }

            for (iy = 0; iy < img.Height; iy++)
            {
                for (ix = 0; ix < img.Width; ix++)
                {
                    if (outMyImage.UPixel[iy, ix] < Const.MAXVAL)
                        outMyImage.UPixel[iy, ix] = 0;
                }
            }

            /*
    for (iy = 0; iy < img.Height; iy++) {
        for (ix = 0; ix < img.Width; ix++) {
            img.UPixel[iy, ix] = outMyImage.UPixel[iy, ix];
        }
    }
             */
            Buffer.BlockCopy(outMyImage.UPixel, 0, img.UPixel, 0, outMyImage.UPixel.Length);


            for (jt = 0; jt < img.Height; jt++)
            {
                for (it = 0; it < img.Width; it++)
                {
                    if (img.UPixel[jt, it] == Const.MAXVAL)
                        img.UPixel[jt, it] = Const.MINVAL;
                    else if (img.UPixel[jt, it] == Const.MINVAL)
                        img.UPixel[jt, it] = Const.MAXVAL;
                    else
                        img.UPixel[jt, it] = img.UPixel[jt, it];
                }
            }

            //MatRelease(&out);
        }

        public void FX_Thinning(MyImage img)
        {
            mat.Mat_Invert(img, img);
            Thinning(img, 0.5);
            mat.Mat_Invert(img, img);
        }

        public double Calc_Global_Mean(MyImage img)
        {
            int i, j;
            float[] n = new float[Const.L_MAX];
            float[] p = new float[Const.L_MAX];
            float globalMean = 0;

            for (j = 0; j < img.Height; j++)
            {
                for (i = 0; i < img.Width; i++)
                {
                    n[img.UPixel[j, i]]++;
                }
            }

            for (i = 0; i < Const.L_MAX; i++)
            {
                p[i] = n[i] / ((float)(img.Height * img.Width));
            }

            for (i = 0; i < Const.L_MAX; i++)
            {
                globalMean += i * p[i];
            }

            return globalMean;
        }

        public double Calc_Local_Sdev(MyImage img, int x, int y)
        {
            double sdev = 0, m = 0, var = 0;
            int a, b, r;
            int[] n = new int[Const.L_MAX];
            double[] p = new double[Const.L_MAX];

            for (b = y - 1; b <= y + 1; b++)
            {
                for (a = x - 1; a <= x + 1; a++)
                {
                    n[img.UPixel[b, a]]++;
                }
            }

            for (r = 0; r < Const.L_MAX; r++)
            {
                p[r] = ((double)n[r]) / (3.0 * 3.0);
            }

            for (r = 0; r < Const.L_MAX; r++)
            {
                m += r * p[r];
            }

            for (r = 0; r < Const.L_MAX; r++)
            {
                var += Math.Pow((r - m), 2) * p[r];
            }

            sdev = Math.Sqrt(var);

            return sdev;
        }

        public MyImage VariableThresholding(MyImage img, double SDEV_MULTIPLIER, double GLOBAL_MEAN_MULTIPLIER)
        {
            MyImage res = new MyImage(img.Height, img.Width, MyImgType.UCHAR);
            int i, j;
            double globalMean, sdev;

            globalMean = Calc_Global_Mean(img);

            for (j = 1; j < img.Height - 1; j++)
            {
                for (i = 1; i < img.Width - 1; i++)
                {
                    sdev = Calc_Local_Sdev(img, i, j);

                    if (img.UPixel[j, i] > (SDEV_MULTIPLIER * sdev)
                        && img.UPixel[j, i] > (GLOBAL_MEAN_MULTIPLIER * globalMean))
                    {
                        res.UPixel[j, i] = Const.MAXVAL;
                    }
                    else
                    {
                        res.UPixel[j, i] = Const.MINVAL;
                    }
                }
            }

            return res;
        }

        MyImage FX_VariableThresholding(MyImage img, double SDEV_MULTIPLIER, double GLOBAL_MEAN_MULTIPLIER)
        {
            MyImage temp = VariableThresholding(img, SDEV_MULTIPLIER, GLOBAL_MEAN_MULTIPLIER);
            MyImage res = temp;
            mat.Mat_Invert(res, res);

            return res;
        }

        public void contrast_stretching(MyImage ptr_ori_image, MyImage ptr_cs_ori_image, int threshold)
        {
            int[] my_histo = new int[Iris.L_MAX];
            int[] my_histo_cs = new int[Iris.L_MAX];

            my_histo = histogram(ptr_ori_image);

            int width = ptr_ori_image.Width;
            int height = ptr_ori_image.Height;

            //	int threshold = 50;
            int min_intensity = 0;
            int max_intensity = 0;

            int i, j;

            //FILE *cs_histo, *cs_histo_2;
            //cs_histo = fopen("cs_histo.txt", "w");
            //cs_histo_2 = fopen("cs_histo_2.txt", "w");

            //for(i = 0; i < L_MAX; i++)    
            //	fprintf(cs_histo,"%d %d\n",i,my_histo[i]);

            //debugImage.DebugImg_WriteIrisTxt("cs_histo.txt", my_histo, 0, Iris.L_MAX);

            for (i = 1; i < Iris.L_MAX; i++)
            {
                if (my_histo[i] > threshold)
                    max_intensity = i;
            }
            //printf("max_intensity %d\n", max_intensity);

            for (i = Iris.L_MAX - 1; i >= 1; i--)
            {
                if (my_histo[i] > threshold)
                    min_intensity = i;
            }
            //printf("min_intensity %d\n", min_intensity);

            float[,] my_pixel = new float[height, width];

            int max_cs_intensity = 0;
            int min_cs_intensity = 0;
            for (j = 0; j < height; j++)
            {
                for (i = 0; i < width; i++)
                {
                    //			ptr_cs_ori_image->pixel[j][i] = (ptr_ori_image->pixel[j][i] - min_intensity) * (255 / (max_intensity - min_intensity));
                    my_pixel[j, i] = (ptr_ori_image.UPixel[j, i] - min_intensity) * (255 / (max_intensity - min_intensity));
                    //			printf("cs %f\n", my_pixel[j][i]);
                    if (my_pixel[j, i] >= max_cs_intensity)
                        max_cs_intensity = (int)my_pixel[j, i];
                    if (my_pixel[j, i] <= min_cs_intensity)
                        min_cs_intensity = (int)my_pixel[j, i];
                }
            }

            //printf("max_cs_intensity %d\n", max_cs_intensity);
            //printf("min_cs_intensity %d\n", min_cs_intensity);
            IntensityTransform intensityTransform = new IntensityTransform();
            intensityTransform.scaling(my_pixel, height, width);
            /*	
                double new_pixel = 0;
                for (j = 0; j < height; j++) {
                    for (i = 0; i < width; i++) {
                        new_pixel = 255 * (((double)my_pixel[j][i] - (double)min_cs_intensity) / (double)max_cs_intensity);
                        printf("%f\n", new_pixel);
                        ptr_cs_ori_image->pixel[j][i] = (int)new_pixel;
                        //ptr_cs_ori_image->pixel[j][i] = my_pixel[j][i];
                    }
                }
            */
            for (j = 0; j < height; j++)
                for (i = 0; i < width; i++)
                    ptr_cs_ori_image.UPixel[j, i] = (byte)my_pixel[j, i];

            my_histo_cs = histogram(ptr_cs_ori_image);
            //for(i = 0; i < Iris.L_MAX; i++)    
            //fprintf(cs_histo_2,"%d %d\n",i,my_histo_cs[i]);
            // debugImage.DebugImg_WriteIrisTxt("cs_histo_2.txt", my_histo, 0, Iris.L_MAX);

        }

    }
}
