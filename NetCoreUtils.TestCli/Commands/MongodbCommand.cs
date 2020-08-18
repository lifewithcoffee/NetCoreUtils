using System;
using System.Collections.Generic;
using System.Text;
using CoreCmd.Attributes;
using NetCoreUtils.TestCli.MongoDbDemo;

namespace NetCoreUtils.TestCli.Commands
{
    [Alias("mongo")]
    public class MongodbCommand : MongodbCommandBase { }
}
