using DotNetty.Codecs;
using DotNetty.Handlers.Logging;
using DotNetty.Transport.Bootstrapping;
using DotNetty.Transport.Channels;
using DotNetty.Transport.Channels.Sockets;
using NetCoreUtils.TestCli.DotNettyDemo;
using NetCoreUtils.TestCli.DotNettyDemo.Clients;
using NetCoreUtils.TestCli.DotNettyDemo.Handlers;
using NetCoreUtils.TestCli.DotNettyDemo.Servers;
using Serilog;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace NetCoreUtils.TestCli.Commands
{
    class DotNettyCommand
    {
        IExecutable _echoClient = new EchoClient();
        IExecutable _echoServer = new EchoServer();

        public void TcpServer()
        {
            Log.Information("dot-netty server executed");
            _echoServer.Execute().Wait();
        }

        public void TcpClient()
        {
            Log.Information("dot-netty tcp-client executed");
            _echoClient.Execute().Wait();
        }
       

    }
}
