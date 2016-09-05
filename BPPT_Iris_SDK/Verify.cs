using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BPPT_Iris_SDK
{
    public class Verify
    {
        public Verify()
        {
        }

        public float doVerify(IrisCode irisCodeLive, IrisCode irisCodeTemplate)
        {
            if (irisCodeLive == null || irisCodeTemplate == null)
            {
                return 1;
            }
            Matching matching = new Matching(irisCodeLive, irisCodeTemplate);
            return matching.PerformMatching();
        }
    }
}
