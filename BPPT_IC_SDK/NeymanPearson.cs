using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BPPT_IC_SDK
{
    public class NeymanPearson
    {
        public static float Calculate(float iris_s, float fingerprint_s)
        {
            float genuineProbability = CalculateProbabilityGenuine(iris_s, fingerprint_s);
            float impostorProbability = CalculateProbabilityImpostor(iris_s, fingerprint_s);
            float neymanScore = genuineProbability / impostorProbability;

            return neymanScore;
        }

        private static float CalculateProbabilityGenuine(float iris_s, float fingerprint_s)
        {
            float genuineIris = (float)((1 / (Math.Sqrt(2 * Math.PI * Math.Pow(10, 2)))) * (Math.Pow(Math.E, (Math.Pow(-(iris_s - 45), 2) / (2 * Math.Pow(10, 2))))));
            float genuineFinger = (float)((1 / (Math.Sqrt(2 * Math.PI * Math.Pow(8, 2)))) * (Math.Pow(Math.E, (Math.Pow(-(fingerprint_s - 40), 2) / (2 * Math.Pow(8, 2))))));
            float genuine = genuineIris * genuineFinger;

            return genuine;
        }

        private static float CalculateProbabilityImpostor(float iris_s, float fingerprint_s)
        {
            float impostorIris = (float)((1 / (Math.Sqrt(2 * Math.PI * Math.Pow(15, 2)))) * (Math.Pow(Math.E, (Math.Pow(-(iris_s - 80), 2) / (2 * Math.Pow(15, 2))))));
            float impostorFinger = (float)((1 / (Math.Sqrt(2 * Math.PI * Math.Pow(10, 2)))) * (Math.Pow(Math.E, (Math.Pow(-(fingerprint_s - 70), 2) / (2 * Math.Pow(10, 2))))));
            float impostor = impostorIris * impostorFinger;

            return impostor;
        }
    }
}
