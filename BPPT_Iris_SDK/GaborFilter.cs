using BiometriBPPT.bppt.ptik.biometric.entity;
using BiometriBPPT.bppt.ptik.biometric.image;
using BiometriBPPT.bppt.ptik.biometric.iris;
using BiometriBPPT.bppt.ptik.biometric.utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BPPT_Iris_SDK
{
    public class GaborFilter
    {
        private PGM_Iris unwrappedImage;
        private PGM_Iris unwrappedImageReal;
        private int radius;
        private int theta;
        private int g_size;
        private int x0;
        private int y0;
        private float gauss_radius;
        private int r;
        private int rows;
        private string fileName = "";

        public GaborFilter(string f)
        {
            fileName = f;

            unwrappedImage = new PGM_Iris(fileName + "\\unwraped_128x8_otsu.pgm");
            unwrappedImageReal = new PGM_Iris(fileName + "\\unwraped_128x8.pgm");
            radius = unwrappedImageReal.Size.Height;
            theta = unwrappedImageReal.Size.Width;

            //g_size = 15;
            g_size = 5;

            x0 = (g_size - 1) / 2;//theta/2;
            y0 = (g_size - 1) / 2;//radius/2;

            gauss_radius = (g_size - 1) / 2;
            r = (int)gauss_radius;
            rows = r * 2 + 1;
        }

        public GaborFilter(string f1, string f2)
        {
            unwrappedImage = new PGM_Iris(f2);
            unwrappedImageReal = new PGM_Iris(f1);
            radius = unwrappedImageReal.Size.Height;
            theta = unwrappedImageReal.Size.Width;

            //g_size = 15;
            g_size = 5;

            x0 = (g_size - 1) / 2;//theta/2;
            y0 = (g_size - 1) / 2;//radius/2;

            gauss_radius = (g_size - 1) / 2;
            r = (int)gauss_radius;
            rows = r * 2 + 1;
        }

        public GaborFilter(PGM_Iris unwraped_128x8, PGM_Iris unwraped_128x8_otsu)
        {
            unwrappedImage = unwraped_128x8_otsu;
            unwrappedImageReal = unwraped_128x8;
            radius = unwrappedImageReal.Size.Height;
            theta = unwrappedImageReal.Size.Width;

            //g_size = 15;
            g_size = 5;

            x0 = (g_size - 1) / 2;//theta/2;
            y0 = (g_size - 1) / 2;//radius/2;

            gauss_radius = (g_size - 1) / 2;
            r = (int)gauss_radius;
            rows = r * 2 + 1;
        }

        public IrisCode PerformGaborFilter()
        {
            int x, y;

            //float sigma;
            //float u0 = 8;
            float u0 = 17.8f;
            float v0 = 2.23f;
            //float v0 = 1;

            int j, i;//, k, l;

            //float[,] matrix2D = gaussian2Delliptical(gauss_radius, 5f, 4f, 0);
            float[,] matrix2D = gaussian2Delliptical(gauss_radius, 1.85f, 1.48f, 0);

            float[,] real = new float[rows, rows];
            float[,] imag = new float[rows, rows];
            float calc = 0;

            for (j = 0; j < g_size; j++)
            {
                for (i = 0; i < g_size; i++)
                {
                    calc = (float)(-2 * Math.PI * (u0 * (i - x0) + v0 * (j - y0)) * Math.PI / 180);
                    real[j, i] = (float)Math.Cos(calc);
                    imag[j, i] = (float)Math.Sin(calc);
                }
            }

            float[,] real_gabor = new float[rows, rows];
            float[,] imag_gabor = new float[rows, rows];

            for (j = 0; j < g_size; j++)
            {
                for (i = 0; i < g_size; i++)
                {
                    real_gabor[j, i] = real[j, i] * matrix2D[j, i];
                }
            }

            for (j = 0; j < g_size; j++)
            {
                for (i = 0; i < g_size; i++)
                {
                    imag_gabor[j, i] = imag[j, i] * matrix2D[j, i];
                }
            }

            float[,] real_gabor_scaled = new float[rows, rows];
            float[,] imag_gabor_scaled = new float[rows, rows];

            for (j = 0; j < g_size; j++)
                for (i = 0; i < g_size; i++)
                {
                    real_gabor_scaled[j, i] = real_gabor[j, i];
                    imag_gabor_scaled[j, i] = imag_gabor[j, i];
                }

            IntensityTransformation.scaling(real_gabor_scaled, g_size, g_size);
            IntensityTransformation.scaling(imag_gabor_scaled, g_size, g_size);


            int[,] realImage = new int[rows, rows];
            int[,] imagImage = new int[rows, rows];

            for (j = 0; j < g_size; j++)
            {
                for (i = 0; i < g_size; i++)
                {
                    realImage[j, i] = (int)real_gabor_scaled[j, i];
                    imagImage[j, i] = (int)imag_gabor_scaled[j, i];
                }
            }

            float[,] new_pixel_real = new float[radius, theta];
            float[,] new_pixel_imag = new float[radius, theta];
            for (j = 0; j < radius; j++)
                for (i = 0; i < theta; i++)
                {
                    new_pixel_real[j, i] = 0;
                    new_pixel_imag[j, i] = 0;
                }

            int[,] paddedImage = padding(r);

            for (j = r; j < paddedImage.GetLength(0) - r; j++)
            {
                for (i = r; i < paddedImage.GetLength(1) - r; i++)
                {
                    for (y = -r; y <= r; y++)
                    {
                        for (x = -r; x <= r; x++)
                        {
                            new_pixel_real[j - r, i - r] += real_gabor[y + r, x + r] * paddedImage[j + y, i + x];
                            new_pixel_imag[j - r, i - r] += imag_gabor[y + r, x + r] * paddedImage[j + y, i + x];
                        }
                    }
                }
            }

            //int newHeight = radius;
            //int newWidth = theta;

            int[,] gaborRealImage = new int[radius, theta];
            int[,] gaborImagImage = new int[radius, theta];

            for (j = 0; j < radius; j++)
                for (i = 0; i < theta; i++)
                {
                    if (new_pixel_real[j, i] >= 0)
                        gaborRealImage[j, i] = 255;
                    else
                        gaborRealImage[j, i] = 0;
                }

            for (j = 0; j < radius; j++)
                for (i = 0; i < theta; i++)
                {
                    if (new_pixel_imag[j, i] >= 0)
                    {
                        gaborImagImage[j, i] = 255;
                    }
                    else
                    {
                        gaborImagImage[j, i] = 0;
                    }
                }

            IrisCode irisCode = new IrisCode();
            irisCode.size = (radius * theta * 2);
            byte[] bit_string = new byte[(radius * theta * 2)];
            byte[] bit_mask_string = new byte[(radius * theta * 2)];


            int c = 0;
            int b = 0;
            int[,] iriscodePGM = new int[radius, theta * 2];

            //printf("iriscode :\n");
            for (j = 0; j < radius; j++)
            {
                for (i = 0; i < theta; i++)
                {
                    if (new_pixel_real[j, i] >= 0)
                    {
                        gaborRealImage[j, i] = 255;
                        iriscodePGM[j, b] = 255;
                        //ptr_iriscode->bit[c] = 1;
                        bit_string[c] = 1;
                    }
                    else
                    {
                        gaborRealImage[j, i] = 0;
                        iriscodePGM[j, b] = 0;
                        //ptr_iriscode->bit[c] = 0;
                        bit_string[c] = 0;
                    }
                    //			printf("%d ",ptr_iriscode->bit[c]);
                    c++;
                    b++;
                    if (new_pixel_imag[j, i] >= 0)
                    {
                        gaborImagImage[j, i] = 255;
                        iriscodePGM[j, b] = 255;
                        //ptr_iriscode->bit[c] = 1;
                        bit_string[c] = 1;
                    }
                    else
                    {
                        gaborImagImage[j, i] = 0;
                        iriscodePGM[j, b] = 0;
                        //ptr_iriscode->bit[c] = 0;
                        bit_string[c] = 0;
                    }
                    c++;
                }
                b = 0;
            }

            c = 0;
            for (j = 0; j < radius; j++)
            {
                for (i = 0; i < theta; i++)
                {
                    if (unwrappedImage.Pixels[j, i] == 0)
                    {
                        //ptr_iriscode->mask[c] = 0;
                        bit_mask_string[c] = 0;
                        c++;
                        //ptr_iriscode->mask[c] = 0;
                        bit_mask_string[c] = 0;
                        c++;
                    }
                    else
                    {
                        //ptr_iriscode->mask[c] = 1;
                        bit_mask_string[c] = 1;
                        c++;
                        //ptr_iriscode->mask[c] = 1;
                        bit_mask_string[c] = 1;
                        c++;
                    }
                }
            }

            irisCode.bit = bit_string;
            irisCode.mask = bit_mask_string;

            //PGM_Iris pgmWriter1 = new PGM_Iris(realImage.GetLength(1), realImage.GetLength(0));
            //pgmWriter1.WriteToPath(fileName+"\\ptr_real_image.pgm", realImage);
            //pgmWriter1.WriteToPath(fileName+"\\ptr_imag_image.pgm", imagImage);
            //pgmWriter1.WriteToPath("ptr_real_image.pgm", realImage);
            //pgmWriter1.WriteToPath("ptr_imag_image.pgm", imagImage);

            //PGM_Iris pgmWriter2 = new PGM_Iris(gaborRealImage.GetLength(1), gaborRealImage.GetLength(0));
            //pgmWriter2.WriteToPath(fileName+"\\ptr_gabor_real_image.pgm", gaborRealImage);
            //pgmWriter2.WriteToPath(fileName+"\\ptr_gabor_imag_image.pgm", gaborImagImage);
            //pgmWriter2.WriteToPath("ptr_gabor_real_image.pgm", gaborRealImage);
            //pgmWriter2.WriteToPath("ptr_gabor_imag_image.pgm", gaborImagImage);

            //PGM_Iris pgmWriter3 = new PGM_Iris(iriscodePGM.GetLength(1), iriscodePGM.GetLength(0));
            //pgmWriter2.WriteToPath(fileName + "\\iriscode.pgm", iriscodePGM);
            //pgmWriter2.WriteToPath("iriscode.pgm", iriscodePGM);

            return irisCode;
        }

        public IrisCode PerformGaborFilterNoWrite()
        {
            int x, y;

            //float sigma;
            //float u0 = 8;
            float u0 = 17.8f;
            float v0 = 2.23f;
            //float v0 = 1;

            int j, i;//, k, l;

            //float[,] matrix2D = gaussian2Delliptical(gauss_radius, 5f, 4f, 0);
            float[,] matrix2D = gaussian2Delliptical(gauss_radius, 1.85f, 1.48f, 0);

            float[,] real = new float[rows, rows];
            float[,] imag = new float[rows, rows];
            float calc = 0;

            for (j = 0; j < g_size; j++)
            {
                for (i = 0; i < g_size; i++)
                {
                    calc = (float)(-2 * Math.PI * (u0 * (i - x0) + v0 * (j - y0)) * Math.PI / 180);
                    real[j, i] = (float)Math.Cos(calc);
                    imag[j, i] = (float)Math.Sin(calc);
                }
            }

            float[,] real_gabor = new float[rows, rows];
            float[,] imag_gabor = new float[rows, rows];

            for (j = 0; j < g_size; j++)
            {
                for (i = 0; i < g_size; i++)
                {
                    real_gabor[j, i] = real[j, i] * matrix2D[j, i];
                }
            }

            for (j = 0; j < g_size; j++)
            {
                for (i = 0; i < g_size; i++)
                {
                    imag_gabor[j, i] = imag[j, i] * matrix2D[j, i];
                }
            }

            float[,] real_gabor_scaled = new float[rows, rows];
            float[,] imag_gabor_scaled = new float[rows, rows];

            for (j = 0; j < g_size; j++)
                for (i = 0; i < g_size; i++)
                {
                    real_gabor_scaled[j, i] = real_gabor[j, i];
                    imag_gabor_scaled[j, i] = imag_gabor[j, i];
                }

            IntensityTransformation.scaling(real_gabor_scaled, g_size, g_size);
            IntensityTransformation.scaling(imag_gabor_scaled, g_size, g_size);


            int[,] realImage = new int[rows, rows];
            int[,] imagImage = new int[rows, rows];

            for (j = 0; j < g_size; j++)
            {
                for (i = 0; i < g_size; i++)
                {
                    realImage[j, i] = (int)real_gabor_scaled[j, i];
                    imagImage[j, i] = (int)imag_gabor_scaled[j, i];
                }
            }

            float[,] new_pixel_real = new float[radius, theta];
            float[,] new_pixel_imag = new float[radius, theta];
            for (j = 0; j < radius; j++)
                for (i = 0; i < theta; i++)
                {
                    new_pixel_real[j, i] = 0;
                    new_pixel_imag[j, i] = 0;
                }

            int[,] paddedImage = padding(r);

            for (j = r; j < paddedImage.GetLength(0) - r; j++)
            {
                for (i = r; i < paddedImage.GetLength(1) - r; i++)
                {
                    for (y = -r; y <= r; y++)
                    {
                        for (x = -r; x <= r; x++)
                        {
                            new_pixel_real[j - r, i - r] += real_gabor[y + r, x + r] * paddedImage[j + y, i + x];
                            new_pixel_imag[j - r, i - r] += imag_gabor[y + r, x + r] * paddedImage[j + y, i + x];
                        }
                    }
                }
            }

            //int newHeight = radius;
            //int newWidth = theta;

            int[,] gaborRealImage = new int[radius, theta];
            int[,] gaborImagImage = new int[radius, theta];

            for (j = 0; j < radius; j++)
                for (i = 0; i < theta; i++)
                {
                    if (new_pixel_real[j, i] >= 0)
                        gaborRealImage[j, i] = 255;
                    else
                        gaborRealImage[j, i] = 0;
                }

            for (j = 0; j < radius; j++)
                for (i = 0; i < theta; i++)
                {
                    if (new_pixel_imag[j, i] >= 0)
                    {
                        gaborImagImage[j, i] = 255;
                    }
                    else
                    {
                        gaborImagImage[j, i] = 0;
                    }
                }

            IrisCode irisCode = new IrisCode();
            irisCode.size = (radius * theta * 2);
            byte[] bit_string = new byte[(radius * theta * 2)];
            byte[] bit_mask_string = new byte[(radius * theta * 2)];


            int c = 0;
            int b = 0;
            int[,] iriscodePGM = new int[radius, theta * 2];

            //printf("iriscode :\n");
            for (j = 0; j < radius; j++)
            {
                for (i = 0; i < theta; i++)
                {
                    if (new_pixel_real[j, i] >= 0)
                    {
                        gaborRealImage[j, i] = 255;
                        iriscodePGM[j, b] = 255;
                        //ptr_iriscode->bit[c] = 1;
                        bit_string[c] = 1;
                    }
                    else
                    {
                        gaborRealImage[j, i] = 0;
                        iriscodePGM[j, b] = 0;
                        //ptr_iriscode->bit[c] = 0;
                        bit_string[c] = 0;
                    }
                    //			printf("%d ",ptr_iriscode->bit[c]);
                    c++;
                    b++;
                    if (new_pixel_imag[j, i] >= 0)
                    {
                        gaborImagImage[j, i] = 255;
                        iriscodePGM[j, b] = 255;
                        //ptr_iriscode->bit[c] = 1;
                        bit_string[c] = 1;
                    }
                    else
                    {
                        gaborImagImage[j, i] = 0;
                        iriscodePGM[j, b] = 0;
                        //ptr_iriscode->bit[c] = 0;
                        bit_string[c] = 0;
                    }
                    c++;
                }
                b = 0;
            }

            c = 0;
            for (j = 0; j < radius; j++)
            {
                for (i = 0; i < theta; i++)
                {
                    if (unwrappedImage.Pixels[j, i] == 0)
                    {
                        //ptr_iriscode->mask[c] = 0;
                        bit_mask_string[c] = 0;
                        c++;
                        //ptr_iriscode->mask[c] = 0;
                        bit_mask_string[c] = 0;
                        c++;
                    }
                    else
                    {
                        //ptr_iriscode->mask[c] = 1;
                        bit_mask_string[c] = 1;
                        c++;
                        //ptr_iriscode->mask[c] = 1;
                        bit_mask_string[c] = 1;
                        c++;
                    }
                }
            }

            irisCode.bit = bit_string;
            irisCode.mask = bit_mask_string;

            return irisCode;
        }

        public float[,] gaussian2Delliptical(float radius, float sigmax, float sigmay, int rotation)
        {
            float[,] matrix = new float[rows, rows];
            for (int j = 0; j < rows; j++)
                for (int i = 0; i < rows; i++)
                    matrix[j, i] = 0;

            int r = (int)radius;
            int rowsgaus = r * 2 + 1;

            int col, row;
            float total = 0;

            for (col = -r; col <= r; col++)
            {
                for (row = -r; row <= r; row++)
                {
                    float distance_row = row * row;
                    float distance_col = col * col;
                    float xg = (float)(Math.Abs(row) * Math.Cos(rotation * Math.PI / 180) - Math.Abs(col) * Math.Sin(rotation * Math.PI / 180));
                    float yg = (float)(Math.Abs(row) * Math.Sin(rotation * Math.PI / 180) + Math.Abs(col) * Math.Cos(rotation * Math.PI / 180));

                    if ((row * col) > Math.Pow(radius, 2))//if (distance_row > Math.Pow(radius, 2) || distance_col > Math.Pow(radius, 2))//if (distance > pow(radius, 2))
                        matrix[col + r, row + r] = 0;
                    else
                        matrix[col + r, row + r] = (float)(Math.Exp(-((xg * xg) / (sigmax * sigmax) + (yg * yg) / (sigmay * sigmay))) / (2 * Math.PI * sigmax * sigmay));

                    total += matrix[col + r, row + r];
                }
            }

            for (int j = 0; j < rowsgaus; j++)
            {
                for (int i = 0; i < rowsgaus; i++)
                {
                    matrix[j, i] /= total;
                }
            }

            return matrix;
        }

        public int[,] padding(int pad_size)
        {
            int new_height = radius + pad_size * 2;
            int new_width = theta + pad_size * 2;

            int height = unwrappedImageReal.Size.Height;
            int width = unwrappedImageReal.Size.Width;

            int[,] paddedImage = new int[radius + r * 2, theta + r * 2];

            int j, i;

            for (j = pad_size; j < new_height - pad_size; j++)
                for (i = pad_size; i < new_width - pad_size; i++)
                    paddedImage[j, i] = unwrappedImageReal.Pixels[j - pad_size, i - pad_size];

            return paddedImage;
        }
    }
}
