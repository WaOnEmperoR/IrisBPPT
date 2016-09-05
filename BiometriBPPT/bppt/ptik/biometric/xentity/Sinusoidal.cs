using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BiometriBPPT.bppt.ptik.biometric.xfilter;

namespace BiometriBPPT.bppt.ptik.biometric.xentity
{
    public class Sinusoidal : Filter
    {
        public Sinusoidal()
        {
        }

        public Sinusoidal(int dimension)
        {
            new Filter(dimension);
        }

        public void generateFilter()
        {

            int phi;
            double sum = 0;

            for (int j = 0; j < Dimension; j++)
            {
                phi = j - (Dimension / 2);
                Filterarray[0, j] = waveletValue(phi, Dimension);
                sum += Filterarray[0, j];
            }
            for (int j = 0; j < Dimension; j++)
            {
                Filterarray[0, j] -= (sum / Dimension);
            }
            for (int i = 1; i < Dimension; i++)
            {
                for (int j = 0; j < Dimension; j++)
                {
                    Filterarray[i, j] = Filterarray[0, j];
                }
            }

            Gaussian temp = new Gaussian(Dimension, 15.0);
            temp.generateFilter();
            multiplyBy(temp);

            // make every row have equal +ve and -ve

            for (int i = 0; i < Dimension; ++i)
            {

                double rowSum = 0.0;

                for (int j = 0; j < Dimension; ++j)
                {
                    rowSum += Filterarray[i, j];
                }

                for (int j = 0; j < Dimension; ++j)
                {
                    Filterarray[i, j] -= rowSum / (double)(Dimension);
                }
            }
        }

        public virtual double waveletValue(int phi, int dimension) {
            return 0;
        }
    }
}
