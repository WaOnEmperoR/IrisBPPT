using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BiometriBPPT.bppt.ptik.biometric.entity
{
    class AssertException : ApplicationException
    {
        public AssertException() { }

        public AssertException(string message)
            : base(message)
        {
        }

        public static void Check(bool condition)
        {
            if (!condition)
                Fail();
        }

        public static void Check(bool condition, string message)
        {
            if (!condition)
                Fail(message);
        }

        public static void FailIf(bool condition)
        {
            if (condition)
                Fail();
        }

        public static void Fail()
        {
            throw new AssertException();
        }

        public static void Fail(string message)
        {
            throw new AssertException(message);
        }
    }
}
