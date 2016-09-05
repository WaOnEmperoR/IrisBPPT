using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BPPT_Iris_SDK
{
    public class Projection
    {
        public static int horizontalProjection(int width, int height, int[,] pixel)
        {
            int[] horizontal = new int[width];
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    horizontal[j] += pixel[i, j];
                }
            }
            int smallest = width * 255 + 900;
            int pos = 0;
            for (int i = 50; i <= 200; i++)
            {
                if (horizontal[i] < smallest)
                {
                    smallest = horizontal[i];
                    pos = i;
                }
            }
            return pos;
        }

        public static int[] horizontalProjection(int[,] pixel)
        {
            int g_ImageHeight = pixel.GetLength(0);
            int g_ImageWidth = pixel.GetLength(1);

            int[] horizontal = new int[g_ImageWidth];
            int[] pos = new int[2];
            for (int i = 0; i < g_ImageHeight; i++)
            {
                for (int j = 0; j < g_ImageWidth; j++)
                {
                    horizontal[j] += pixel[i, j];
                }
            }
            int max = 255 * g_ImageHeight;
            for (int i = 0; i < g_ImageWidth; i++)
            {
                if (horizontal[i] != max)
                {
                    pos[0] = i;
                    break;
                }
            }
            for (int i = g_ImageWidth - 1; i >= 0; i--)
            {
                if (horizontal[i] != max)
                {
                    pos[1] = i;
                    break;
                }
            }

            return pos;
        }

        public static int[] specialHorizontalProjection(int width, int height, int[,] pixel)
        {
            int[] horizontal = new int[width];
            int[] pos = new int[2];
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    horizontal[j] += pixel[i, j];
                }
            }
            int max = 255 * height;
            for (int i = 0; i < width; i++)
            {
                if (horizontal[i] != max)
                {
                    pos[0] = i;
                    break;
                }
            }
            for (int i = width - 1; i >= 0; i--)
            {
                if (horizontal[i] != max)
                {
                    pos[1] = i;
                    break;
                }
            }

            return pos;
        }

        public static int[,] horizontalROI(int width, int height, int[,] pixel, int pos)
        {
            int pos1 = pos - 70;
            int pos2 = pos + 70;
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    if (j > pos1 && j < pos2)
                    {

                    }
                    else
                    {
                        pixel[i, j] = 0;
                    }
                }
            }

            return pixel;
        }

        public static int verticalProjection(int width, int height, int[,] pixel)
        {
            int[] vertical = new int[height];
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    vertical[i] += pixel[i, j];
                }
            }
            int smallest = width * 255 + 900;
            int pos = 0;
            for (int i = 60; i <= 200; i++)
            {
                if (vertical[i] < smallest)
                {
                    smallest = vertical[i];
                    pos = i;
                }
            }
            return pos;
        }

        public static int[] verticalProjection(int[,] pixel)
        {
            int g_ImageHeight = pixel.GetLength(0);
            int g_ImageWidth = pixel.GetLength(1);

            int[] vertical = new int[g_ImageHeight];
            int[] pos = new int[2];
            for (int i = 0; i < g_ImageHeight; i++)
            {
                for (int j = 0; j < g_ImageWidth; j++)
                {
                    vertical[i] += pixel[i, j];
                }
            }
            int max = 255 * g_ImageWidth;
            for (int i = 0; i < g_ImageHeight; i++)
            {
                if (vertical[i] != max)
                {
                    pos[0] = i;
                    break;
                }
            }
            for (int i = g_ImageHeight - 1; i >= 0; i--)
            {
                if (vertical[i] != max)
                {
                    pos[1] = i;
                    break;
                }
            }

            return pos;
        }

        public static int[] specialVerticalProjection(int width, int height, int[,] pixel)
        {
            int[] vertical = new int[height];
            int[] pos = new int[2];
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    vertical[i] += pixel[i, j];
                }
            }
            int max = 255 * width;
            for (int i = 0; i < height; i++)
            {
                if (vertical[i] != max)
                {
                    pos[0] = i;
                    break;
                }
            }
            for (int i = height - 1; i >= 0; i--)
            {
                if (vertical[i] != max)
                {
                    pos[1] = i;
                    break;
                }
            }

            return pos;
        }

        public static void generateImageInitiatePoint(String filename, string filepath)
        {
            PGM_Iris pgmIris = new PGM_Iris(filepath);
            pgmIris.WriteToPath(filename + ".pgm");
        }
    }
}
