using Microsoft.VisualStudio.TestTools.UnitTesting;
using NetCoreUtils.ProcessUtils;
using System;

namespace NetCoreUtils.MsTest
{
    [TestClass]
    public class ShellExecutorTests
    {
        [TestMethod]
        public void Test_BatchPrint_bat_file() => Console.WriteLine(new TerminalUtil().BatchPrint(@"e:\test_bat.bat"));

        [TestMethod]
        public void Test_BatchPrint_git() => Console.WriteLine(new TerminalUtil().BatchPrint("git status"));

        [TestMethod]
        public void Test_BatchPrint_core()
        {
            Console.WriteLine(new TerminalUtil().BatchPrint("core"));
        }

        [TestMethod]
        public void Test_bash() => Console.WriteLine(new TerminalUtil().Bash(@"e:\test_bat.bat"));
    }
}
