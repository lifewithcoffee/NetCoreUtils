using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace NetCoreUtils.TestCli.DotNettyDemo
{
    public interface IExecutable
    {
        Task ExecuteAsync();
    }
}
