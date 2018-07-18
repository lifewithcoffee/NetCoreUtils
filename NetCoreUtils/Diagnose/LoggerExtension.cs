using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NetCoreUtils.Diagnose
{
    static public class LoggerExtension
    {
        static public void WriteException(this ILogger logger, Exception ex, string message = null)
        {
            throw new NotImplementedException();
        }
    }
}
