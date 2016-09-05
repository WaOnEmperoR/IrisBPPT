using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BiometriBPPT.bppt.ptik.biometric.entity
{
    public class MathBppt
    {
        public static float DegreeToRad(float degree)
        {
            return (float)(degree * Iris.PI / 180);
        }

        public static float RadToDegree(float rad)
        {
            return (float)(rad * 180 / Iris.PI);
        }

        public static float toFloat(float rad)
        {
            return float.Parse(rad.ToString("0.000000"));
        }

        public static float toFloat(double rad)
        {
            return float.Parse(rad.ToString("0.000000"));
        }
    }
}
