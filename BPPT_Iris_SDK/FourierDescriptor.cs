using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BPPT_Iris_SDK
{
    public class FourierDescriptor
    {
        public double FourierRealPart;
        public double FourierImaginaryPart;

        public FourierDescriptor()
        { }

        public FourierDescriptor(double Real, double Imag)
        {
            this.FourierRealPart = Real;
            this.FourierImaginaryPart = Imag;
        }
    }
}
