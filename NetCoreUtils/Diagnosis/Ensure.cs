using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NetCoreUtils.Diagnosis
{
    static public class Ensure
    {
        private static void ThrowException(string msg = "")
        {
            throw new Exception("Ensure confirmation failed. " + msg);
        }

        public static void IsTrue(bool condition)
        {
            if (!condition)
            {
                ThrowException();
            }
        }

        public static void IsTrue(bool condition, string msg)
        {
            if (!condition)
            {
                ThrowException(msg);
            }
        }
    }
}
