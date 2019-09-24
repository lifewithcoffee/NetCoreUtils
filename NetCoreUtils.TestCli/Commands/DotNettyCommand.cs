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
        public void TcpEchoServer()
        {
            Log.Information("dot-netty TCP server started:");
            new EchoServer().Execute().Wait();
        }

        public void TcpEchoClient()
        {
            Log.Information("dot-netty TCP client started:");
            new EchoClient().Execute().Wait();
        }

        public void TcpServer()
        {
            Log.Information("dot-netty TCP client started:");
            new TcpServer().Execute().Wait();
        }

        public void UdpServer()
        {
            Log.Information("dot-netty UDP server started:");
            new UdpServer().Execute().Wait();
        }
    }
}
