using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BiometriBPPT.bppt.ptik.biometric.entity;
using BiometriBPPT.bppt.ptik.biometric.xentity;

namespace BiometriBPPT.bppt.ptik.biometric.xprocessing
{
    public class GaborToPixel
    {
        public static bool gaborPixel(int rho, int phi, Sinusoidal sinusoidalFilter, MyImage rawImage, MyImage mask)
        {

            // size of the filter to be applied
            int filterSize = sinusoidalFilter.Dimension;

            // running total used for integration
            double runningTotal = 0.0;

            // translated co-ords within image
            int imageX;
            int imageY;
            int angles = rawImage.Width;

            for (int i = 0; i < filterSize; ++i)
            {
                for (int j = 0; j < filterSize; ++j)
                {

                    // Actual angular position within the image
                    imageY = j + phi - (filterSize / 2);

                    // Allow filters to loop around the image in the angular direction
                    imageY %= angles;
                    if (imageY < 0)
                        imageY += angles;

                    // Actual radial position within the image
                    imageX = i + rho - (filterSize / 2);

                    // If the bit is good then apply the filter and add this to the sum
                    if (mask.UPixel[imageX, imageY]>128)
                    {
                        runningTotal += (sinusoidalFilter.getFilter(i, j)) * (double)(rawImage.UPixel[imageX, imageY]);
                    }
                }
            }

            // Return true if +ve and false if -ve
            return (runningTotal >= 0.0) ? true : false;
        }
    }
}
