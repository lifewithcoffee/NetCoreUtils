using DotNetty.Codecs;
using DotNetty.Handlers.Logging;
using DotNetty.Transport.Bootstrapping;
using DotNetty.Transport.Channels;
using DotNetty.Transport.Channels.Sockets;
using TestApp.Cli.DotNettyDemo.Handlers;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace TestApp.Cli.DotNettyDemo.Clients
{
    class EchoClient : IExecutable
    {
        public async Task ExecuteAsync()
        {
            var group = new MultithreadEventLoopGroup();

            var bootstrap = new Bootstrap();
            bootstrap
                .Group(group)
                .Channel<TcpSocketChannel>()
                .Option(ChannelOption.TcpNodelay, true)
                .Handler(new ActionChannelInitializer<ISocketChannel>(channel =>
                {
                    IChannelPipeline pipeline = channel.Pipeline;

                    pipeline.AddLast(new LoggingHandler());
                    pipeline.AddLast("framing-enc", new LengthFieldPrepender(2));
                    pipeline.AddLast("framing-dec", new LengthFieldBasedFrameDecoder(ushort.MaxValue, 0, 2, 0, 2));

                    pipeline.AddLast("echo", new EchoClientHandler());

                }));

            IChannel clientChannel = await bootstrap.ConnectAsync(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 18007));
            Console.ReadLine();
            await clientChannel.CloseAsync();
        }
    }
}
