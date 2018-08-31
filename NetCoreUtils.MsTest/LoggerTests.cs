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
            using (var logger = new Logger(new LoggerConfig { OutputToTerminal = true }))
            {
                logger.WriteError("An error message.");
                logger.WriteDetails("some detail\nsome detail\nsome more details", 0);
                logger.WriteDetails("some detail\nsome detail\nsome more details");
                logger.WriteDetails("some detail\nsome detail\nsome more details", 2);

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
                logger.WriteDebug("An trace message.");
            }
        }

        [TestMethod]
        public void construct_the_same_logger_twice()
        {
            using (var logger = new Logger(new LoggerConfig { OutputToTerminal = true }))
            using (var logger2 = new Logger(new LoggerConfig { OutputToTerminal = true }))
            {

            }
        }
    }
}
