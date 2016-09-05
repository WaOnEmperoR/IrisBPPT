using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BiometriBPPT.bppt.ptik.biometric.entity;

namespace BiometriBPPT.bppt.ptik.biometric.image
{
    public class Thresholding
    {
        // Otsu's Optimum Global Thresholding based on Gonzales, Woods, Sec. 10.3.3
        // N.Otsu,"A Threshold Selection Method from Gray-Level Histograms," IEEE Trans. Systems, Man, and Cybernetics, Vol.9, No.1, p.62-66

        public void otsu_optimum_global_thr_binarization(MyImage ptr_my_image, MyImage ptr_result_image)
        {
            int i, j;
            int[] histo = new int[Iris.L_MAX];
            float[] nhisto = new float[Iris.L_MAX], P1 = new float[Iris.L_MAX], m = new float[Iris.L_MAX], between_class_var = new float[Iris.L_MAX];
            float sum_p, sum_m, mG, max, eta_separability_measure, global_var;
            byte threshold = 0;
            int height = ptr_my_image.Height;
            int width = ptr_my_image.Width;

            for (i = 0; i < Iris.L_MAX; i++)
            {
                histo[i] = 0;
                nhisto[i] = 0;
            }

            for (j = 0; j < height; j++)
                for (i = 0; i < width; i++)
                    histo[ptr_my_image.UPixel[j, i]]++;

            for (i = 0; i < Iris.L_MAX; i++)
                nhisto[i] = (float)histo[i] / (height * width);  // normalized histogram
            //  for(i=0;i<L_MAX;i++)    fprintf(stdout,"%d\t%f\n",i,nhisto[i]);  exit(0);

            sum_p = 0;

            for (i = 0; i < Iris.L_MAX; i++)
            {
                sum_p += nhisto[i];
                P1[i] = sum_p; // P1[i] is the probability that a pixel to be assigned to class C1
            }
            //  for(i=0;i<L_MAX;i++)    fprintf(stdout,"%d\t%f\n",i,P1[i]);  exit(0);

            sum_m = 0;

            for (i = 0; i < Iris.L_MAX; i++)
            {
                sum_m += i * nhisto[i];
                m[i] = sum_m;  // cumulative mean (average intensity) up to level k Eq.(10.3-8)
            }
            //  for(i=0;i<L_MAX;i++)    fprintf(stdout,"%d\t%f\n",i,m[i]);  exit(0);

            for (mG = 0, i = 0; i < Iris.L_MAX; i++)
                mG += i * nhisto[i]; // the average intensity (global mean) Eq.(10.3-9)
            //  fprintf(stderr,"Global Mean: %f\n",mG);

            for (global_var = 0, i = 0; i < Iris.L_MAX; i++)
                global_var += ((float)i - mG) * ((float)i - mG) * nhisto[i]; // global variance Eq.(10.3-13)

            //  fprintf(Fp_log,"Searching maximum between class variance\n");
            max = -1;

            for (i = 0; i < Iris.L_MAX; i++)
                if (P1[i] > 0.1 && P1[i] < 1)
                {
                    between_class_var[i] = (mG * P1[i] - m[i]) * (mG * P1[i] - m[i]) / (P1[i] * (1 - P1[i]));
                    if (between_class_var[i] > max)
                    {
                        max = between_class_var[i];
                        threshold = (byte)i;
                    }

                    //			fprintf(Fp_log,"%d\t%f\t%f\t%f\t%f\n",i,mG,P1[i],m[i],between_class_var[i]);
                }

            eta_separability_measure = between_class_var[threshold] / global_var;
            //  fprintf(Fp_log,"binarization threshold obtained by Otsu method is %d with separability measure eta:%f\n",threshold,eta_separability_measure);

            for (j = 0; j < height; j++)
                for (i = 0; i < width; i++)
                    if (ptr_my_image.UPixel[j, i] > threshold)
                        ptr_result_image.UPixel[j, i] = Iris.HIGH;
                    else
                        ptr_result_image.UPixel[j, i] = Iris.LOW;

        }
    }
}
