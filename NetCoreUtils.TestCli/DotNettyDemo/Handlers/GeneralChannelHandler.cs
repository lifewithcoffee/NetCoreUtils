using DotNetty.Buffers;
using DotNetty.Transport.Channels;
using DotNetty.Transport.Channels.Sockets;
using Serilog;
using System;
using System.Collections.Generic;
using System.Text;

namespace NetCoreUtils.TestCli.DotNettyDemo.Handlers
{
    class GeneralChannelHandler : SimpleChannelInboundHandler<object>
    {
        protected override void ChannelRead0(IChannelHandlerContext ctx, object msg)
        {
            Log.Information("GeneralChannelHandler.ChannelRead0() called");

            // for receiving UDP data
            var packet = msg as DatagramPacket;
            if(packet != null)
                Console.WriteLine("Received DatagramPacket:" + packet.Content.ToString(Encoding.UTF8));

            // for receiving TCP data
            var byteBuffer = msg as IByteBuffer;
            if (byteBuffer != null)
                Console.WriteLine("Received IByteBuffer: " + byteBuffer.ToString(Encoding.UTF8));

        }

        public override void ChannelRead(IChannelHandlerContext ctx, object msg)
        {
            base.ChannelRead(ctx, msg);
            ctx.FireChannelRead(msg);
        }

        public override void ChannelActive(IChannelHandlerContext context)
        {
            Log.Information("GeneralChannelHandler.ChannelActive() called");
            context.FireChannelActive();
        }

        public override void ChannelReadComplete(IChannelHandlerContext context)
        {
            context.Flush();
        }

        public override void ChannelInactive(IChannelHandlerContext context)
        {
            Log.Information("GeneralChannelHandler.ChannelInactive() called");
        }

        public override void ExceptionCaught(IChannelHandlerContext context, Exception exception)
        {
            Log.Error($"Error: {exception}");
        }
    }

    class GeneralChannelHandler2 : SimpleChannelInboundHandler<object>
    {
        public override void ChannelActive(IChannelHandlerContext context)
        {
            Log.Information("GeneralChannelHandler2.ChannelActive() called");
        }

        protected override void ChannelRead0(IChannelHandlerContext ctx, object msg)
        {
            Log.Information("GeneralChannelHandler2.ChannelRead0() called");
        }
    }
}
