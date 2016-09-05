using BPPT_Iris_SDK.id.go.bppt.biometri.iris.image;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace BPPT_Iris_SDK.id.go.bppt.biometri.iris.processing
{
    public class Histogram
    {
        int[] histogram = new int[256];

        public int[] GetHistogram
        {
            get { return histogram; }
            set { histogram = value; }
        }
        int pixel_count;
        double[] probability_distribution = new double[256];
        double[] goodnessArray = new double[256];

        public Histogram(Bitmap bmp)
        {
            histogram = buildHistogram(bmp);
        }

        int[] buildHistogram(Bitmap bmp)
        {
            LockBitmap lockBmp = new LockBitmap(bmp);
            lockBmp.LockBits();
            int[] histogram = new int[256];

            for (int imgY = 0; imgY < bmp.Height; imgY++)
            {
                for (int imgX = 0; imgX < bmp.Width; imgX++)
                {
                    histogram[lockBmp.GetPixel8bpp(imgX, imgY)]++;
                }
            }
            lockBmp.UnlockBits();
            return histogram;
        }

        public int getThresholdWithoutBlack()
        {
            int max = 0;

            for (int i = 0; i < 80; i++)
                histogram[i] = 0; // disregard blacks for pupil

            int pixel_count = 0;
            for (int i = 0; i < 256; i++)
                pixel_count += histogram[i];
            //cout << pixel_count << endl;

            for (int i = 0; i < 256; i++)
            {
                probability_distribution[i] = (double)(histogram[i]) / pixel_count;
                //cout << probability_distribution[i] << endl;
            }

            createGoodnessArray();

            for (int i = 0; i < 256; i++)
                if (goodnessArray[i] > goodnessArray[max])
                    max = i;
            //cout << "Otsu thresholded on:" << threshold << endl;
            return max;
        }

        public int getThresholdWithoutWhite()
        {
            int max = 0;

            for (int i = 0; i < 80; i++)
                histogram[i] = 0; // disregard blacks for pupil

            histogram[255] = 0; // disregard white

            pixel_count = 0;
            for (int i = 0; i < 256; i++)
                pixel_count += histogram[i];
            //cout << pixel_count << endl;

            for (int i = 0; i < 256; i++)
            {
                probability_distribution[i] = (double)(histogram[i]) / pixel_count;
                //cout << probability_distribution[i] << endl;
            }

            createGoodnessArray();

            for (int i = 0; i < 256; i++)
                if (goodnessArray[i] > goodnessArray[max])
                    max = i;
            //cout << "Otsu thresholded on:" << threshold << endl;
            return max;
        }

        double omega(int k)
        {
            double omega = 0;
            for (int i = 0; i < k; i++)
                omega += probability_distribution[i];
            return omega;
        }

        double mew(int k)
        {
            double mew = 0;
            for (int i = 0; i < k; i++)
                mew += (i + 1) * probability_distribution[i];
            return mew;
        }

        double goodness(int k)
        {
            if (omega(k) == 1 || omega(k) == 0)
                return 0;
            return (mew(256) * omega(k) - mew(k)) * (mew(256) * omega(k) - mew(k)) / (omega(k) * (1 - omega(k)));
        }

        void createGoodnessArray()
        {
            for (int i = 0; i < 256; i++)
            {
                goodnessArray[i] = goodness(i);
            }
        }
    }
}
