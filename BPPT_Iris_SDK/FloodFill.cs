using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BPPT_Iris_SDK
{
    class FloodFill
    {
        private Queue<Point> queue_pixels = new Queue<Point>();
        private int[,] img_pixels;
        private int[,] img_mask_fills;
        private int x_seed, y_seed, lim1, lim2, threshold, height_center, width_center;

        public FloodFill(int[,] img_pixels, int x_seed, int y_seed, int lim1, int lim2, int threshold, int height_center, int width_center)
        {
            this.img_pixels = img_pixels;
            this.x_seed = x_seed;
            this.y_seed = y_seed;
            this.lim1 = lim1;
            this.lim2 = lim2;
            this.height_center = height_center;
            this.width_center = width_center;
            this.threshold = threshold;
        }

        public int[,] doFloodFilling()
        {
            int height = img_pixels.GetLength(0);
            int width = img_pixels.GetLength(1);

            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    img_mask_fills[i,j] = 0;
                }
            }

            queue_pixels.Enqueue(new Point(x_seed, y_seed));
            img_mask_fills[x_seed, y_seed] = 128;

            while (queue_pixels.Count > 0)
            {
                Point current = queue_pixels.Dequeue();
                int i = current.x;
                int j = current.y;

                if (img_mask_fills[i - 1, j - 1] == 0 && 
                    gradMag(img_pixels, i - 1, j - 1) <= threshold && 
                    distanceCenter(i - 1, j - 1, height_center, width_center) >= lim1 &&
                    distanceCenter(i - 1, j - 1, height_center, width_center) < lim2)
                {
                    img_mask_fills[i - 1, j - 1] = 128;
                    queue_pixels.Enqueue(new Point(i - 1, j - 1));
                }
                if (img_mask_fills[i - 1, j] == 0 &&
                    gradMag(img_pixels, i - 1, j) <= threshold &&
                    distanceCenter(i - 1, j, height_center, width_center) >= lim1 &&
                    distanceCenter(i - 1, j, height_center, width_center) < lim2)
                {
                    img_mask_fills[i - 1, j] = 128;
                    queue_pixels.Enqueue(new Point(i - 1, j));
                }
                if (img_mask_fills[i - 1, j + 1] == 0 &&
                    gradMag(img_pixels, i - 1, j + 1) <= threshold &&
                    distanceCenter(i - 1, j + 1, height_center, width_center) >= lim1 &&
                    distanceCenter(i - 1, j + 1, height_center, width_center) < lim2)
                {
                    img_mask_fills[i - 1, j + 1] = 128;
                    queue_pixels.Enqueue(new Point(i - 1, j + 1));
                }
                if (img_mask_fills[i, j - 1] == 0 &&
                    gradMag(img_pixels, i, j - 1) <= threshold &&
                    distanceCenter(i, j - 1, height_center, width_center) >= lim1 &&
                    distanceCenter(i, j - 1, height_center, width_center) < lim2)
                {
                    img_mask_fills[i, j - 1] = 128;
                    queue_pixels.Enqueue(new Point(i, j - 1));
                }
                if (img_mask_fills[i, j + 1] == 0 &&
                    gradMag(img_pixels, i, j + 1) <= threshold &&
                    distanceCenter(i, j + 1, height_center, width_center) >= lim1 &&
                    distanceCenter(i, j + 1, height_center, width_center) < lim2)
                {
                    img_mask_fills[i, j + 1] = 128;
                    queue_pixels.Enqueue(new Point(i, j + 1));
                }
                if (img_mask_fills[i + 1, j - 1] == 0 &&
                    gradMag(img_pixels, i + 1, j - 1) <= threshold &&
                    distanceCenter(i + 1, j - 1, height_center, width_center) >= lim1 &&
                    distanceCenter(i + 1, j - 1, height_center, width_center) < lim2)
                {
                    img_mask_fills[i + 1, j - 1] = 128;
                    queue_pixels.Enqueue(new Point(i + 1, j - 1));
                }
                if (img_mask_fills[i + 1, j] == 0 &&
                    gradMag(img_pixels, i + 1, j) <= threshold &&
                    distanceCenter(i + 1, j, height_center, width_center) >= lim1 &&
                    distanceCenter(i + 1, j, height_center, width_center) < lim2)
                {
                    img_mask_fills[i + 1, j] = 128;
                    queue_pixels.Enqueue(new Point(i + 1, j));
                }
                if (img_mask_fills[i + 1, j + 1] == 0 &&
                    gradMag(img_pixels, i + 1, j + 1) <= threshold &&
                    distanceCenter(i + 1, j + 1, height_center, width_center) >= lim1 &&
                    distanceCenter(i + 1, j + 1, height_center, width_center) < lim2)
                {
                    img_mask_fills[i + 1, j + 1] = 128;
                    queue_pixels.Enqueue(new Point(i + 1, j + 1));
                }
            }

            return img_mask_fills;
        }

        public static double distanceCenter(int j, int i, int center_j, int center_i)
        {
            return Math.Sqrt(((j - center_j) * (j - center_j)) + ((i - center_i) * (i - center_i)));
        }

        public static double gradMag(int[,] pix2, int j, int i)
        {
            double sobel1 = (1 * pix2[j - 1, i - 1] + 2 * pix2[j - 1, i] + 1 * pix2[j - 1, i + 1] + 0 * pix2[j, i - 1] + 0 * pix2[j, i] + 0 * pix2[j, i + 1] - 1 * pix2[j + 1, i - 1] - 2 * pix2[j + 1, i] - 1 * pix2[j + 1, i + 1]) / 1;
            double sobel2 = (-1 * pix2[j - 1, i - 1] + 0 * pix2[j - 1, i] + pix2[j - 1, i + 1] + -2 * pix2[j, i - 1] + 0 * pix2[j, i] + 2 * pix2[j, i + 1] - 1 * pix2[j + 1, i - 1] + 0 * pix2[j + 1, i] + pix2[j + 1, i + 1]) / 1;
            return Math.Sqrt((sobel1 * sobel1) + (sobel2 * sobel2));
        }
    }

    public struct Point
    {
        public int x;
        public int y;

        public Point(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
    }
}
