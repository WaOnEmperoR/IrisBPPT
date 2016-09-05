using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BiometriBPPT.bppt.ptik.biometric.entity;

namespace BiometriBPPT.bppt.ptik.biometric.image
{
    public class Padding
    {
        public void padding(MyImage ptr_my_image, int pad_size, MyImage ptr_padded_image)
        {
            int new_height = ptr_padded_image.Height;
            int new_width = ptr_padded_image.Width;

            int height = ptr_my_image.Height;
            int width = ptr_my_image.Width;

            int j, i;

            for (j = 0; j < new_height; j++)
            {
                for (i = 0; i < new_width; i++)
                {
                    ptr_padded_image.UPixel[j, i] = 0;
                }
            }

            for (j = pad_size; j < new_height - pad_size; j++)
                for (i = pad_size; i < new_width - pad_size; i++)
                    ptr_padded_image.UPixel[j, i] = ptr_my_image.UPixel[j - pad_size, i - pad_size];

        }

        public void unpadding(MyImage ptr_padded_image, int pad_size, MyImage ptr_my_image)
        {
            int pheight = ptr_padded_image.Height;
            int pwidth = ptr_padded_image.Width;

            int height = ptr_my_image.Height;
            int width = ptr_my_image.Width;

            int j, i;

            for (j = pad_size; j < pheight - pad_size; j++)
            {
                for (i = pad_size; i < pwidth - pad_size; i++)
                {
                    ptr_my_image.DPixel[j - pad_size, i - pad_size] = ptr_padded_image.DPixel[j, i];
                }
            }
        }
    }
}
