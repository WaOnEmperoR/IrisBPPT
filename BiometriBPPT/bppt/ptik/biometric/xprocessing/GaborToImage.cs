using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BiometriBPPT.bppt.ptik.biometric.entity;
using BiometriBPPT.bppt.ptik.biometric.xentity;

namespace BiometriBPPT.bppt.ptik.biometric.xprocessing
{
    public class GaborToImage
    {
        public void gaborImage(MyImage rawImage, MyImage maskImage, Iris.iriscode irisCode)
        {

            int width = rawImage.Width;		// theta direction
            int height = rawImage.Height;	// r direction

            //bool[,] mask = pix2Boolean(maskImage);
            
            // Number of slices image is cut up into. Ideally angular slices should divide
            // 360, and size of bitCode without a remainder. More importantly, their product should
            // be divisible by 32
            int angularSlices = 256;
            int radialSlices = 1024 / angularSlices;

            // maximum filter size - set to 1/3 of image height to avoid large, uninformative filters
            int maxFilter = height / 3;

            int filterHeight;

            // tracks the position which needs to be modified in the bitcode and bitcodemask
            int bitCodePosition = 0;

            SinWavelet pSine = null;
            CosWavelet pCosine = null;

            for (int aSlice = 0; aSlice < angularSlices; ++aSlice)
            {

                int theta = aSlice;

                for (int rSlice = 0; rSlice < radialSlices; ++rSlice)
                {

                    // Works out which pixel in the image to apply the filter to
                    // Uniformly positions the centres of the filters between radius=3 and radius=height/2
                    // Does not consider putting a filter centre at less than radius=3, to avoid tiny filters
                    int radius = ((rSlice * (height - 6)) / (2 * radialSlices)) + 3;

                    // Set filter dimension to the largest filter that fits in the image
                    filterHeight = (radius < (height - radius)) ? 2 * radius - 1 : 2 * (height - radius) - 1;

                    // If the filter size exceeds the width of the image then correct this
                    if (filterHeight > width - 1)
                        filterHeight = width - 1;

                    // If the filter size exceeds the maximum size specified earlier then correct this
                    if (filterHeight > maxFilter)
                        filterHeight = maxFilter;

                    pSine = new SinWavelet(filterHeight);
                    pCosine = new CosWavelet(filterHeight);

                    pSine.generateFilter();
                    pCosine.generateFilter();

                    // Apply the filters to the calculated pixel in the image and set bitCode accordingly
                    if (GaborToPixel.gaborPixel(radius, theta, pCosine, rawImage, maskImage)) irisCode.Bit[bitCodePosition] = 1; else irisCode.Bit[bitCodePosition] = 0;
                    if (GaborToPixel.gaborPixel(radius, theta, pSine, rawImage, maskImage)) irisCode.Bit[bitCodePosition] = 1; else irisCode.Bit[bitCodePosition] = 0;

                    /*
                    if(pSine) { 
                        delete pSine;
                        pSine = NULL;
                    }
                    if(pCosine) {
                        delete pCosine;
                        pCosine = NULL;
                    }
                    */

                    // Check whether the pixel itself is bad
                    if (maskImage.UPixel[radius, theta]>128)
                        irisCode.Mask[bitCodePosition] = 1;
                    else
                        irisCode.Mask[bitCodePosition] = 0;

                    // Check whether a filter is good or bad
                    if (!checkFilter(radius, theta, filterHeight, maskImage))
                        irisCode.Mask[bitCodePosition] = 0;

                    // We're assuming that pairs of bits in the bitCodeMask are equal
                    irisCode.Mask[bitCodePosition + 1] = irisCode.Mask[bitCodePosition];

                    bitCodePosition += 2;
                }
            }

            /*
            // Make sure all the pointers are being good
            if(pSine) { 
                delete pSine;
                pSine = NULL;
            }

            if(pCosine) {
                delete pCosine;
                pCosine = NULL;
            }
            */

        }

        // Checks if a filter is "good" or not
        bool checkFilter(int radius, int theta, int filterHeight, MyImage mask)
        {
            int sum = 0;
            double goodRatio = 0.5; //ratio of good bits in a good filter
            double ratio;

            // Check the mask of all pixels within the range of the filter
            for (int rPos = radius - (filterHeight / 2); rPos <= radius + (filterHeight / 2); ++rPos)
                for (int tPos = theta - (filterHeight / 2); tPos <= theta + (filterHeight / 2); ++tPos)
                    try
                    {
                        if (mask.UPixel[rPos, tPos]>128) sum += 1;
                    }
                    catch (Exception ex) { 
                    }

            ratio = (double)(sum) / (double)(filterHeight * filterHeight);

            // If the ratio of good pixels to total pixels in the filter is good, return true
            return (ratio >= goodRatio) ? true : false;

        }

        /*
        private bool[,] pix2Boolean(MyImage maskImage)
        {
            bool[,] mask = new bool[maskImage.Height, maskImage.Width];
            for (int j = 0; j < maskImage.Height; j++)
                for (int i = 0; i < maskImage.Width; i++)
                    mask[j, i] = (maskImage.UPixel[j, i] > 128) ? true : false;
            return mask;
        }
        */ 
    }
}
