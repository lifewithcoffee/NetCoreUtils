using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NetCoreUtils.Diagnosis.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace NetCoreUtils.MsTest
{
    [TestClass]
    public class LoggerTests
    {
        void throw_an_exception()
        {
            throw new Exception("Some exception");
        }

        [TestMethod]
        public void Test_logger()
        {
            using (var logger = new Logger(true))
            {
                logger.WriteError("An error message.");
                logger.WriteWarning("An warning message.");
                logger.WriteInfo("An info message.");
                try
                {
                    throw_an_exception();
                }
                catch(Exception ex)
                {
                    logger.WriteException(ex, "Some additional message");
                }
                logger.WriteTrace("An trace message.");
            }
        }
    }
}
