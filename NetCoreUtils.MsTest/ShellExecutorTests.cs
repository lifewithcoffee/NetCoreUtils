using Microsoft.VisualStudio.TestTools.UnitTesting;
using NetCoreUtils.Shell;
using System;

namespace NetCoreUtils.MsTest
{
    [TestClass]
    public class ShellExecutorTests
    {
        [TestMethod]
        public void Test_BatchPrint_bat_file() => Console.WriteLine(new ShellExecutor().BatchPrint(@"e:\test_bat.bat"));

        [TestMethod]
        public void Test_BatchPrint_git() => Console.WriteLine(new ShellExecutor().BatchPrint("git status"));

        [TestMethod]
        public void Test_BatchPrint_core()
        {
            Console.WriteLine(new ShellExecutor().BatchPrint("core"));
        }

        [TestMethod]
        public void Test_bash() => Console.WriteLine(new ShellExecutor().Bash(@"e:\test_bat.bat"));
    }
}
