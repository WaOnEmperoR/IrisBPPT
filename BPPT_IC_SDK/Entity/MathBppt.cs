using System;


namespace BPPT_IC_SDK.Entity
{
    public class MathBppt
    {
        public static float DegreeToRad(float degree)
        {
            return (float)(degree * Math.PI / 180);
        }

        public static float RadToDegree(float rad)
        {
            return (float)(rad * 180 / Math.PI);
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
