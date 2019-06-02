using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetCoreUtils.MethodCall
{
    public class SafeCall
    {
        static public void Execute(Action fn, Action<Exception> exceptionHandler = null, Action finalHandler = null)
        {
            try
            {
                fn();
            }
            catch (Exception ex)
            {
                if (exceptionHandler == null)
                    Trace.WriteLine(ex.StackTrace);
                else
                    exceptionHandler(ex);
            }
            finally
            {
                if (finalHandler != null)
                    finalHandler();
            }
        }
    }
}
