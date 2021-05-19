using System.Runtime.InteropServices;
using System;
using System.Collections.Generic;
using System.Text;

namespace NetCoreUtils.OS
{
    static public class SystemUtil
    {
        static public string GetUserDir()
        {

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                return Environment.GetEnvironmentVariable("USERPROFILE");
            }
            else
            {
                return Environment.GetEnvironmentVariable("HOME");
            }
        }
    }
}
