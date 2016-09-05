using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BiometriBPPT.bppt.ptik.biometric.entity;
using BiometriBPPT.bppt.ptik.biometric.image;
using BiometriBPPT.bppt.ptik.biometric.utility;
using System.IO;
using System.Diagnostics;
using BiometriBPPT.bppt.ptik.biometric.xprocessing;

namespace BiometriBPPT.bppt.ptik.biometric.iris
{
    public class MainIris
    {
        MyImage myImageORI;
        DebugImage debugImage;
        Iris.iriscode iriscode;
        List<Iris.iris_obj> irisObj;
        string hasilMatch = "";
        MatrixBppt mat = new MatrixBppt();

        public string HasilMatch
        {
            get { return hasilMatch; }
            set { hasilMatch = value; }
        }

        public MyImage AsMyImage
        {
            get { return myImageORI; }
            set { myImageORI = value; }
        }

        public Iris.iriscode IrisCode
        {
            get { return iriscode; }
            set { iriscode = value; }
        }

        public List<Iris.iris_obj> IrisObj
        {
            get { return irisObj; }
            set { irisObj = value; }
        }

        public MainIris(MyImage img, DebugImage debI)
        {
            myImageORI = img;
            debugImage = debI;
            iriscode = new Iris.iriscode();
        }

        public MainIris(DebugImage debI)
        {
            debugImage = debI;
            iriscode = new Iris.iriscode();
        }

        private void mainAlgo()
        {
            //DebugImage debugImage = new DebugImage(true);
            //debugImage.LogPath = fileName;
            EdgeDetection edgeDetection = new EdgeDetection(debugImage);
            IntensityTransform intensityTransform = new IntensityTransform();
            Preprocessing preprocessing = new Preprocessing(debugImage);
            HoughTransform houghTransform = new HoughTransform();
            IrisProcessing irisProcessing = new IrisProcessing(debugImage);
            Thresholding thresholding = new Thresholding();

            int i, j;
            //byte[][] pixel;
            int image_height, image_width;
            //int first_peak;
            //int check_for_pupil = 0;
            //int binarization_threshold = 0;

            MyImage my_image = cloneMyImage(myImageORI);
            MyImage ptr_my_image = my_image;

            image_height = ptr_my_image.Height;
            image_width = ptr_my_image.Width;

            // INITIALIZE OBJECTS //
            MyImage ori_image = new MyImage(image_height, image_width);
            MyImage ptr_ori_image = ori_image;

            MyImage canny_image = new MyImage(image_height, image_width);
            MyImage ptr_canny_image = canny_image;

            // END INITILIAZE OBJECTS //

            // BACKUP ORIGINAL IMAGE //	
            // Create new stopwatch
            Stopwatch stopwatch = new Stopwatch();

            // Begin timing
            //stopwatch.Start();
            /*
             for (j = 0; j < image_height; j++)
                 for (i = 0; i < image_width; i++)
                     ptr_ori_image.UPixel[j, i] = ptr_my_image.UPixel[j, i];
              */
            //ptr_ori_image = mat.Mat_Clone(ptr_my_image);
            // Stop timing
            //stopwatch.Stop();

            // Write result
            //Debug.WriteLine("Time elapsed: {0}", stopwatch.Elapsed);
            /*
            for (j = 0; j < image_height; j++)
                for (i = 0; i < image_width; i++)
                    ptr_ori_image.UPixel[j, i] = ptr_my_image.UPixel[j, i];
             */
            ptr_ori_image = mat.Mat_Clone(ptr_my_image);
            // END BACKUP ORIGINAL IMAGE //

            //double begin = 0;
            // variable declarations used for time calculation
            //float elapTicks;
            //float elapMilli = 0;
            //float elapSeconds = 0;
            //float elapMinutes = 0;


            edgeDetection.canny(ptr_my_image, ptr_canny_image, 100, 25);
            debugImage.DebugImg_WriteImg("canny-image1.pgm", ptr_canny_image);

            //preprocessing.MedianFilter(ptr_canny_image);


            intensityTransform.complement(ptr_canny_image);
            debugImage.DebugImg_WriteImg("canny-image2.pgm", ptr_canny_image);
            preprocessing.Erosion(ptr_canny_image);
            debugImage.DebugImg_WriteImg("canny-image3.pgm", ptr_canny_image);
            intensityTransform.complement(ptr_canny_image);


            debugImage.DebugImg_WriteImg("canny-image.pgm", ptr_canny_image);
            debugImage.DebugImg_ImgToTxt("canny-image.txt", ptr_canny_image);

            Iris.pupil_info pupilInfo = new Iris.pupil_info();
            Iris.pupil_info ptr_pupil_info = pupilInfo;

            ptr_pupil_info.Radius = 0;
            ptr_pupil_info.A = 0;
            ptr_pupil_info.B = 0;

            houghTransform.hough_circle(ptr_canny_image, ptr_pupil_info);

            MyImage roi_image = new MyImage(ptr_pupil_info.Radius * 2 + 100, ptr_pupil_info.Radius * 2 + 100);
            MyImage ptr_roi_image = roi_image;

            MyImage ori_roi_image = new MyImage(ptr_pupil_info.Radius * 2 + 100, ptr_pupil_info.Radius * 2 + 100);
            MyImage ptr_ori_roi_image = ori_roi_image;

            for (j = 0; j < ori_roi_image.Height; j++)
                for (i = 0; i < ori_roi_image.Width; i++)
                    ori_roi_image.UPixel[j, i] = 255;

            Iris.iris_boundary iris_boundary = new Iris.iris_boundary();
            Iris.iris_boundary ptr_iris_boundary = iris_boundary;//T_IRIS_BOUNDARY *ptr_iris_boundary = &iris_boundary;

            irisProcessing.find_pupillary_limbus_boundary(ptr_my_image, ptr_roi_image, ptr_ori_image, ptr_ori_roi_image, ptr_pupil_info, ptr_iris_boundary);

            //printf("Elapsed time: %f secs.\n", (float)clock() / CLOCKS_PER_SEC);

            MyImage ptr_unwrapped_image = new MyImage(Iris.RADIUS, Iris.THETA);
            MyImage ptr_otsu_unwrapped_image = new MyImage(Iris.RADIUS, Iris.THETA);
            MyImage ptr_gabor_real_image = new MyImage(Iris.RADIUS, Iris.THETA);
            MyImage ptr_gabor_imag_image = new MyImage(Iris.RADIUS, Iris.THETA);

            // UNWRAP IRIS //		
            irisProcessing.iris_unwrapping(ptr_ori_image, ptr_iris_boundary, ptr_pupil_info, ptr_unwrapped_image);

            //path = build_result_path(result_dir_path, "iris-unwrapped.pgm");
            //write_output(path, ptr_unwrapped_image);
            //sprintf(unwrapped_path, "%s", path);
            debugImage.DebugImg_WriteImg("iris-unwrapped.pgm", ptr_unwrapped_image);
            debugImage.DebugImg_ImgToTxt("iris-unwrapped.txt", ptr_unwrapped_image);
            //	GtkTextviewAppend(text_field, "Iris normalization . . . OK\n");		
            //path[0] = '\0';

            // NOISE MASK //
            thresholding.otsu_optimum_global_thr_binarization(ptr_unwrapped_image, ptr_otsu_unwrapped_image);
            intensityTransform.complement(ptr_otsu_unwrapped_image);
            irisProcessing.check_otsu(ptr_otsu_unwrapped_image);
            //path = build_result_path(result_dir_path, "otsu-iris-unwrapped.pgm");
            //write_output(path, ptr_otsu_unwrapped_image);
            debugImage.DebugImg_WriteImg("otsu-iris-unwrapped.pgm", ptr_otsu_unwrapped_image);
            //path[0] = '\0';

            //printf("Elapsed time: %f secs.\n", (float)clock() / CLOCKS_PER_SEC);
            //printf("unwrapping+masking\n");

            Iris.iriscode ptr_iriscode = IrisCode;

            ptr_iriscode.Bit = new byte[Iris.RADIUS * Iris.THETA * 2];
            ptr_iriscode.Size = Iris.RADIUS * Iris.THETA * 2;
            ptr_iriscode.Mask = new byte[Iris.RADIUS * Iris.THETA * 2];

            for (j = 0; j < ptr_iriscode.Size; j++)
            {
                ptr_iriscode.Bit[j] = 0;
                ptr_iriscode.Mask[j] = 0;
            }

            irisProcessing.gabor_filtering(ptr_unwrapped_image, ptr_gabor_real_image, ptr_gabor_imag_image, ptr_iriscode, ptr_otsu_unwrapped_image);
            debugImage.DebugImg_byte1dToTxt("score.txt", IrisCode.Bit);
            //printf("Elapsed time: %f secs.\n", (float) clock()/CLOCKS_PER_SEC);
            //printf("gabor+encoding\n");
        }

        private void xMainAlgo()
        {
            //DebugImage debugImage = new DebugImage(true);
            //debugImage.LogPath = fileName;
            EdgeDetection edgeDetection = new EdgeDetection(debugImage);
            IntensityTransform intensityTransform = new IntensityTransform();
            Preprocessing preprocessing = new Preprocessing(debugImage);
            HoughTransform houghTransform = new HoughTransform();
            IrisProcessing irisProcessing = new IrisProcessing(debugImage);
            ProcessIris processIris = new ProcessIris();
            Thresholding thresholding = new Thresholding();

            int i, j;
            //byte[][] pixel;
            int image_height, image_width;
            //int first_peak;
            //int check_for_pupil = 0;
            //int binarization_threshold = 0;

            MyImage my_image = cloneMyImage(myImageORI);
            MyImage ptr_my_image = my_image;

            image_height = ptr_my_image.Height;
            image_width = ptr_my_image.Width;

            // INITIALIZE OBJECTS //
            MyImage ori_image = new MyImage(image_height, image_width);
            MyImage ptr_ori_image = ori_image;

            MyImage canny_image = new MyImage(image_height, image_width);
            MyImage ptr_canny_image = canny_image;

            // END INITILIAZE OBJECTS //

            // BACKUP ORIGINAL IMAGE //	
            // Create new stopwatch
            //Stopwatch stopwatch = new Stopwatch();

            // Begin timing
            //stopwatch.Start();
            /*
             for (j = 0; j < image_height; j++)
                 for (i = 0; i < image_width; i++)
                     ptr_ori_image.UPixel[j, i] = ptr_my_image.UPixel[j, i];
              */
            //ptr_ori_image = mat.Mat_Clone(ptr_my_image);
            // Stop timing
            //stopwatch.Stop();

            // Write result
            //Debug.WriteLine("Time elapsed: {0}", stopwatch.Elapsed);
            /*
            for (j = 0; j < image_height; j++)
                for (i = 0; i < image_width; i++)
                    ptr_ori_image.UPixel[j, i] = ptr_my_image.UPixel[j, i];
             */
            ptr_ori_image = mat.Mat_Clone(ptr_my_image);
            // END BACKUP ORIGINAL IMAGE //

            //double begin = 0;
            // variable declarations used for time calculation
            //float elapTicks;
            //float elapMilli = 0;
            //float elapSeconds = 0;
            //float elapMinutes = 0;


            //edgeDetection.canny(ptr_my_image, ptr_canny_image, 100, 25);
            edgeDetection.canny(ptr_my_image, ptr_canny_image, 100, 25);
            debugImage.DebugImg_WriteImg("canny-image1.pgm", ptr_canny_image);
            //preprocessing.MedianFilter(ptr_canny_image);

            intensityTransform.complement(ptr_canny_image);
            debugImage.DebugImg_WriteImg("canny-image2.pgm", ptr_canny_image);
            preprocessing.Erosion(ptr_canny_image);
            debugImage.DebugImg_WriteImg("canny-image3.pgm", ptr_canny_image);
            intensityTransform.complement(ptr_canny_image);

            debugImage.DebugImg_WriteImg("canny-image.pgm", ptr_canny_image);

            Iris.pupil_info pupilInfo = new Iris.pupil_info();
            Iris.pupil_info ptr_pupil_info = pupilInfo;

            ptr_pupil_info.Radius = 0;
            ptr_pupil_info.A = 0;
            ptr_pupil_info.B = 0;

            houghTransform.hough_circle(ptr_canny_image, ptr_pupil_info);

            MyImage roi_image = new MyImage(ptr_pupil_info.Radius * 2 + 100, ptr_pupil_info.Radius * 2 + 100);
            MyImage ptr_roi_image = roi_image;

            MyImage ori_roi_image = new MyImage(ptr_pupil_info.Radius * 2 + 100, ptr_pupil_info.Radius * 2 + 100);
            MyImage ptr_ori_roi_image = ori_roi_image;

            for (j = 0; j < ori_roi_image.Height; j++)
                for (i = 0; i < ori_roi_image.Width; i++)
                    ori_roi_image.UPixel[j, i] = 255;

            Iris.iris_boundary iris_boundary = new Iris.iris_boundary();
            Iris.iris_boundary ptr_iris_boundary = iris_boundary;//T_IRIS_BOUNDARY *ptr_iris_boundary = &iris_boundary;

            irisProcessing.find_pupillary_limbus_boundary(ptr_my_image, ptr_roi_image, ptr_ori_image, ptr_ori_roi_image, ptr_pupil_info, ptr_iris_boundary);

            //printf("Elapsed time: %f secs.\n", (float)clock() / CLOCKS_PER_SEC);

            int irisWidth = ptr_iris_boundary.Limbus - ptr_iris_boundary.Pupil;  // the iris width

            MyImage ptr_unwrapped_image = new MyImage(irisWidth, 256);
            MyImage ptr_otsu_unwrapped_image = new MyImage(irisWidth, 256);
            MyImage ptr_gabor_real_image = new MyImage(irisWidth, 256);
            MyImage ptr_gabor_imag_image = new MyImage(irisWidth, 256);

            // UNWRAP IRIS //		
            processIris.iris_unwrapping(ptr_ori_image, ptr_iris_boundary, ptr_pupil_info, ptr_unwrapped_image, debugImage);

            //path = build_result_path(result_dir_path, "iris-unwrapped.pgm");
            //write_output(path, ptr_unwrapped_image);
            //sprintf(unwrapped_path, "%s", path);
            debugImage.DebugImg_WriteImg("iris-unwrapped.pgm", ptr_unwrapped_image);
            //	GtkTextviewAppend(text_field, "Iris normalization . . . OK\n");		
            //path[0] = '\0';

            // NOISE MASK //
            thresholding.otsu_optimum_global_thr_binarization(ptr_unwrapped_image, ptr_otsu_unwrapped_image);
            intensityTransform.complement(ptr_otsu_unwrapped_image);
            irisProcessing.check_otsu(ptr_otsu_unwrapped_image);
            //path = build_result_path(result_dir_path, "otsu-iris-unwrapped.pgm");
            //write_output(path, ptr_otsu_unwrapped_image);
            debugImage.DebugImg_WriteImg("otsu-iris-unwrapped.pgm", ptr_otsu_unwrapped_image);
            //path[0] = '\0';

            //printf("Elapsed time: %f secs.\n", (float)clock() / CLOCKS_PER_SEC);
            //printf("unwrapping+masking\n");

            Iris.iriscode ptr_iriscode = IrisCode;

            ptr_iriscode.Bit = new byte[2048];
            ptr_iriscode.Size = 2048;
            ptr_iriscode.Mask = new byte[2048];

            for (j = 0; j < ptr_iriscode.Size; j++)
            {
                ptr_iriscode.Bit[j] = 0;
                ptr_iriscode.Mask[j] = 0;
            }

            GaborToImage gaborToImage = new GaborToImage();
            gaborToImage.gaborImage(ptr_unwrapped_image, ptr_otsu_unwrapped_image, ptr_iriscode);

        }

        public void enroll(string side, string name)
        {
            mainAlgo();
            //return;
            Iris.iris_obj iObj = new Iris.iris_obj();
            iObj.Class_no = null;
            iObj.Side = side;
            iObj.Name = name;
            debugImage.LogPath = "IrisTemplate";

            for (int l = 0; l < IrisCode.Bit.Length; l++)
            {
                iObj.Bit += IrisCode.Bit[l];
                iObj.Bit_mask += "0";
                //iObj.Bit_mask += IrisCode.Mask[l];
            }
            IrisProcessing irisProcessing = new IrisProcessing(debugImage);
            irisProcessing.WriteIrisObj("IrisTemplate\\iriscode_" + side + "_" + name + ".txt", iObj, FileMode.Create);
        }

        public void enrollFolder(string masterFolder)
        {
            int i, j;
            PGM pgm;
            string[] class_no = listDirs(masterFolder);
            string[] temp;
            string[] filename;

            string[,] side = new string[class_no.Length, 2];
            for (i = 0; i < class_no.Length; i++)
            {
                temp = listDirs(class_no[i]);
                side[i, 0] = temp[0];
                side[i, 1] = temp[1];
            }

            //List<Iris.iris_obj> irisObj = new List<Iris.iris_obj>();

            int k;
            for (j = 0; j < class_no.Length; j++)
                for (i = 0; i < side.GetLength(1); i++)
                {
                    filename = listFiles(side[j, i]);
                    for (k = 0; k < filename.Length; k++)
                    {
                        temp = filename[k].Split('\\');
                        Iris.iris_obj iObj = new Iris.iris_obj();
                        iObj.Class_no = temp[1];
                        iObj.Side = temp[2];
                        iObj.Name = temp[3];
                        pgm = new PGM(filename[k]);
                        string[] names = iObj.Name.Split('.');
                        iObj.Name = names[0];
                        debugImage.LogPath = temp[0] + "\\" + temp[1] + "\\" + temp[2] + "\\img-" + names[0];
                        myImageORI = new MyImage(pgm.Height, pgm.Width, MyImgType.UCHAR);

                        myImageORI.UPixel = pgm.Data;
                        MainIris mainIris = new MainIris(myImageORI, debugImage);
                        mainIris.mainAlgo();
                        //mainIris.xMainAlgo();
                        for (int l = 0; l < mainIris.IrisCode.Bit.Length; l++)
                        {
                            iObj.Bit += mainIris.IrisCode.Bit[l];
                            //iObj.Bit_mask += "0";
                            iObj.Bit_mask += mainIris.IrisCode.Mask[l];
                        }
                        IrisProcessing irisProcessing = new IrisProcessing(debugImage);
                        if (j + i + k == 0)
                        {
                            irisProcessing.WriteIrisObj("iriscode_database.txt", iObj, FileMode.Create);
                        }
                        else
                        {
                            irisProcessing.WriteIrisObj("iriscode_database.txt", iObj, FileMode.Append);
                        }
                    }
                }
        }

        public void read_db(string fileName)
        {
            IrisObj = new List<Iris.iris_obj>();

            FileStream fs = new FileStream(fileName, FileMode.Open);
            StreamReader sr = new StreamReader(fs);
            string temp = "";
            string[] data;
            while ((temp = sr.ReadLine()) != null)
            {
                Iris.iris_obj irisObj = new Iris.iris_obj();
                data = temp.Split('_');
                irisObj.Class_no = data[0];
                irisObj.Side = data[1];
                irisObj.Name = data[2];
                irisObj.Bit = data[3];
                irisObj.Bit_mask = data[4];
                IrisObj.Add(irisObj);
            }
            /*
            for (int i = 0; i < length; i++)
            {
                Iris.iris_obj irisObj = new Iris.iris_obj();
                sw.WriteLine(i + " " + HIP[i]);
            } */
            sr.Dispose();
            sr.Close();
            fs.Close();


        }

        public void matching()
        {
            mainAlgo();

            int i, j;
            Iris.iriscode tmp_iriscode = new Iris.iriscode();
            tmp_iriscode.Size = IrisCode.Size;
            tmp_iriscode.Bit = new byte[IrisCode.Size];
            tmp_iriscode.Mask = new byte[IrisCode.Size];

            Iris.iriscode ptr_tmp_iriscode = tmp_iriscode;


            for (j = 0; j < IrisCode.Size; j++)
            {
                tmp_iriscode.Bit[j] = IrisCode.Bit[j];
                tmp_iriscode.Mask[j] = IrisCode.Mask[j];
            }

            /*
            float HD = 0;
            float max_HD = 1;
            int maxHD_id = 0;
            for (j = 0; j < IrisObj.Count; j++)
            {
                shift_left(ptr_tmp_iriscode, j * 2);
                for (i = 0; i < IrisObj.Count; i++)
                {//for (i = 0; i < 30; i++) {//120; i++) {
                    Iris.iris_obj tmp_iris_obj = new Iris.iris_obj();
                    tmp_iris_obj.Name = IrisObj[i].Name;
                    tmp_iris_obj.Class_no = IrisObj[i].Class_no;
                    tmp_iris_obj.Side = IrisObj[i].Side;
                    //tmp_iris_obj.Bit = new byte[2 * Iris.RADIUS * Iris.THETA];
                    //tmp_iris_obj.Bit_mask = (unsigned char*)malloc(2 * RADIUS * THETA * sizeof(unsigned char));
                    tmp_iris_obj.Bit = IrisObj[i].Bit;
                    tmp_iris_obj.Bit_mask = IrisObj[i].Bit_mask;

                    Iris.iris_obj ptr_tmp_iris_obj = tmp_iris_obj;

                    HD = find_HD_match(ptr_tmp_iris_obj);

                    //			printf("%s HD = %f\n", iris_object[i].name, HD);
                    if (HD < max_HD && HD >= 0)
                    {
                        max_HD = HD;
                        maxHD_id = i;
                    }
                }

                //		printf("max_HD = %f maxHD_id = %d\n", max_HD, maxHD_id);
                //		printf("class_no %s \t name = %s \t side = %s \n", iris_object[maxHD_id].class_no, iris_object[maxHD_id].name, iris_object[maxHD_id].side);

                shift_right(ptr_tmp_iriscode, j * 4);
                for (i = 0; i < IrisObj.Count; i++)
                {//for (i = 0; i < 30; i++) {//120; i++) {
                    Iris.iris_obj tmp_iris_obj = new Iris.iris_obj();
                    tmp_iris_obj.Name = IrisObj[i].Name;
                    tmp_iris_obj.Class_no = IrisObj[i].Class_no;
                    tmp_iris_obj.Side = IrisObj[i].Side;
                    //tmp_iris_obj.Bit = (unsigned char*)malloc(2 * RADIUS * THETA * sizeof(unsigned char));
                    //tmp_iris_obj.Bit_mask = (unsigned char*)malloc(2 * RADIUS * THETA * sizeof(unsigned char));	
                    tmp_iris_obj.Bit = IrisObj[i].Bit;
                    tmp_iris_obj.Bit_mask = IrisObj[i].Bit_mask;

                    Iris.iris_obj ptr_iris_obj = tmp_iris_obj;

                    HD = find_HD_match(ptr_iris_obj);

                    //			printf("%s HD = %f\n", iris_object[i].name, HD);
                    if (HD < max_HD && HD > 0)
                    {
                        max_HD = HD;
                        maxHD_id = i;
                    }
                }

                //		fprintf(fp_single_HD, "%s_%s_%f", name_only, ptr_iris_obj->name, inter_class_HD)
            }
            HasilMatch = "max_HD = " + max_HD + " maxHD_id = " + maxHD_id + "\n";
            HasilMatch += "class_no = " + IrisObj[maxHD_id].Class_no + " name = " + IrisObj[maxHD_id].Name + " side = " + IrisObj[maxHD_id].Side;
            //printf("max_HD = %f maxHD_id = %d\n", max_HD, maxHD_id);
            //printf("class_no %s \t name = %s \t side = %s \n\n", iris_object[maxHD_id].class_no, iris_object[maxHD_id].name, iris_object[maxHD_id].side);
            //fprintf(fp_HD, "%s_%s_%f\n", name_only, iris_object[maxHD_id].name, max_HD);
            //fclose(fp_HD);
             */

            float HD = 0;
            float max_local_HD = 1;
            //int local_HD_id = 0;
            //int usable_bit_max = 0;
            float max_HD = 1;
            int maxHD_id = 0;
            int match_found = 0;

            for (i = 0; i < IrisObj.Count; i++)
            {//40; i++) {
                Iris.iris_obj tmp_iris_obj = new Iris.iris_obj();//IRIS TEMPLATE
                tmp_iris_obj.Name = IrisObj[i].Name;
                tmp_iris_obj.Class_no = IrisObj[i].Class_no;
                tmp_iris_obj.Side = IrisObj[i].Side;
                tmp_iris_obj.Bit = IrisObj[i].Bit;
                tmp_iris_obj.Bit_mask = IrisObj[i].Bit_mask;

                Iris.iris_obj ptr_tmp_iris_obj = tmp_iris_obj;


                max_local_HD = 1;

                for (j = 0; j < 30; j++)
                {
                    shift_left(ptr_tmp_iriscode, j * 2);
                    HD = find_HD_match(ptr_tmp_iris_obj);
                    if (HD < max_local_HD)
                    {
                        max_local_HD = HD;
                        //max_HD_id = i;
                    }

                    shift_right(ptr_tmp_iriscode, j * 2);
                    HD = find_HD_match(ptr_tmp_iris_obj);
                    if (HD < max_local_HD)
                    {
                        max_local_HD = HD;
                        //max_HD_id = i;
                    }
                }

                match_found = 0;
                if (max_local_HD < max_HD && max_local_HD >= 0.0)
                {
                    max_HD = max_local_HD;
                    maxHD_id = i;
                }
            }

            HasilMatch = "max_HD = " + max_HD + " maxHD_id = " + maxHD_id + "\n";
            HasilMatch += "class_no = " + IrisObj[maxHD_id].Class_no + " name = " + IrisObj[maxHD_id].Name + " side = " + IrisObj[maxHD_id].Side;
        }

        float find_HD_match(Iris.iris_obj ptr_iris_obj)
        {
            int j;
            int length = IrisCode.Size;
            //printf("length = %d\n", length);
            int diff_c = 0;
            int usable_bit = 0;
            float HD = 0;
            for (j = 0; j < length; j++)
            {
                if (IrisCode.Mask[j] == 0 && ptr_iris_obj.Bit_mask[j] == '0')
                {
                    string temp = "" + ptr_iris_obj.Bit[j];
                    //int temp = int.Parse(ptr_iris_obj.Bit[j].ToString);
                    if (int.Parse(temp) != IrisCode.Bit[j])
                        diff_c++;
                    usable_bit++;
                }
            }
            //printf("\n"); 
            //printf("%s\n", ptr_iris_obj->name);
            //printf("diff_c = %d\n", diff_c);
            //printf("usable_bit = %d\n", usable_bit);
            //	printf("diff_c %d\n", diff_c);
            HD = (float)diff_c / usable_bit;
            //printf("HD = %f\n\n", HD);
            //	ptr_hd_norm->HD = HD;
            //	ptr_hd_norm->usable_bit = usable_bit;
            return HD;
        }

        void shift_left(Iris.iriscode ptr_iriscode_result, int degree)
        {
            int length = IrisCode.Size;

            int j, c, d;

            byte[] temp = new byte[length];
            byte[] temp_mask = new byte[length];

            for (j = 0; j < length; j++)
            {
                c = j + degree;
                if (c >= length)
                    d = c - length;
                else
                    d = c;
                temp[j] = IrisCode.Bit[d];
                temp_mask[j] = IrisCode.Mask[d];
            }

            for (j = 0; j < length; j++)
            {
                ptr_iriscode_result.Bit[j] = temp[j];
                ptr_iriscode_result.Mask[j] = temp_mask[j];
                //		printf("%d", ptr_iriscode_result->bit[j]);
            }
            //	printf("\n");
        }

        float find_HD(Iris.iris_obj ptr_iris_obj, Iris.HD_NORM ptr_hd_norm)
        {
            int j;
            int length = IrisCode.Size;
            int diff_c = 0;
            int usable_bit = 0;
            float HD = 0;

            for (j = 0; j < length; j++)
            {
                if (IrisCode.Mask[j] == 0 && ptr_iris_obj.Bit_mask[j] == '0')
                {
                    string temp = "" + ptr_iris_obj.Bit[j];
                    //int temp = int.Parse(ptr_iris_obj.Bit[j].ToString);
                    if (int.Parse(temp) != IrisCode.Bit[j])
                        diff_c++;
                    usable_bit++;
                }
            }


            HD = (float)diff_c / usable_bit;
            //printf("HD = %f\n\n", HD);
            ptr_hd_norm.HD = HD;
            ptr_hd_norm.UsableBit = usable_bit;
            return HD;
        }

        public void matching_one_to_many()
        {
            /*
            FILE *fp_HD; 
	
            char *HD_name = malloc(24 * sizeof(char));
            strcpy(HD_name, "HD_one_to_many_norm");
            strcat(HD_name, ".txt");
            fp_HD = fopen(HD_name, "a");
             */

            mainAlgo();

            int i, j;

            Iris.iriscode tmp_iriscode = new Iris.iriscode();
            tmp_iriscode.Size = IrisCode.Size;
            tmp_iriscode.Bit = new byte[IrisCode.Size];
            tmp_iriscode.Mask = new byte[IrisCode.Size];
            Iris.iriscode ptr_tmp_iriscode = tmp_iriscode;

            for (j = 0; j < iriscode.Size; j++)
            {
                tmp_iriscode.Bit[j] = iriscode.Bit[j];
                tmp_iriscode.Mask[j] = iriscode.Mask[j];
            }

            float HD = 0;
            float max_local_HD = 1;
            //int local_HD_id = 0;
            int usable_bit_max = 0;

            for (i = 0; i < IrisObj.Count; i++)
            {
                Iris.iris_obj tmp_iris_obj = new Iris.iris_obj(); //IRIS TEMPLATE

                tmp_iris_obj.Name = IrisObj[i].Name;
                tmp_iris_obj.Class_no = IrisObj[i].Class_no;
                tmp_iris_obj.Side = IrisObj[i].Side;
                //tmp_iris_obj.Bit = (unsigned char*)malloc(2 * RADIUS * THETA * sizeof(unsigned char));
                //tmp_iris_obj.Bit_mask = (unsigned char*)malloc(2 * RADIUS * THETA * sizeof(unsigned char));
                tmp_iris_obj.Bit = IrisObj[i].Bit;
                tmp_iris_obj.Bit_mask = IrisObj[i].Bit_mask;

                Iris.iris_obj ptr_tmp_iris_obj = tmp_iris_obj;

                Iris.HD_NORM hd_norm = new Iris.HD_NORM();
                Iris.HD_NORM ptr_hd_norm = hd_norm;

                max_local_HD = 1;
                usable_bit_max = 0;

                for (j = 0; j < 30; j++)
                {
                    shift_left(ptr_tmp_iriscode, j * 2);
                    HD = find_HD(ptr_tmp_iris_obj, ptr_hd_norm);
                    if (HD < max_local_HD)
                    {
                        max_local_HD = HD;
                        usable_bit_max = ptr_hd_norm.UsableBit;
                    }

                    shift_right(ptr_tmp_iriscode, j * 2);
                    HD = find_HD(ptr_tmp_iris_obj, ptr_hd_norm);
                    if (HD < max_local_HD)
                    {
                        max_local_HD = HD;
                        usable_bit_max = ptr_hd_norm.UsableBit;
                    }
                }

                //fprintf(fp_HD, "%s_%s_%f_%d\n", name_only, iris_object[i].name, max_local_HD, usable_bit_max);
            }
            //fclose(fp_HD);
        }

        void shift_right(Iris.iriscode ptr_iriscode_result, int degree)
        {
            int length = IrisCode.Size;

            int j, c, d;

            byte[] temp = new byte[length];
            byte[] temp_mask = new byte[length];

            for (j = length - 1; j >= 0; j--)
            {
                c = j - degree;
                if (c < 0)
                    d = c + length;
                else
                    d = c;
                temp[j] = IrisCode.Bit[d];
                temp_mask[j] = IrisCode.Mask[d];
            }

            for (j = 0; j < length; j++)
            {
                ptr_iriscode_result.Bit[j] = temp[j];
                ptr_iriscode_result.Mask[j] = temp_mask[j];
                //		printf("%d", ptr_iriscode_result->bit[j]);
            }
            //	printf("\n");
        }
        string[] listDirs(String directoryName)
        {

            return System.IO.Directory.GetDirectories(directoryName);

        }

        string[] listFiles(String directoryName)
        {

            return System.IO.Directory.GetFiles(directoryName);

        }

        MyImage cloneMyImage(MyImage myimage)
        {
            MyImage newMat = new MyImage(myimage.Height, myimage.Width, myimage.TypeData);

            if (myimage.TypeData == MyImgType.UCHAR)
            {
                Buffer.BlockCopy(myimage.UPixel, 0, newMat.UPixel, 0, newMat.UPixel.Length);
            }
            else if (myimage.TypeData == MyImgType.FLOAT)
            {
                System.Array.Copy(myimage.FPixel, newMat.FPixel, newMat.FPixel.Length);
            }
            else if (myimage.TypeData == MyImgType.INT)
            {
                System.Array.Copy(myimage.IPixel, newMat.IPixel, newMat.IPixel.Length);
            }

            return newMat;
        }
    }
}
