using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BiometriBPPT.bppt.ptik.biometric.xentity
{
    public class Filter
    {
        private int dimension;
        private double[,] filterarray;

        public int Dimension
        {
            get { return dimension; }
            set { dimension = value; }
        }

        public double[,] Filterarray
        {
            get { return filterarray; }
            set { filterarray = value; }
        }

        public Filter()
        {
        }

        public Filter(int _dimension)
        {
            dimension = _dimension;

            filterarray = new double[dimension, dimension];

            // Construct Array
            //for (int i = 0; i < dimension; i++)
            //    filterarray[i] = new double[dimension];
        }

        public double getFilter(int row, int col)
        {
            return Filterarray[row, col];
        }

        public void multiplyBy(Filter otherFilter)
        {
            if (dimension != otherFilter.Dimension) { }
            //cout << "ERROR! You cannot multiply two filters which are not the same size!\nThe multiplication was not performed." << endl;
            for (int j = 0; j < dimension; j++)
            {
                for (int i = 0; i < dimension; i++)
                {
                    filterarray[j, i] *= otherFilter.getFilter(j, i);
                }
            }
        }
    }
}
