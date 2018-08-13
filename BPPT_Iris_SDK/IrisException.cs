using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BPPT_Iris_SDK
{
    public class IrisException : Exception
    {
        public IrisException() : base()
        {
        }

        public IrisException(string message) : base(message)
        {
        }

        public IrisException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
