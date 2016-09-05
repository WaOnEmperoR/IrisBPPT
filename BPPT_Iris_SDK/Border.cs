using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BPPT_Iris_SDK
{
    public class Border
    {
        private int x;
        private int y;
        private int chaincode;

        public int X
        {
            get { return x; }
            set { x = value; }
        }

        public void setX(int _x)
        {
            this.x = _x;
        }

        public int Y
        {
            get { return y; }
            set { y = value; }
        }

        public void setY(int _y)
        {
            this.y = _y;
        }

        public int Chaincode
        {
            get { return chaincode; }
            set { chaincode = value; }
        }
    }
}
