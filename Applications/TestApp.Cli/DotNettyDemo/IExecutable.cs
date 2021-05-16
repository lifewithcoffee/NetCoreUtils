using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TestApp.Cli.DotNettyDemo
{
    public interface IExecutable
    {
        Task ExecuteAsync();
    }
}
