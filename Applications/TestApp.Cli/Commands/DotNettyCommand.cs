using DotNetty.Buffers;
using DotNetty.Codecs;
using DotNetty.Handlers.Logging;
using DotNetty.Transport.Bootstrapping;
using DotNetty.Transport.Channels;
using DotNetty.Transport.Channels.Embedded;
using DotNetty.Transport.Channels.Sockets;
using TestApp.Cli.DotNettyDemo;
using TestApp.Cli.DotNettyDemo.Clients;
using TestApp.Cli.DotNettyDemo.Handlers;
using TestApp.Cli.DotNettyDemo.Servers;
using Serilog;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace TestApp.Cli.Commands
{
    class DotNettyCommand
    {
        public async Task TcpEchoServer()
        {
            Log.Information("DotNetty TCP server started:");
            await new EchoServer().ExecuteAsync();
        }

        public async Task TcpEchoClient()
        {
            Log.Information("DotNetty TCP client started:");
            await new EchoClient().ExecuteAsync();
        }

        public async Task TcpServer()
        {
            Log.Information("DotNetty TCP server started:");
            await new TcpServer().ExecuteAsync();
        }

        public async Task UdpServer()
        {
            Log.Information("DotNetty UDP server started:");
            await new UdpServer().ExecuteAsync();
        }

        /// <summary>
        /// Embedded channel can be used in dotnetty channel unit test, see dotnetty official project's unit tests
        /// </summary>
        public void EmbeddedChannel()
        {
            EmbeddedChannel channel = new EmbeddedChannel(new StringDecoder(Encoding.UTF8));
            channel.WriteInbound(Unpooled.WrappedBuffer(new byte[] { (byte)0xE2, (byte)0x98, (byte)0xA2 }));
            string myObject = channel.ReadInbound<string>();
            Console.WriteLine(myObject);
        }
    }
}
