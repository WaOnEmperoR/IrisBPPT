using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BiometriBPPT.bppt.ptik.biometric.xentity;

namespace BiometriBPPT.bppt.ptik.biometric.xfilter
{
    public class Gaussian : Filter
    {
        // Peak is the value in the centre of the gaussian
        protected double peak;
        protected double alpha;
        protected double beta;

        protected double waveletValue(int rho, int phi)
        {

            return peak * Math.Exp(-Math.Pow((rho), 2) / Math.Pow(alpha, 2))
                * Math.Exp(-Math.Pow((phi), 2) / Math.Pow(beta, 2));

        }

        public Gaussian(int _dimension, double _peak)
        {
            Dimension = _dimension;
            Filterarray = new double[_dimension, _dimension];
            peak = _peak;

            // Scale the constants so that gaussian is always in the same range
            // Uses alpha = dimension * (4sqrt(-ln(1/3)))**-1
            // The gaussian will have the value peak/3 at each of its edges
            // and peak/9 at its corners
            alpha = (_dimension - 1) * 0.4770322291;
            beta = alpha;
        }

        public void generateFilter()
        {
            int rho;
            int phi;

            for (int i = 0; i < Dimension; i++)
            {
                rho = i - (Dimension / 2);
                for (int j = 0; j < Dimension; j++)
                {
                    phi = j - (Dimension / 2);
                    Filterarray[i, j] = waveletValue(rho, phi);
                }
            }
        }
    }
}
