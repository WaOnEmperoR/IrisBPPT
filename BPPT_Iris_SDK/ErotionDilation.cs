using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BPPT_Iris_SDK
{
    public class ErotionDilation
    {
        private int[,] output;

        public ErotionDilation()
        {

        }

        public int[,] Erotion(int[,] input) {
            output = new int[input.GetLength(0),input.GetLength(1)];
            for (int j = 0; j < input.GetLength(0); j++) {
                for (int i = 0; i < input.GetLength(1); i++) {
                    if (input[j,i] == 255 && input[j,i - 1] == 255 && input[j,i + 1] == 255 && input[j - 1,i] == 255 && input[j + 1,i] == 255) {
                        output[j,i] = 255;
                    } else {
                        output[j,i] = 0;
                    }
                }
            }
            return output;
        }

        public int[,] Dilation(int[,] input) {
            output = new int[input.GetLength(0),input.GetLength(1)];
            for (int i = 0; i < input.GetLength(0); i++) {
                for (int j = 0; j < input.GetLength(1); j++) {
                    if (input[i,j] == 255) {
                        output[i,j] = 255;
                        output[i,j - 1] = 255;
                        output[i,j + 1] = 255;
                        output[i - 1,j] = 255;
                        output[i + 1,j] = 255;
                    }
                }
            }

            return output;
        }

        public int[,] Connect(int[,] input) {
            output = new int[input.GetLength(0),input.GetLength(1)];
            for (int i = 0; i < input.GetLength(0); i++) {
                for (int j = 0; j < input.GetLength(1); j++) {
                    output[i,j] = input[i,j];
                }
            }
            for (int i = 1; i < input.GetLength(0) - 1; i++) {
                for (int j = 1; j < input.GetLength(1) - 1; j++) {
                    int count = 0;
                    if (input[i - 1,j-1] == 255) {
                        count++;
                    }
                    if (input[i - 1,j] == 255) {
                        count++;
                    }
                    if (input[i - 1,j+1] == 255) {
                        count++;
                    }
                    if (input[i,j - 1] == 255) {
                        count++;
                    }
                    if (input[i,j + 1] == 255) {
                        count++;
                    }

                    if (input[i + 1,j-1] == 255) {
                        count++;
                    }
                    if (input[i + 1,j] == 255) {
                        count++;
                    }
                    if (input[i + 1,j+1] == 255) {
                        count++;
                    }
                
                
                    if(count >= 2)
                     output[i,j] = 255;
                }
            }

            return output;
        }
    }
}
