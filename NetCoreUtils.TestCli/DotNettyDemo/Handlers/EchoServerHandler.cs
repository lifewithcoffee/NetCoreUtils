using DotNetty.Transport.Channels;

namespace NetCoreUtils.TestCli.DotNettyDemo.Handlers
{
    using System;
    using System.Text;
    using DotNetty.Buffers;
    using DotNetty.Transport.Channels;
    using DotNetty.Transport.Channels.Sockets;

    public class EchoServerHandler : ChannelHandlerAdapter
    {
        public override void ChannelActive(IChannelHandlerContext context)
        {
            base.ChannelActive(context);

            Console.WriteLine($"EchoServerHandler.ChannelActive() called: context = {context}");
        }

        public override void ChannelInactive(IChannelHandlerContext context)
        {
            base.ChannelInactive(context);

            Console.WriteLine($"EchoServerHandler.ChannelInactive() called: context = {context}");
        }

        public override void ChannelRead(IChannelHandlerContext context, object message)
        {
            Console.WriteLine($"Received: {message.ToString()}");

            var buffer = message as IByteBuffer;
            if (buffer != null)
            {
                Console.WriteLine("Received from client: " + buffer.ToString(Encoding.UTF8));
            }
            context.WriteAsync(message);
        }

        public override void ChannelReadComplete(IChannelHandlerContext context) => context.Flush();

        public override void ExceptionCaught(IChannelHandlerContext context, Exception exception)
        {
            Console.WriteLine("Exception: " + exception);
            context.CloseAsync();
        }
    }
}