using AForge.Imaging.Filters;
using BiometriBPPT.bppt.ptik.biometric.entity;
using BiometriBPPT.bppt.ptik.biometric.image;
using BiometriBPPT.bppt.ptik.biometric.iris;
using BiometriBPPT.bppt.ptik.biometric.utility;
using BPPT_Iris_SDK;
using BPPT_Iris_SDK.id.go.bppt.biometri.iris.image;
using BPPT_Iris_SDK.id.go.bppt.biometri.iris.processing;
using Testing;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace AttendanceSystem
{
    public partial class AttendanceSystem : Form
    {
        private string employeeNumber;
        private string folderResult;
        private string folderPath;
        private string filePath_left;
        private string filePath_right;
        private string fileName_left;
        private string fileName_right;

        PGM_Iris g_Pgm, g_PgmLeft, g_PgmRight;
        Bitmap g_BmpInputImage, g_BmpLeftImage, g_BmpRightImage, g_BmpPupil, g_BmpIris, g_BmpEyelid, g_BmpCombine;
        //private File g_File;
        private int[,] g_Pixel1;
        private int[,] g_Pixel2;
        private int[,] g_Pixel3;
        private int[,] g_PixelTest;
        private int g_ImageHeight;
        private int g_ImageWidth;
        private int g_PosWidth, g_PosHeight, g_PosWidth2, g_PosHeight2;
        private String g_FilePath;
        private String g_FileName;
        private String g_Extension;
        private PGM_Iris pgmWriter;
        private int center_x;
        private int center_y;
        private int rotatedCenterX;
        private int rotatedCenterY;
        private int radius1;
        private int radius2;
        private int rotatedRadius1;
        private int rotatedRadius2;

        private float HD_Left;
        private float HD_Right;

        MyImage myImageORI;

        private IrisCode irisCode;

        public AttendanceSystem()
        {
            InitializeComponent();

            labelResultTitle.Text = "";
        }

        private void ConvertToPGM(bool left, string path)
        {
            PGMConverter pgmConverter = new PGMConverter(path);
            pgmConverter.ConvertToPGM();

            if (left)
            {
                filePath_left = pgmConverter.PgmFilePath + pgmConverter.PgmFileName;
                fileName_left = pgmConverter.PgmFileName;
            }
            else
            {
                filePath_right = pgmConverter.PgmFilePath + pgmConverter.PgmFileName;
                fileName_right = pgmConverter.PgmFileName;
            }
        }

        private void LoadImageFile()
        {
            g_PgmLeft = new PGM_Iris(filePath_left);
            g_BmpLeftImage = PGM_Iris.CreateBitmap(g_PgmLeft);

            g_PgmRight = new PGM_Iris(filePath_right);
            g_BmpRightImage = PGM_Iris.CreateBitmap(g_PgmRight);

            pictureBoxLeftImage.Image = g_BmpLeftImage;
            pictureBoxRightImage.Image = g_BmpRightImage;
        }

        private int horizontalProjection(int width, int height, int[,] pixel)
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

        private int[] horizontalProjection(int[,] pixel)
        {
            int[] horizontal = new int[g_ImageWidth];
            int[] pos = new int[2];
            for (int i = 0; i < g_ImageHeight; i++)
            {
                for (int j = 0; j < g_ImageWidth; j++)
                {
                    horizontal[j] += pixel[i, j];
                }
            }
            int max = 255 * g_ImageHeight;
            for (int i = 0; i < g_ImageWidth; i++)
            {
                if (horizontal[i] != max)
                {
                    pos[0] = i;
                    break;
                }
            }
            for (int i = g_ImageWidth - 1; i >= 0; i--)
            {
                if (horizontal[i] != max)
                {
                    pos[1] = i;
                    break;
                }
            }

            return pos;
        }

        private int[] specialHorizontalProjection(int width, int height, int[,] pixel)
        {
            int[] horizontal = new int[width];
            int[] pos = new int[2];
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    horizontal[j] += pixel[i, j];
                }
            }
            int max = 255 * height;
            for (int i = 0; i < width; i++)
            {
                if (horizontal[i] != max)
                {
                    pos[0] = i;
                    break;
                }
            }
            for (int i = width - 1; i >= 0; i--)
            {
                if (horizontal[i] != max)
                {
                    pos[1] = i;
                    break;
                }
            }

            return pos;
        }

        private int[,] horizontalROI(int width, int height, int[,] pixel, int pos)
        {
            int pos1 = pos - 70;
            int pos2 = pos + 70;
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    if (j > pos1 && j < pos2)
                    {

                    }
                    else
                    {
                        pixel[i, j] = 0;
                    }
                }
            }

            return pixel;
        }

        private int verticalProjection(int width, int height, int[,] pixel)
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

        public int[] verticalProjection(int[,] pixel)
        {
            int[] vertical = new int[g_ImageHeight];
            int[] pos = new int[2];
            for (int i = 0; i < g_ImageHeight; i++)
            {
                for (int j = 0; j < g_ImageWidth; j++)
                {
                    vertical[i] += pixel[i, j];
                }
            }
            int max = 255 * g_ImageWidth;
            for (int i = 0; i < g_ImageHeight; i++)
            {
                if (vertical[i] != max)
                {
                    pos[0] = i;
                    break;
                }
            }
            for (int i = g_ImageHeight - 1; i >= 0; i--)
            {
                if (vertical[i] != max)
                {
                    pos[1] = i;
                    break;
                }
            }

            return pos;
        }

        public int[] specialVerticalProjection(int width, int height, int[,] pixel)
        {
            int[] vertical = new int[height];
            int[] pos = new int[2];
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    vertical[i] += pixel[i, j];
                }
            }
            int max = 255 * width;
            for (int i = 0; i < height; i++)
            {
                if (vertical[i] != max)
                {
                    pos[0] = i;
                    break;
                }
            }
            for (int i = height - 1; i >= 0; i--)
            {
                if (vertical[i] != max)
                {
                    pos[1] = i;
                    break;
                }
            }

            return pos;
        }

        private void generateImageInitiatePoint(String filename)
        {
            PGM_Iris pgmIris = new PGM_Iris(g_FilePath);
            pgmIris.WriteToPath(filename + ".pgm");
        }

        private double distanceCenter(int j, int i, int center_j, int center_i)
        {
            return Math.Sqrt(((j - center_j) * (j - center_j)) + ((i - center_i) * (i - center_i)));
        }

        private double gradMag(int[,] pix2, int j, int i)
        {
            double sobel1 = (1 * pix2[j - 1, i - 1] + 2 * pix2[j - 1, i] + 1 * pix2[j - 1, i + 1] + 0 * pix2[j, i - 1] + 0 * pix2[j, i] + 0 * pix2[j, i + 1] - 1 * pix2[j + 1, i - 1] - 2 * pix2[j + 1, i] - 1 * pix2[j + 1, i + 1]) / 1;
            double sobel2 = (-1 * pix2[j - 1, i - 1] + 0 * pix2[j - 1, i] + pix2[j - 1, i + 1] + -2 * pix2[j, i - 1] + 0 * pix2[j, i] + 2 * pix2[j, i + 1] - 1 * pix2[j + 1, i - 1] + 0 * pix2[j + 1, i] + pix2[j + 1, i + 1]) / 1;
            //System.out.println(Math.atan(sobel1/sobel2));
            return Math.Sqrt((sobel1 * sobel1) + (sobel2 * sobel2));
        }

        private int[,] combineImage(int[,] pixel1, int[,] pixel2)
        {
            int[,] temp_pixel2 = new int[pixel1.GetLength(0), pixel1.GetLength(1)];

            for (int j = 0; j < pixel1.GetLength(0); j++)
            {
                for (int i = 0; i < pixel1.GetLength(1); i++)
                {
                    if (pixel2[j, i] == 255)
                    {
                        temp_pixel2[j, i] = 255;
                    }
                    else
                    {
                        temp_pixel2[j, i] = pixel1[j, i];
                    }
                }
            }

            return temp_pixel2;
        }

        private int[,] combineImage(int[,] pixel1, int[,] pixel2, int[,] pixel3)
        {
            int[,] temp_pixel2 = new int[pixel1.GetLength(0), pixel1.GetLength(1)];

            for (int j = 0; j < pixel1.GetLength(0); j++)
            {
                for (int i = 0; i < pixel1.GetLength(1); i++)
                {
                    if (pixel2[j, i] == 255 || pixel3[j, i] == 255)
                    {
                        temp_pixel2[j, i] = 255;
                    }
                    else
                    {
                        temp_pixel2[j, i] = pixel1[j, i];
                    }
                }
            }

            return temp_pixel2;
        }

        private void dilationContour(int[,] temp_pixel, int[,] pixel2, int iteration, int width_center, int height_center, double threshold, String type)
        {
            int[,] temp_pixel2 = new int[g_ImageHeight, g_ImageWidth];
            int[,] temp_pixel3 = new int[g_ImageHeight, g_ImageWidth];

            int lim1 = 0;
            int lim2 = 0;
            if (string.Equals(type, "iris", StringComparison.OrdinalIgnoreCase))
            {
                temp_pixel2[height_center, width_center] = 128;
                lim1 = 0;
                lim2 = 100;
            }
            else if (string.Equals(type, "pupil", StringComparison.OrdinalIgnoreCase))
            {
                temp_pixel2[height_center, width_center] = 128;
                lim1 = 0;
                lim2 = 50;
            }

            int min_pos_x = 0;
            int max_pos_x = 0;
            int min_pos_y = 0;
            int max_pos_y = 0;
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
                    }
                }

                if (string.Equals(type, "pupil", StringComparison.OrdinalIgnoreCase))
                {
                    if ((l + 1) >= iteration)
                    {
                        int pupil_diameter = max_pos_y - min_pos_y;
                        g_PosWidth = (int)(min_pos_x + max_pos_x) / 2;
                        g_PosHeight = (int)((min_pos_y + max_pos_y) / 2);
                        g_PosWidth2 = (int)(min_pos_x + max_pos_x) / 2;
                        g_PosHeight2 = (int)((min_pos_y + max_pos_y) / 2) + (pupil_diameter / 2) + 10;

                        bool finish = false;
                        int i = 1;
                        int tmp_x = g_PosWidth2;
                        int tmp_y = g_PosHeight2;

                        if (gradMag(temp_pixel, g_PosHeight2, g_PosWidth2) > threshold)
                        {
                            while (!finish)
                            {
                                if ((g_PosHeight2 - i) > (min_pos_y + max_pos_y))
                                {
                                    if (gradMag(temp_pixel, g_PosHeight2 - i, g_PosWidth2) <= threshold)
                                    {
                                        g_PosHeight2 -= i;
                                        finish = true;
                                        break;
                                    }
                                }
                                if (!finish && (g_PosHeight2 + i) < g_ImageHeight)
                                {
                                    if (gradMag(temp_pixel, g_PosHeight2 + i, g_PosWidth2) <= threshold)
                                    {
                                        g_PosHeight2 += i;
                                        finish = true;
                                        break;
                                    }
                                }
                                if (!finish && (g_PosWidth2 - i) > (min_pos_x + min_pos_y))
                                {
                                    if (gradMag(temp_pixel, g_PosHeight2, g_PosWidth2 - i) <= threshold)
                                    {
                                        g_PosWidth2 -= i;
                                        finish = true;
                                        break;
                                    }
                                }
                                if (!finish && (g_PosWidth2 + i) < g_ImageWidth)
                                {
                                    if (gradMag(temp_pixel, g_PosHeight2, g_PosWidth2 + i) <= threshold)
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

                pgmWriter.WriteToPath(folderResult + "\\anim_black_" + type + ".pgm", temp_pixel2);

            }
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

        private void RunPreprocessing(bool left, PGM_Iris tmpPGM)
        {
            if (left)
            {
                if (!System.IO.File.Exists(employeeNumber + "\\" + "left"))
                {
                    System.IO.Directory.CreateDirectory(employeeNumber + "\\" + "left");
                }

                folderResult = employeeNumber + "\\left";
                g_FilePath = filePath_left;
                g_FileName = fileName_left;
            }
            else
            {
                if (!System.IO.File.Exists(employeeNumber + "\\" + "right"))
                {
                    System.IO.Directory.CreateDirectory(employeeNumber + "\\" + "right");
                }

                folderResult = employeeNumber + "\\right";
                g_FilePath = filePath_right;
                g_FileName = fileName_right;
            }

            PGM_Iris oriImg = tmpPGM;
            pgmWriter = new PGM_Iris(oriImg.Size.Width, oriImg.Size.Height);

            int[,] contrastStretching = IntensityTransformation.ContrastStretching(oriImg.Pixels, 44);
            pgmWriter.WriteToPath(folderResult + "\\ori_contrast_stretching.pgm", contrastStretching);

            g_Pgm = new PGM_Iris(folderResult + "\\ori_contrast_stretching.pgm");
            g_BmpInputImage = PGM_Iris.CreateBitmap(g_Pgm);
            g_Extension = g_Pgm.Extension;
            g_ImageHeight = g_Pgm.Size.Height;
            g_ImageWidth = g_Pgm.Size.Width;
            g_Pixel1 = g_Pgm.Pixels;

            g_PosWidth = g_PosHeight = g_PosWidth2 = g_PosHeight2 = 0;
        }

        private void RunLocalization()
        {
            int[,] temp = new int[g_ImageHeight, g_ImageWidth];
            g_Pixel2 = new int[g_ImageHeight, g_ImageWidth];
            g_Pixel3 = new int[g_ImageHeight, g_ImageWidth];
            for (int i = 0; i < g_ImageHeight; i++)
            {
                for (int j = 0; j < g_ImageWidth; j++)
                {
                    temp[i, j] = g_Pixel1[i, j];
                    g_Pixel2[i, j] = g_Pixel1[i, j];
                    g_Pixel3[i, j] = g_Pixel1[i, j];
                }
            }
           
                g_PosWidth = horizontalProjection(g_ImageWidth, g_ImageHeight, temp);
                temp = horizontalROI(g_ImageWidth, g_ImageHeight, temp, g_PosWidth);
                g_PosHeight = verticalProjection(g_ImageWidth, g_ImageHeight, temp);

                bool finish = false;
                int p = 1;
                int tmp_a = g_PosWidth2;
                int tmp_b = g_PosHeight2;

                if (g_Pixel1[g_PosHeight, g_PosWidth] > 17 || gradMag(g_Pixel2, g_PosHeight, g_PosWidth) > 45)
                {
                    while (!finish)
                    {
                        if ((g_PosHeight - p) > 10)
                        {
                            if (g_Pixel1[g_PosHeight - p, g_PosWidth] <= 17 && gradMag(g_Pixel2, g_PosHeight - p, g_PosWidth) <= 45)
                            {
                                g_PosHeight -= p;
                                finish = true;
                                break;
                            }
                        }
                        if (!finish && (g_PosHeight + p) < g_ImageHeight - 10)
                        {
                            if (g_Pixel1[g_PosHeight + p, g_PosWidth] <= 17 && gradMag(g_Pixel2, g_PosHeight + p, g_PosWidth) <= 45)
                            {
                                g_PosHeight += p;
                                finish = true;
                                break;
                            }
                        }
                        if (!finish && (g_PosWidth - p) > 10)
                        {
                            if (g_Pixel1[g_PosHeight, g_PosWidth - p] <= 17 && gradMag(g_Pixel2, g_PosHeight, g_PosWidth - p) <= 45)
                            {
                                g_PosWidth -= p;
                                finish = true;
                                break;
                            }
                        }
                        if (!finish && (g_PosWidth + p) < g_ImageWidth - 10)
                        {
                            if (g_Pixel1[g_PosHeight, g_PosWidth + p] <= 17 && gradMag(g_Pixel2, g_PosHeight, g_PosWidth + p) <= 45)
                            {
                                g_PosWidth += p;
                                finish = true;
                                break;
                            }
                        }
                        if ((g_PosHeight - p) < 10 && (g_PosHeight + p) > g_ImageHeight - 10 && (g_PosWidth - p) < 10 && (g_PosWidth + p) > g_ImageWidth - 10)
                        {
                            finish = true;
                        }

                        p++;
                    }
                }
                g_PosHeight2 = g_PosHeight + 40;
                g_PosWidth2 = g_PosWidth;

            generateImageInitiatePoint(folderResult + "\\initial_point");

            dilationContour(g_Pixel2, g_Pixel3, 160, g_PosWidth, g_PosHeight, 45, "pupil");

            for (int i = 0; i < g_ImageHeight; i++)
            {
                for (int j = 0; j < g_ImageWidth; j++)
                {
                    temp[i, j] = g_Pixel1[i, j];
                    g_Pixel2[i, j] = g_Pixel1[i, j];
                    g_Pixel3[i, j] = g_Pixel1[i, j];
                }
            }

            dilationContour(g_Pixel2, g_Pixel3, 160, g_PosWidth2, g_PosHeight2, 45, "iris");

            try
            {
                Otsu a1 = new Otsu(new PGM_Iris(folderResult + "\\anim_black_iris.pgm").Pixels);
                int[,] temp_pixel = a1.runOtsu();
                pgmWriter.WriteToPath(folderResult + "\\anim_black_iris_otsu.pgm", temp_pixel);
                a1 = new Otsu(new PGM_Iris(folderResult + "\\anim_black_pupil.pgm").Pixels);
                temp_pixel = a1.runOtsu();
                pgmWriter.WriteToPath(folderResult + "\\anim_black_pupil_otsu.pgm", temp_pixel);
            }
            catch (Exception ex)
            {
            }

            try
            {
                ImageRepresentation asd = new ImageRepresentation(new PGM_Iris(folderResult + "\\anim_black_iris_otsu.pgm").Pixels);
                int[,] pixel2 = asd.FourierDescriptorRepresentation(5, "original");
                pgmWriter.WriteToPath(folderResult + "\\ori_border_iris.pgm", pixel2);

                asd = new ImageRepresentation(new PGM_Iris(folderResult + "\\anim_black_pupil_otsu.pgm").Pixels);
                pixel2 = asd.FourierDescriptorRepresentation(5, "original");
                pgmWriter.WriteToPath(folderResult + "\\ori_border_pupil.pgm", pixel2);
            }
            catch (Exception ex)
            {
            }

            int[,] iniPointPupil = new int[g_ImageHeight, g_ImageWidth];
            int[,] iniPointIris = new int[g_ImageHeight, g_ImageWidth];
            for (int i = 0; i < 240; i++)
            {
                for (int j = 0; j < 320; j++)
                {
                    if (i == g_PosHeight && j == g_PosWidth)
                    {
                        iniPointPupil[i, j] = 255;
                    }
                    else
                    {
                        iniPointPupil[i, j] = 0;
                    }

                    if (i == g_PosHeight2 && j == g_PosWidth2)
                    {
                        iniPointIris[i, j] = 255;
                    }
                    else
                    {
                        iniPointIris[i, j] = 0;
                    }
                }
            }

            pgmWriter.WriteToPath(folderResult + "\\initial_point_pupil.pgm", iniPointPupil);
            pgmWriter.WriteToPath(folderResult + "\\initial_point_iris.pgm", iniPointIris);

            int[,] superimposePupil = combineImage(new PGM_Iris(g_FilePath).Pixels, new PGM_Iris(folderResult + "\\initial_point_pupil.pgm").Pixels);
            pgmWriter.WriteToPath(folderResult + "\\superimpose_pupil.pgm", superimposePupil);

            int[,] superimposeIris = combineImage(new PGM_Iris(g_FilePath).Pixels, new PGM_Iris(folderResult + "\\initial_point_iris.pgm").Pixels);
            pgmWriter.WriteToPath(folderResult + "\\superimpose_iris.pgm", superimposeIris);
        }

        private void RunPreSegmentation()
        {
            PGM_Iris pgmIris = new PGM_Iris(folderResult + "\\ori_border_pupil.pgm");
            Bitmap img = PGM_Iris.CreateBitmap(pgmIris);

            pgmIris = new PGM_Iris(folderResult + "\\ori_border_iris.pgm");
            img = PGM_Iris.CreateBitmap(pgmIris);

            try
            {
                int M = 0;

                ImageRepresentation imgRepresentation;

                try
                {
                    imgRepresentation = new ImageRepresentation(new PGM_Iris(folderResult + "\\anim_black_iris_otsu.pgm").Pixels);
                    int[,] pixel2 = imgRepresentation.FourierDescriptorRepresentation(17, "original");
                    pgmWriter.WriteToPath(folderResult + "\\otp_iris.pgm", pixel2);
                }
                catch (Exception ex)
                {
                }

                try
                {
                    imgRepresentation = new ImageRepresentation(new PGM_Iris(folderResult + "\\anim_black_pupil_otsu.pgm").Pixels);
                    int[,] pixel2 = imgRepresentation.FourierDescriptorRepresentation(5, "original");
                    pgmWriter.WriteToPath(folderResult + "\\otp_pupil.pgm", pixel2);
                }
                catch (Exception ex)
                {
                }
            }
            catch (Exception e)
            {

            }

            try
            {
                int[,] pixel2 = combineImage(new PGM_Iris(g_FilePath).Pixels, new PGM_Iris(folderResult + "\\ori_border_iris.pgm").Pixels, new PGM_Iris(folderResult + "\\ori_border_pupil.pgm").Pixels);
                pgmWriter.WriteToPath(folderResult + "\\combine_ori.pgm", pixel2);
                pixel2 = combineImage(new PGM_Iris(g_FilePath).Pixels, new PGM_Iris(folderResult + "\\otp_iris.pgm").Pixels, new PGM_Iris(folderResult + "\\otp_pupil.pgm").Pixels);
                pgmWriter.WriteToPath(folderResult + "\\combine_fd.pgm", pixel2);
                pixel2 = combineImage(new PGM_Iris(g_FilePath).Pixels, new PGM_Iris(folderResult + "\\anim_black_pupil_otsu.pgm").Pixels);
                pgmWriter.WriteToPath(folderResult + "\\combine_awal_pupil.pgm", pixel2);
                pixel2 = combineImage(new PGM_Iris(g_FilePath).Pixels, new PGM_Iris(folderResult  + "\\anim_black_iris_otsu.pgm").Pixels);
                pgmWriter.WriteToPath(folderResult + "\\combine_awal_iris.pgm", pixel2);

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        private void RunSegmentation()
        {
            try
            {
                String filePath = folderResult + "\\otp_pupil.pgm";
                String filePath2 = folderResult + "\\otp_iris.pgm";

                PGM_Iris inputImage = new PGM_Iris(filePath);
                PGM_Iris inputImage2 = new PGM_Iris(filePath2);

                int picWidth = inputImage.Size.Width;
                int picHeight = inputImage.Size.Height;
                int maxValue = inputImage.MaxValue;

                int[,] data2D = inputImage.Pixels;
                int[,] data2D2 = inputImage2.Pixels;

                ErotionDilation asd = new ErotionDilation();
                int[,] pixel2 = asd.Connect(data2D);
                int[,] pixel3 = asd.Connect(data2D2);

                HoleFilling bsd = new HoleFilling();
                pixel2 = bsd.runHoleFilling(pixel2);
                pixel3 = bsd.runHoleFilling(pixel3);

                pgmWriter.WriteToPath(folderResult + "\\output2_new.pgm", pixel2);
                pgmWriter.WriteToPath(folderResult + "\\output1_new.pgm", pixel3);

            }
            catch (Exception ex)
            {
            }
            // TODO add your handling code here:

            try
            {
                String filePath = folderResult + "\\output1_new.pgm";
                String filePath2 = folderResult + "\\output2_new.pgm";

                PGM_Iris inputImage = new PGM_Iris(filePath);
                PGM_Iris inputImage2 = new PGM_Iris(filePath2);

                int picWidth = inputImage.Size.Width;
                int picHeight = inputImage.Size.Height;
                int maxValue = inputImage.MaxValue;

                int[,] data2D = inputImage.Pixels;
                int[,] data2D2 = inputImage2.Pixels;
                int[,] data2D3 = g_Pgm.Pixels;

                int[,] temp = new int[picHeight, picWidth];

                for (int y = 0; y < picHeight; y++)
                {
                    for (int x = 0; x < picWidth; x++)
                    {
                        if (data2D[y, x] == 0)
                        {
                            if (data2D2[y, x] == 0)
                            {
                                temp[y, x] = 255;
                            }
                            else
                            {
                                temp[y, x] = data2D3[y, x];
                            }
                        }
                        else
                        {
                            temp[y, x] = 255;
                        }

                    }
                }

                pgmWriter.WriteToPath(folderResult + "\\segmented.pgm", temp);
            }
            catch (Exception ex)
            {
            }
        }

        private void RunNormalization()
        {
            try
            {
                String filePath = folderResult + "\\output2_new.pgm";
                String filePath2 = folderResult + "\\segmented.pgm";

                PGM_Iris inputImage = new PGM_Iris(filePath);
                PGM_Iris inputImage2 = new PGM_Iris(filePath2);

                Bitmap imgBmp = PGM_Iris.CreateBitmap(inputImage);
                imgBmp.RotateFlip(RotateFlipType.Rotate270FlipX);
                int[,] segmentedPixel = new int[imgBmp.Width, imgBmp.Height];
                for (int i = 0; i < imgBmp.Width; i++)
                {
                    for (int j = 0; j < imgBmp.Height; j++)
                    {
                        segmentedPixel[i, j] = imgBmp.GetPixel(i, j).ToArgb();
                    }
                }

                pgmWriter.WriteToPath(folderResult + "\\output2_new_rotated.pgm", segmentedPixel);

                PGM_Iris rotatedIris = new PGM_Iris(folderResult + "\\output2_new_rotated.pgm");

                int[,] data2D = inputImage.Pixels;
                int[,] data2D2 = inputImage2.Pixels;
                int[,] data2D3 = rotatedIris.Pixels;

                int[] pos1 = horizontalProjection(data2D);
                int[] pos2 = verticalProjection(data2D);
                int[] rotatedPos1 = horizontalProjection(data2D3);
                int[] rotatedPos2 = verticalProjection(data2D3);

                //radius pupil
                int radius1 = pos1[1] - pos1[0];
                //radius iris
                int radius2 = pos2[1] - pos2[0];

                //rotated radius pupil
                int rotatedRadius1 = rotatedPos1[1] - rotatedPos1[0];
                //rotated radius iris
                int rotatedRadius2 = rotatedPos2[1] - rotatedPos2[0];

                double bla1 = radius1 / 2;
                double bla2 = radius2 / 2;
                double bla3 = rotatedRadius1 / 2;
                double bla4 = rotatedRadius2 / 2;

                int x_center = pos1[0] + ((int)Math.Round(bla1));
                int y_center = pos2[0] + ((int)Math.Round(bla2));
                this.center_x = x_center;
                this.center_y = y_center;
                this.rotatedCenterX = rotatedPos1[0] + ((int)Math.Round(bla3));
                this.rotatedCenterY = rotatedPos2[0] + ((int)Math.Round(bla4));

                int radius = 0;
                if (radius1 > radius2)
                {
                    radius = radius1;
                }
                else
                {
                    radius = radius2;
                }

                int rotatedRadius = 0;
                if (rotatedRadius1 > rotatedRadius2)
                {
                    rotatedRadius = rotatedRadius1;
                }
                else
                {
                    rotatedRadius = rotatedRadius2;
                }

                double bla = radius / 2;
                double blabla = rotatedRadius / 2;

                this.radius1 = ((int)Math.Round(bla) + 2);
                this.radius2 = ((int)Math.Round(bla) + 30);

                this.rotatedRadius1 = ((int)Math.Round(blabla) + 2);
                this.rotatedRadius2 = ((int)Math.Round(blabla) + 30);

                int[,] temp = new int[g_ImageHeight, g_ImageWidth];
                for (int y = 0; y < g_ImageHeight; y++)
                {
                    for (int x = 0; x < g_ImageWidth; x++)
                    {
                        if (distanceCenter(x, y, x_center, y_center) > ((int)Math.Round(bla)) + 2 && distanceCenter(x, y, x_center, y_center) < ((int)Math.Round(bla)) + 30)
                        {
                            temp[y, x] = data2D2[y, x];
                        }
                        else
                        {
                            temp[y, x] = 255;
                        }

                    }
                }

                pgmWriter.WriteToPath(folderResult + "\\normalized.pgm", temp);

                Bitmap rotatedNormalizedImg = PGM_Iris.CreateBitmap(new PGM_Iris(folderResult + "\\normalized.pgm"));
                rotatedNormalizedImg.RotateFlip(RotateFlipType.Rotate270FlipX);
                g_PixelTest = new int[rotatedNormalizedImg.Width, imgBmp.Height];
                for (int i = 0; i < rotatedNormalizedImg.Width; i++)
                {
                    for (int j = 0; j < rotatedNormalizedImg.Height; j++)
                    {
                        g_PixelTest[i, j] = rotatedNormalizedImg.GetPixel(i, j).ToArgb();
                    }
                }
                pgmWriter.WriteToPath(folderResult + "\\rotated_normalized.pgm", g_PixelTest);

                PGM_Iris pgmIris = new PGM_Iris(folderResult + "\\normalized.pgm");
                Bitmap img = PGM_Iris.CreateBitmap(pgmIris);
            }
            catch (Exception ex)
            {
            }
        }

        private void RunFeatureExtraction()
        {
            try
            {
                int[,] data2D = new PGM_Iris(folderResult + "\\normalized.pgm").Pixels;

                int r_pupil = radius1;
                int r_iris = radius2;
                int theta = (int)Math.Floor((Math.PI * 2 * r_iris));

                int center_x = this.center_x;
                int center_y = this.center_y;
                int radius = r_iris - r_pupil;

                int[,] output = new int[radius, theta * 2];
                int[,] output2 = new int[radius, theta];

                for (int x = 0; x < radius; x++)
                {
                    for (int y = 0; y < theta * 2; y++)
                    {
                        double xp_theta = center_x + ((double)r_pupil * Math.Cos(Math.PI * y / 180.0));
                        double yp_theta = center_y - ((double)r_pupil * Math.Sin(Math.PI * y / 180.0));

                        double xl_theta = center_x + ((double)r_iris * Math.Cos(Math.PI * y / 180.0));
                        double yl_theta = center_y - ((double)r_iris * Math.Sin(Math.PI * y / 180.0));

                        int x_r_theta = (int)((1.0 - (double)x / radius) * xp_theta + (double)x / radius * xl_theta);
                        int y_r_theta = (int)((1.0 - (double)x / radius) * yp_theta + (double)x / radius * yl_theta);

                        output[x, y] = data2D[(int)y_r_theta, (int)x_r_theta];
                    }
                }

                for (int x = 0; x < radius; x++)
                {
                    for (int y = 0; y < theta; y++)
                    {
                        output2[x, y] = output[x, y + theta];
                    }
                }

                //output2 = ExtraProcessing.Scaling(output2, radius, theta);

                PGM_Iris specialWriter = new PGM_Iris(theta, radius);
                specialWriter.WriteToPath(folderResult + "\\unwraped.pgm", output2);

                Bitmap oriBmp = PGM_Iris.CreateBitmap(new PGM_Iris(folderResult + "\\unwraped.pgm"));
                PGMConverter resized = new PGMConverter(folderResult + "\\", 293, 28);
                resized.ConvertToPGM(oriBmp);

                output2 = new PGM_Iris(folderResult + "\\unwraped.pgm").Pixels;
                int[,] output3 = IntensityTransformation.ContrastStretching(output2, 10);

                Otsu a1 = new Otsu(output3);
                int[,] temp_pixel = a1.OtsuBinarization(output2);
                temp_pixel = a1.Complement(temp_pixel);
                temp_pixel = a1.CheckOtsu(temp_pixel);

                //specialWriter = new PGM_Iris(theta, radius);
                specialWriter.WriteToPath(folderResult + "\\unwraped_otsu.pgm", temp_pixel);

                PGMConverter pgmConverter = new PGMConverter(folderResult + "\\", 128, 8);
                Bitmap newBmp = pgmConverter.ConvertToBitmap(oriBmp);
                output2 = pgmConverter.ConvertToInt(newBmp);

                specialWriter = new PGM_Iris(output2.GetLength(1), output2.GetLength(0));
                specialWriter.WriteToPath(folderResult + "\\unwraped_128x8.pgm", output2);

                Bitmap oriBmpOtsu = PGM_Iris.CreateBitmap(new PGM_Iris(folderResult + "\\unwraped_otsu.pgm"));
                Bitmap newBmpOtsu = pgmConverter.ConvertToBitmap(oriBmpOtsu);
                int[,] output2Otsu = pgmConverter.ConvertToInt(newBmpOtsu);
                specialWriter.WriteToPath(folderResult + "\\unwraped_128x8_otsu.pgm", output2Otsu);

                GaborFilter gaborFilter = new GaborFilter(folderResult);
                irisCode = gaborFilter.PerformGaborFilter();

            }
            catch (Exception ex)
            {
            }
        }

        private void RunMatching(bool left)
        {
            string matchingFilePath = "C:\\Users\\defianas\\IrisBPPTDatabase\\" + employeeNumber;
            DirectoryInfo folder = new DirectoryInfo(matchingFilePath);
            FileInfo[] listOfFiles = folder.GetFiles("*.pgm");

            string matchingFileName;
            if (left)
            {
                matchingFileName = matchingFilePath + "\\left";
            }
            else
            {
                matchingFileName = matchingFilePath + "\\right";
            }

            GaborFilter gaborFilter = new GaborFilter(matchingFileName);
            IrisCode matchIrisCode = gaborFilter.PerformGaborFilterNoWrite();

            Matching matching = new Matching(irisCode, matchIrisCode);

            if (left)
            {
                HD_Left = matching.PerformMatching();
            }
            else
            {
                HD_Right = matching.PerformMatching();
            }
        }

        private void RecognizeEye(bool left, PGM_Iris eye)
        {
            RunPreprocessing(left, eye);
            RunLocalization();
            RunPreSegmentation();
            RunSegmentation();
            RunNormalization();
            RunFeatureExtraction();
            RunMatching(left);
        }

        private void setImage(bool left, String g_FilePath, PictureBox pb)
        {
            PGM_Iris g_PgmScanner = new PGM_Iris(g_FilePath);
            Bitmap g_BmpInputImage = PGM_Iris.CreateBitmap(g_PgmScanner);
            int g_ImageHeightScanner = g_PgmScanner.Size.Height;
            int g_ImageWidthScanner = g_PgmScanner.Size.Width;
            myImageORI = new MyImage(g_ImageHeightScanner, g_ImageWidthScanner, MyImgType.UCHAR);
            myImageORI.UPixel = Util.SingleToMulti(g_PgmScanner.GetPixelData(), g_ImageHeightScanner, g_ImageWidthScanner);

            if (!System.IO.File.Exists(employeeNumber + "\\" + "left"))
            {
                System.IO.Directory.CreateDirectory(employeeNumber + "\\" + "left");
            }

            if (!System.IO.File.Exists(employeeNumber + "\\" + "right"))
            {
                System.IO.Directory.CreateDirectory(employeeNumber + "\\" + "right");
            }

            string path;
            if (left)
            {
                path = employeeNumber + "\\" + "left";
            }
            else
            {
                path = employeeNumber + "\\" + "right";
            }

            PGMConverter pgmConvert = new PGMConverter(path);
            pgmConvert.ConvertToPGM(g_FilePath, g_BmpInputImage);

            //g_BmpInputImage = BPPT_Iris_SDK.id.go.bppt.biometri.iris.image.utils.PGMUtil.ToBitmap(g_FilePath);

            pb.Image = g_BmpInputImage;
        }

        private void buttonChooseFile_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog openFileDialogImage = new FolderBrowserDialog();
            DialogResult result = openFileDialogImage.ShowDialog();
            if (result == DialogResult.OK)
            {
                folderPath = openFileDialogImage.SelectedPath;
                DirectoryInfo folder = new DirectoryInfo(folderPath);
                FileInfo[] listOfFiles = folder.GetFiles("*.pgm");

                filePath_left = listOfFiles[0].FullName;
                fileName_left = listOfFiles[0].Name;

                filePath_right = listOfFiles[1].FullName;
                fileName_right = listOfFiles[1].Name;

                if (!string.Equals(Path.GetExtension(filePath_left), ".pgm", StringComparison.OrdinalIgnoreCase))
                {
                    ConvertToPGM(true, filePath_left);
                }

                if (!string.Equals(Path.GetExtension(filePath_right), ".pgm", StringComparison.OrdinalIgnoreCase))
                {
                    ConvertToPGM(false, filePath_right);
                }

                LoadImageFile();
            }
        }

        private void buttonScanIris_Click(object sender, EventArgs e)
        {
            employeeNumber = this.textBoxEmployeeNumber.Text;
            frmCapture irisCamera = new frmCapture();
            irisCamera.ShowDialog();

            setImage(true, "lEye.pgm", pictureBoxLeftImage);
            setImage(false, "rEye.pgm", pictureBoxRightImage);

            g_PgmLeft = new PGM_Iris(employeeNumber+"\\left\\lEye.pgm");
            g_PgmRight = new PGM_Iris(employeeNumber+"\\right\\rEye.pgm");
        }

        private void buttonProcess_Click(object sender, EventArgs e)
        {
            employeeNumber = this.textBoxEmployeeNumber.Text;

            labelResultTitle.Text = "Result: ";
            labelResult.Text = "Please Wait...";
            labelResult.Invalidate();
            labelResult.Update();
            labelResult.Refresh();
            Application.DoEvents();

            RecognizeEye(true, g_PgmLeft);
            RecognizeEye(false, g_PgmRight);

            if (HD_Left <= 0.32f && HD_Right <= 0.32f)
            {
                labelResult.Text = "Accepted";
            }
            else
            {
                labelResult.Text = "Rejected";
            }
        }
    }
}
