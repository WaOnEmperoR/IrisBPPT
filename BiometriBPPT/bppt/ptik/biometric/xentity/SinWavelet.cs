using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BiometriBPPT.bppt.ptik.biometric.xentity
{
    public class SinWavelet: Sinusoidal
    {
        public SinWavelet(int dimension) {
            
            Dimension = dimension;
            Filterarray = new double[dimension, dimension];
        }

        public override double waveletValue(int theta, int dimension) { 
           return Math.Sin(Math.PI * theta / (dimension / 2));            
        }
    }
}
