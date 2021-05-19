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
        static public void Execute(Action fn, Action<Exception> overwrittenExceptionHandler = null, Action finalHandler = null)
        {
            try
            {
                fn();
            }
            catch (Exception ex)
            {
                if (overwrittenExceptionHandler == null)
                {
                    Console.Error.WriteLine(ex.Message);

                    if (ex.InnerException != null)
                        Console.Error.WriteLine(ex.InnerException.Message);

                    Console.Error.WriteLine(ex.StackTrace);
                }
                else
                    overwrittenExceptionHandler(ex);
            }
            finally
            {
                if (finalHandler != null)
                    finalHandler();
            }
        }
    }
}
