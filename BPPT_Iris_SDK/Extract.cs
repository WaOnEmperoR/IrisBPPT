using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Imaging;
using System.Diagnostics;
using System.IO;

namespace BPPT_Iris_SDK
{
    public class Extract
    {
        String filePath, folderResult, g_FilePath, g_FileName;
        String fileName, g_Extension;
        PGM_Iris g_Pgm;
        Bitmap g_BmpImage, g_BmpInputImage;
        private Bitmap oriBmp;
        private PGM_Iris oriImg;
        private int width, height;
        int g_ImageHeight, g_ImageWidth, g_PosWidth, g_PosWidth2, g_PosHeight, g_PosHeight2;
        PGM_Iris pgmWriter;
        int[,] g_Pixel1, g_Pixel2, g_Pixel3;
        private int center_x;
        private int center_y;
        private int rotatedCenterX;
        private int rotatedCenterY;
        private int radius1;
        private int radius2;
        private int rotatedRadius1;
        private int rotatedRadius2;

        private IrisCode irisCode;

        public Extract()
        {
               
        }

        public IrisCode doExtract(String par_input)
        {
            irisCode = new IrisCode();

            PGMConverter pgmConverter = new PGMConverter(par_input);
            pgmConverter.ConvertToPGM(par_input);

            filePath = pgmConverter.PgmFilePath + pgmConverter.PgmFileName;
            fileName = pgmConverter.PgmFileName;

            //Console.WriteLine("File Path : " + filePath);
            //Console.WriteLine("File Name : " + fileName);

            g_Pgm = new PGM_Iris(filePath);
            g_BmpImage = PGM_Iris.CreateBitmap(g_Pgm);

            RunPreprocessing(g_Pgm, 9999);
            RunLocalization();
            RunPreSegmentation();
            RunSegmentation();
            RunNormalization();
            RunFeatureExtraction();

            return irisCode;
        }

        public IrisCode doExtract(Bitmap bmp_in)
        {
            irisCode = new IrisCode();

            PGMConverter pgmConverter = new PGMConverter(bmp_in);
            pgmConverter.ConvertToPGM_Awal("input_iris.png", bmp_in);

            //pgmConverter.PgmFileName = Path.GetFileNameWithoutExtension("input_iris.png") + ".pgm";

            //filePath = pgmConverter.PgmFilePath + pgmConverter.PgmFileName;
            //fileName = pgmConverter.PgmFileName;

            filePath = "input_iris.pgm";
            fileName = "input_iris.png";

            //Console.WriteLine("File Path : " + filePath);
            //Console.WriteLine("File Name : " + fileName);

            g_Pgm = new PGM_Iris(filePath);
            g_BmpImage = PGM_Iris.CreateBitmap(g_Pgm);

            RunPreprocessing(g_Pgm, 9999);
            RunLocalization();
            RunPreSegmentation();
            RunSegmentation();
            RunNormalization();
            RunFeatureExtraction();

            return irisCode;
        }

        private void RunPreprocessing(PGM_Iris tmpPGM, int emp_num)
        {
            //Stopwatch stopwatch = new Stopwatch();
            //stopwatch.Start();

            if (!System.IO.File.Exists(emp_num.ToString()))
            {
                System.IO.Directory.CreateDirectory(emp_num.ToString());
            }

            folderResult = emp_num.ToString();
            g_FilePath = filePath;
            g_FileName = fileName;


            PGM_Iris oriImg = tmpPGM;
            pgmWriter = new PGM_Iris(oriImg.Size.Width, oriImg.Size.Height);

            pgmWriter.WriteToPath(folderResult + "\\ori_pre_contrast_stretching.pgm", oriImg.Pixels);

            int[,] contrastStretching = IntensityTransformation.ContrastStretching(oriImg.Pixels, 44);
            pgmWriter.WriteToPath(folderResult + "\\ori_contrast_stretching.pgm", contrastStretching);

            g_Pgm = new PGM_Iris(folderResult + "\\ori_contrast_stretching.pgm");
            g_BmpInputImage = PGM_Iris.CreateBitmap(g_Pgm);
            g_Extension = g_Pgm.Extension;
            g_ImageHeight = g_Pgm.Size.Height;
            g_ImageWidth = g_Pgm.Size.Width;
            g_Pixel1 = g_Pgm.Pixels;

            g_PosWidth = g_PosHeight = g_PosWidth2 = g_PosHeight2 = 0;

            //stopwatch.Stop();
            //Console.WriteLine("Preprocessing: " + stopwatch.ElapsedMilliseconds.ToString());
        }

        private void RunLocalization()
        {
            //Stopwatch stopwatch = new Stopwatch();
            //stopwatch.Start();

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

            g_PosWidth = Projection.horizontalProjection(g_ImageWidth, g_ImageHeight, temp);
            temp = Projection.horizontalROI(g_ImageWidth, g_ImageHeight, temp, g_PosWidth);
            g_PosHeight = Projection.verticalProjection(g_ImageWidth, g_ImageHeight, temp);

            bool finish = false;
            int p = 1;
            int tmp_a = g_PosWidth2;
            int tmp_b = g_PosHeight2;

            int tmp_smallest = g_Pixel1[g_PosHeight, g_PosWidth];
            int tmp_pos_x = g_PosWidth;
            int tmp_pos_y = g_PosHeight;

            if (g_Pixel1[g_PosHeight, g_PosWidth] > 17 || ConditionalDilation.gradMag(g_Pixel2, g_PosHeight, g_PosWidth) > 20)
            {
                while (!finish)
                {
                    //Console.WriteLine(tmp_smallest);
                    if ((g_PosHeight - p) > 10)
                    {
                        if (g_Pixel1[g_PosHeight - p, g_PosWidth] <= 17 && ConditionalDilation.gradMag(g_Pixel2, g_PosHeight - p, g_PosWidth) <= 20)
                        {
                            g_PosHeight -= p;
                            finish = true;
                            break;
                        }
                        if (g_Pixel1[g_PosHeight - p, g_PosWidth] < tmp_smallest && ConditionalDilation.gradMag(g_Pixel2, g_PosHeight - p, g_PosWidth) <= 20)
                        {
                            tmp_smallest = g_Pixel1[g_PosHeight - p, g_PosWidth];
                            tmp_pos_x = g_PosWidth;
                            tmp_pos_y = g_PosHeight - p;
                        }
                    }
                    if (!finish && (g_PosHeight + p) < g_ImageHeight - 10)
                    {
                        if (g_Pixel1[g_PosHeight + p, g_PosWidth] <= 17 && ConditionalDilation.gradMag(g_Pixel2, g_PosHeight + p, g_PosWidth) <= 20)
                        {
                            g_PosHeight += p;
                            finish = true;
                            break;
                        }
                        if (g_Pixel1[g_PosHeight + p, g_PosWidth] < tmp_smallest && ConditionalDilation.gradMag(g_Pixel2, g_PosHeight + p, g_PosWidth) <= 20)
                        {
                            tmp_smallest = g_Pixel1[g_PosHeight + p, g_PosWidth];
                            tmp_pos_x = g_PosWidth;
                            tmp_pos_y = g_PosHeight + p;
                        }
                    }
                    if (!finish && (g_PosWidth - p) > 10)
                    {
                        if (g_Pixel1[g_PosHeight, g_PosWidth - p] <= 17 && ConditionalDilation.gradMag(g_Pixel2, g_PosHeight, g_PosWidth - p) <= 20)
                        {
                            g_PosWidth -= p;
                            finish = true;
                            break;
                        }
                        if (g_Pixel1[g_PosHeight, g_PosWidth - p] < tmp_smallest && ConditionalDilation.gradMag(g_Pixel2, g_PosHeight, g_PosWidth - p) <= 20)
                        {
                            tmp_smallest = g_Pixel1[g_PosHeight, g_PosWidth - p];
                            tmp_pos_x = g_PosWidth - p;
                            tmp_pos_y = g_PosHeight;
                        }
                    }
                    if (!finish && (g_PosWidth + p) < g_ImageWidth - 10)
                    {
                        if (g_Pixel1[g_PosHeight, g_PosWidth + p] <= 17 && ConditionalDilation.gradMag(g_Pixel2, g_PosHeight, g_PosWidth + p) <= 20)
                        {
                            g_PosWidth += p;
                            finish = true;
                            break;
                        }
                        if (g_Pixel1[g_PosHeight, g_PosWidth + p] < tmp_smallest && ConditionalDilation.gradMag(g_Pixel2, g_PosHeight, g_PosWidth + p) <= 20)
                        {
                            tmp_smallest = g_Pixel1[g_PosHeight, g_PosWidth + p];
                            tmp_pos_x = g_PosWidth + p;
                            tmp_pos_y = g_PosHeight;
                        }
                    }
                    if ((g_PosHeight - p) <= 10 && (g_PosHeight + p) >= g_ImageHeight - 10 && (g_PosWidth - p) <= 10 && (g_PosWidth + p) >= g_ImageWidth - 10)
                    {
                        g_PosWidth = tmp_pos_x;
                        g_PosHeight = tmp_pos_y;
                        finish = true;
                    }

                    p++;
                }
            }
            g_PosHeight2 = g_PosHeight + 40;
            g_PosWidth2 = g_PosWidth;
            bool analyzed = false;

            int [,] pixel_analysis = new int[g_ImageHeight, g_ImageWidth];

            ConditionalDilation.dilationContour(g_Pixel2, g_Pixel3, 160, g_PosWidth, g_PosHeight, 20, "pupil", folderResult, pixel_analysis);

            while (pixel_analysis[g_PosHeight2, g_PosWidth2] == 128)
            {
                g_PosHeight2 += 2;
                analyzed = true;
            }

            // Ensuring safety
            if (analyzed)
                g_PosHeight2 += 4;

            //Console.WriteLine("Folder Result : " + folderResult);
            //Projection.generateImageInitiatePoint(folderResult + "\\initial_point", g_FilePath);

            //stopwatch.Stop();
            //Console.WriteLine("Localization: " + stopwatch.ElapsedMilliseconds.ToString());
            
            for (int i = 0; i < g_ImageHeight; i++)
            {
                for (int j = 0; j < g_ImageWidth; j++)
                {
                    temp[i, j] = g_Pixel1[i, j];
                    g_Pixel2[i, j] = g_Pixel1[i, j];
                    g_Pixel3[i, j] = g_Pixel1[i, j];
                }
            }

            ConditionalDilation.dilationContour(g_Pixel2, g_Pixel3, 160, g_PosWidth2, g_PosHeight2, 45, "iris", folderResult, pixel_analysis);

            try
            {
                //Otsu a1 = new Otsu(new PGM_Iris(folderResult + "\\anim_black_iris.pgm").Pixels);
                //int[,] temp_pixel = a1.runOtsu();

                int[,] binarys = new PGM_Iris(folderResult + "\\anim_black_iris.pgm").Pixels;
                int[,] temp_pixel = new int[g_ImageHeight, g_ImageWidth];
                for (int j = 0; j < g_ImageHeight; j++)
                {
                    for (int i = 0; i < g_ImageWidth; i++)
                    {
                        if (binarys[j, i] == 128)
                        {
                            temp_pixel[j, i] = 255;
                        }
                        else
                        {
                            temp_pixel[j, i] = 0;
                        }
                    }
                }
                pgmWriter.WriteToPath(folderResult + "\\anim_black_iris_otsu.pgm", temp_pixel);

                //a1 = new Otsu(new PGM_Iris(folderResult + "\\anim_black_pupil.pgm").Pixels);
                //temp_pixel = a1.runOtsu();

                binarys = new PGM_Iris(folderResult + "\\anim_black_pupil.pgm").Pixels;
                temp_pixel = new int[g_ImageHeight, g_ImageWidth];
                for (int j = 0; j < g_ImageHeight; j++)
                {
                    for (int i = 0; i < g_ImageWidth; i++)
                    {
                        if (binarys[j, i] == 128)
                        {
                            temp_pixel[j, i] = 255;
                        }
                        else
                        {
                            temp_pixel[j, i] = 0;
                        }
                    }
                }

                pgmWriter.WriteToPath(folderResult + "\\anim_black_pupil_otsu.pgm", temp_pixel);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception..." + ex.Message);
            }

            try
            {
                ImageRepresentation_Import asd = new ImageRepresentation_Import(new PGM_Iris(folderResult + "\\anim_black_iris_otsu.pgm").Pixels);
                int[,] pixel2 = asd.FourierDescriptorRepresentation(5, "asd");

                int panjang = pixel2.GetLength(1);
                int lebar = pixel2.GetLength(0);

                for (int i = 0; i < pixel2.GetLength(0); i++)
                {
                    for (int j = 0; j < pixel2.GetLength(1); j++)
                    {
                        pixel2[i, j] = 255 - pixel2[i, j];
                    }
                }

                pgmWriter.WriteToPath(folderResult + "\\ori_border_iris.pgm", pixel2);

                asd = new ImageRepresentation_Import(new PGM_Iris(folderResult + "\\anim_black_pupil_otsu.pgm").Pixels);
                pixel2 = asd.FourierDescriptorRepresentation(17, "asd");

                for (int i = 0; i < pixel2.GetLength(0); i++)
                {
                    for (int j = 0; j < pixel2.GetLength(1); j++)
                    {
                        pixel2[i, j] = 255 - pixel2[i, j];
                    }
                }

                pgmWriter.WriteToPath(folderResult + "\\ori_border_pupil.pgm", pixel2);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception..." + ex.Message);
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

            //int[,] superimposePupil = combineImage(new PGM_Iris(g_FilePath).Pixels, new PGM_Iris(folderResult + "\\initial_point_pupil.pgm").Pixels);
            //pgmWriter.WriteToPath(folderResult + "\\superimpose_pupil.pgm", superimposePupil);

            //int[,] superimposeIris = combineImage(new PGM_Iris(g_FilePath).Pixels, new PGM_Iris(folderResult + "\\initial_point_iris.pgm").Pixels);
            //pgmWriter.WriteToPath(folderResult + "\\superimpose_iris.pgm", superimposeIris);

        }

        private void RunPreSegmentation()
        {
            //Stopwatch stopwatch = new Stopwatch();
            //stopwatch.Start();

            PGM_Iris pgmIris = new PGM_Iris(folderResult + "\\ori_border_pupil.pgm");
            Bitmap img = PGM_Iris.CreateBitmap(pgmIris);

            pgmIris = new PGM_Iris(folderResult + "\\ori_border_iris.pgm");
            img = PGM_Iris.CreateBitmap(pgmIris);

            try
            {
                int M = 0;

                ImageRepresentation_Import imgRepresentation;

                try
                {
                    imgRepresentation = new ImageRepresentation_Import(new PGM_Iris(folderResult + "\\anim_black_iris_otsu.pgm").Pixels);
                    int[,] pixel2 = imgRepresentation.FourierDescriptorRepresentation(17, "asd");

                    for (int i = 0; i < pixel2.GetLength(0); i++)
                    {
                        for (int j = 0; j < pixel2.GetLength(1); j++)
                        {
                            pixel2[i, j] = 255 - pixel2[i, j];
                        }
                    }

                    pgmWriter.WriteToPath(folderResult + "\\otp_iris.pgm", pixel2);
                }
                catch (Exception ex)
                {
                }

                try
                {
                    imgRepresentation = new ImageRepresentation_Import(new PGM_Iris(folderResult + "\\anim_black_pupil_otsu.pgm").Pixels);
                    int[,] pixel2 = imgRepresentation.FourierDescriptorRepresentation(5, "asd");

                    for (int i = 0; i < pixel2.GetLength(0); i++)
                    {
                        for (int j = 0; j < pixel2.GetLength(1); j++)
                        {
                            pixel2[i, j] = 255 - pixel2[i, j];
                        }
                    }

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
                pixel2 = combineImage(new PGM_Iris(g_FilePath).Pixels, new PGM_Iris(folderResult + "\\anim_black_iris_otsu.pgm").Pixels);
                pgmWriter.WriteToPath(folderResult + "\\combine_awal_iris.pgm", pixel2);

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            //stopwatch.Stop();
            //Console.WriteLine("Pre Segmentation: " + stopwatch.ElapsedMilliseconds.ToString());
        }

        private void RunSegmentation()
        {
            //Stopwatch stopwatch = new Stopwatch();
            //stopwatch.Start();

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
                data2D3 = IntensityTransformation.ContrastStretching(g_Pgm.Pixels, 44);

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

            //stopwatch.Stop();
            //Console.WriteLine("Segmentation: " + stopwatch.ElapsedMilliseconds.ToString());
        }

        private void RunNormalization()
        {
            //Stopwatch stopwatch = new Stopwatch();
            //stopwatch.Start();

            try
            {
                String filePath = folderResult + "\\output2_new.pgm";
                String filePath2 = folderResult + "\\segmented.pgm";

                PGM_Iris inputImage = new PGM_Iris(filePath);
                PGM_Iris inputImage2 = new PGM_Iris(filePath2);

                int[,] data2D = inputImage.Pixels;
                int[,] data2D2 = inputImage2.Pixels;


                int[] pos1 = Projection.horizontalProjection(data2D);
                int[] pos2 = Projection.verticalProjection(data2D);

                //radius pupil
                int radius1 = pos1[1] - pos1[0];
                //radius iris
                int radius2 = pos2[1] - pos2[0];

                double bla1 = radius1 / 2;
                double bla2 = radius2 / 2;
                //double bla3 = rotatedRadius1 / 2;
                //double bla4 = rotatedRadius2 / 2;

                int x_center = pos1[0] + ((int)Math.Round(bla1));
                int y_center = pos2[0] + ((int)Math.Round(bla2));
                this.center_x = x_center;
                this.center_y = y_center;

                int radius = 0;
                if (radius1 > radius2)
                {
                    radius = radius1;
                }
                else
                {
                    radius = radius2;
                }

                double bla = radius / 2;

                this.radius1 = ((int)Math.Round(bla) + 2);
                this.radius2 = ((int)Math.Round(bla) + 30);

                int[,] temp = new int[g_ImageHeight, g_ImageWidth];
                for (int y = 0; y < g_ImageHeight; y++)
                {
                    for (int x = 0; x < g_ImageWidth; x++)
                    {
                        if (ConditionalDilation.distanceCenter(x, y, x_center, y_center) > ((int)Math.Round(bla)) + 2 && ConditionalDilation.distanceCenter(x, y, x_center, y_center) < ((int)Math.Round(bla)) + 30)
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
                PGM_Iris pgmIris = new PGM_Iris(folderResult + "\\normalized.pgm");
                Bitmap img = PGM_Iris.CreateBitmap(pgmIris);
            }
            catch (Exception ex)
            {
            }

            //stopwatch.Stop();
            //Console.WriteLine("Normalization: " + stopwatch.ElapsedMilliseconds.ToString());
        }

        private void RunFeatureExtraction()
        {
            //Stopwatch stopwatch = new Stopwatch();
            //stopwatch.Start();

            try
            {
                int[,] data2D = new PGM_Iris(folderResult + "\\normalized.pgm").Pixels;

                int r_pupil = radius1;
                int r_iris = radius2;
                //int theta = (int)Math.Floor((Math.PI * 2 * r_iris));
                int theta = 180;

                int center_x = this.center_x;
                int center_y = this.center_y;
                //int radius = r_iris - r_pupil;
                int radius = 20;

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

                        if (y_r_theta < 0)
                        {
                            y_r_theta = 0;
                        }
                        if (y_r_theta >= data2D.GetLength(0))
                        {
                            y_r_theta = data2D.GetLength(0) - 1;
                        }
                        if (x_r_theta < 0)
                        {
                            x_r_theta = 0;
                        }
                        if (x_r_theta >= data2D.GetLength(1))
                        {
                            x_r_theta = data2D.GetLength(1) - 1;
                        }

                        output[x, y] = data2D[(int)y_r_theta, (int)x_r_theta];
                    }
                }

                for (int x = 0; x < radius; x++)
                {
                    for (int y = 0; y < theta; y++)
                    {
                        output2[x, y] = output[x, y + (theta)];
                    }
                }

                //output2 = ExtraProcessing.Scaling(output2, radius, theta);

                PGM_Iris specialWriter = new PGM_Iris(theta, radius);
                specialWriter.WriteToPath(folderResult + "\\unwraped.pgm", output2);

                Bitmap oriBmp = PGM_Iris.CreateBitmap(new PGM_Iris(folderResult + "\\unwraped.pgm"));
                PGMConverter resized = new PGMConverter(folderResult + "\\", 293, 28);
                resized.ConvertToPGM(oriBmp);

                output2 = new PGM_Iris(folderResult + "\\unwraped.pgm").Pixels;
                int[,] output3 = IntensityTransformation.ContrastStretching(output2, 0);

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

            //stopwatch.Stop();
            //Console.WriteLine("Feature Extraction: " + stopwatch.ElapsedMilliseconds.ToString());
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

    }
}
