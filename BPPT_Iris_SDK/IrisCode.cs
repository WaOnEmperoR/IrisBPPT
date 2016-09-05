using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BPPT_Iris_SDK
{
    public class IrisCode
    {
        private byte[] bitImage;
        private int sizeImage;
        private byte[] maskImage;

        public byte[] bit
        {
            get
            {
                return bitImage;
            }
            set
            {
                this.bitImage = value;
            }
        }

        public int size
        {
            get
            {
                return sizeImage;
            }
            set
            {
                this.sizeImage = value;
            }
        }

        public byte[] mask
        {
            get
            {
                return maskImage;
            }
            set
            {
                this.maskImage = value;
            }
        }
    }
}
