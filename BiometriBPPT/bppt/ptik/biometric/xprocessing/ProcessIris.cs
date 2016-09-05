using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BiometriBPPT.bppt.ptik.biometric.entity;
using BiometriBPPT.bppt.ptik.biometric.image;
using BiometriBPPT.bppt.ptik.biometric.utility;
using BiometriBPPT.bppt.ptik.biometric.xentity;
using Cairo;

namespace BiometriBPPT.bppt.ptik.biometric.xprocessing
{
    class ProcessIris
    {
        public void iris_unwrapping(MyImage ptr_ori_roi_image, Iris.iris_boundary ptr_iris_boundary, Iris.pupil_info ptr_pupil_info, MyImage ptr_unwrapped_image, DebugImage debugImage)
        {
            int j, i;
            int Rpupil = ptr_iris_boundary.Pupil;
            int Rlimbus = ptr_iris_boundary.Limbus;
            int a = ptr_pupil_info.A;
            int b = ptr_pupil_info.B;

            //printf("Rpupil %d\t Rlimbus %d\t a %d\t b %d\n", Rpupil, Rlimbus, a, b);

            // int x, y;
            int theta = ptr_unwrapped_image.Width;
            int radius = ptr_unwrapped_image.Height;
            int x_r_theta, y_r_theta, xp_theta, yp_theta, xl_theta, yl_theta;//, x_center, y_center;
            byte[,] temp_unwrapped = new byte[radius, theta];
            //printf("theta %d radius %d\n", theta, radius);

            for (j = 0; j < radius; j++)
            {
                for (i = 0; i < theta; i++)
                {
                    //Math.Sin(Math.PI * theta / (dimension / 2));   
                    // find coordinate of the pupillary boundary point at angle = theta
                    xp_theta = a + (int)((float)Rpupil * Math.Cos(Iris.PI * (float)i / 180));
                    yp_theta = b - (int)((float)Rpupil * Math.Sin(Iris.PI * (float)i / 180));

                    // find coordinate of the limbus boundary point  at angle = theta
                    xl_theta = a + (int)((float)Rlimbus * Math.Cos(Iris.PI * (float)i / 180));
                    yl_theta = b - (int)((float)Rlimbus * Math.Sin(Iris.PI * (float)i / 180));

                    // find the intensity of pixel located at ( x_r_theta, y_r_theta) in the origin
                    x_r_theta = (int)((1.0 - (float)j / radius) * xp_theta + (float)j / radius * xl_theta);
                    y_r_theta = (int)((1.0 - (float)j / radius) * yp_theta + (float)j / radius * yl_theta);
                    //if (y_r_theta < 0) y_r_theta = 0;
                    //if (x_r_theta < 0) x_r_theta = 0;
                    temp_unwrapped[j, i] = ptr_ori_roi_image.UPixel[y_r_theta, x_r_theta];
                }
            }

            // Get only the lower half of iris region
            for (j = 0; j < radius; j++)
            {
                for (i = 0; i < theta; i++)
                {
                    ptr_unwrapped_image.UPixel[j, i] = temp_unwrapped[j, i];
                }
            }

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
    }
}
