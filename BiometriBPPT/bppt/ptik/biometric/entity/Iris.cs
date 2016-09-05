using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BiometriBPPT.bppt.ptik.biometric.entity
{
    public class Iris
    {
        public class Coordinate
        {
            private int x;
            private int y;

            public int X
            {
                get { return x; }
                set { x = value; }
            }

            public int Y
            {
                get { return y; }
                set { y = value; }
            }
        }

        public class HD_NORM
        {
            private float hd;
            private int usableBit;

            public float HD
            {
                get { return hd; }
                set { hd = value; }
            }

            public int UsableBit
            {
                get { return usableBit; }
                set { usableBit = value; }
            }
        }

        public class iris_boundary
        {
            private int pupil;
            private int limbus;

            public int Pupil
            {
                get { return pupil; }
                set { pupil = value; }
            }

            public int Limbus
            {
                get { return limbus; }
                set { limbus = value; }
            }
        }

        public struct kernel
        {
            public double[][] value;
            public int size;
        }

        public class iriscode
        {
            private byte[] bit;
            private int size;
            private byte[] mask;

            public byte[] Bit
            {
                get { return bit; }
                set { bit = value; }
            }

            public int Size
            {
                get { return size; }
                set { size = value; }
            }

            public byte[] Mask
            {
                get { return mask; }
                set { mask = value; }
            }
        }

        public class pupil_info
        {
            private int a;
            private int b;
            private int radius;

            public int A
            {
                get { return a; }
                set { a = value; }
            }

            public int B
            {
                get { return b; }
                set { b = value; }
            }

            public int Radius
            {
                get { return radius; }
                set { radius = value; }
            }
        }

        public class peak_loc
        {
            private int first;
            private int second;

            public int First
            {
                get { return first; }
                set { first = value; }
            }

            public int Second
            {
                get { return second; }
                set { second = value; }
            }
        }

        public class iris_obj
        {
            private string class_no;
            private string side;
            private string name;
            private string bit;
            private string bit_mask;

            public string Class_no
            {
                get { return class_no; }
                set { class_no = value; }
            }

            public string Side
            {
                get { return side; }
                set { side = value; }
            }

            public string Name
            {
                get { return name; }
                set { name = value; }
            }

            public string Bit
            {
                get { return bit; }
                set { bit = value; }
            }

            public string Bit_mask
            {
                get { return bit_mask; }
                set { bit_mask = value; }
            }
        }

        public const int L_MAX = 256;
        public const int L_BASE = 5;        // base value for connected component extraction
        public const int MASK_SIZE = 5;     // filter mask 5x5
        public const int LOW = 0;           // object is black
        public const int MID1 = 64;			// 85					// 64					//128      // object is black
        public const int MID2 = 128;		// 170				// 128
        public const int MID3 = 192;
        public const int HIGH = 255;        // background is white
        public const int OBJECT_INTENSITY = HIGH;
        public const double PI = 3.1415926536;
        //#define 	 PI				 3.1415926536
        //#define    CLK_TCK   CLOCKS_PER_SEC

        public const int RADIUS = 20;
        public const int THETA = 180;

        public const string PATH = "IrisLog";
    }
}
