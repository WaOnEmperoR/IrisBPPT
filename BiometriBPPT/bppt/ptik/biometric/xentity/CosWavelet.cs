using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BiometriBPPT.bppt.ptik.biometric.xentity
{
    class CosWavelet : Sinusoidal
    {
        public CosWavelet(int dimension)
        {
            Dimension = dimension;
            Filterarray = new double[dimension, dimension];
        }

        override public double waveletValue(int theta, int dimension) { 
            return Math.Cos(Math.PI * theta / (dimension / 2));
        }
    }
}
