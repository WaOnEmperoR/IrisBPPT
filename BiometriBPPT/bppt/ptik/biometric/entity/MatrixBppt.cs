using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BiometriBPPT.bppt.ptik.biometric.entity
{
    public class MatrixBppt
    {
        public MyImage Mat_FloatToChar(MyImage mat)
        {
            //assert(mat->type == FLOAT);

            int i, j;
            float val, min = 0, max = 1;
            MyImage newMat;

            /* Search for the min/max value of the matrix. */
            for (j = 0; j < mat.Height; j++)
            {
                for (i = 0; i < mat.Width; i++)
                {
                    val = mat.FPixel[j, i];
                    if (val < min) min = val;
                    if (val > max) max = val;
                }
            }

            /* Initialize UCHAR matrix and do conversion from FLOAT. */
            newMat = new MyImage(mat.Height, mat.Width, MyImgType.UCHAR);
            for (j = 0; j < mat.Height; j++)
            {
                for (i = 0; i < mat.Width; i++)
                {

                    /* Scale FLOAT to UCHAR. */
                    byte newVal = (byte)((mat.FPixel[j, i] - min) * 255 / (max - min));
                    newMat.UPixel[j, i] = newVal;
                }
            }

            return newMat;
        }

        public MyImage Mat_Invert(MyImage src, MyImage dst)
        {
            int i, j;
            if (dst == null)
            {
                dst = new MyImage(src.Height, src.Width, MyImgType.UCHAR);
            }
            for (j = 0; j < src.Height; j++)
                for (i = 0; i < src.Width; i++)
                    dst.UPixel[j, i] = (byte)(255 - src.UPixel[j, i]);
            return dst;
        }

        public MyImage Mat_Clone(MyImage myimage)
        {
            //assert(mat->rows > 0 && mat->cols > 0);

            MyImage newMat = new MyImage(myimage.Height, myimage.Width, myimage.TypeData);

            if (myimage.TypeData == MyImgType.UCHAR)
            {
                System.Array.Copy(myimage.UPixel, newMat.UPixel, newMat.UPixel.Length);
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
