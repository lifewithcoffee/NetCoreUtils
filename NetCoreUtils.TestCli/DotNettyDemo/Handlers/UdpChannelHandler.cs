using DotNetty.Buffers;
using DotNetty.Transport.Channels;
using DotNetty.Transport.Channels.Sockets;
using Serilog;
using System;
using System.Collections.Generic;
using System.Text;

namespace NetCoreUtils.TestCli.DotNettyDemo.Handlers
{
    class UdpChannelHandler : SimpleChannelInboundHandler<object>
    {
        protected override void ChannelRead0(IChannelHandlerContext ctx, object msg)
        {
            Log.Information("UdpChannelHandler.ChannelRead0() called");

            var package = msg as DatagramPacket;
            if(package != null)
                Console.WriteLine("Received:" + package.Content.ToString(Encoding.UTF8));
        }

        public override void ChannelActive(IChannelHandlerContext context)
        {
            Log.Information("UdpChannelHandler.ChannelActive() called");
        }

        public override void ChannelReadComplete(IChannelHandlerContext context)
        {
            context.Flush();
        }

        public override void ChannelInactive(IChannelHandlerContext context)
        {
            Log.Information("UdpChannelHandler.ChannelInactive() called");
        }

        public override void ExceptionCaught(IChannelHandlerContext context, Exception exception)
        {
            Log.Error($"Error: {exception}");
        }
    }
}
