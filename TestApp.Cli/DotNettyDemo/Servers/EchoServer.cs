using DotNetty.Codecs;
using DotNetty.Handlers.Logging;
using DotNetty.Transport.Bootstrapping;
using DotNetty.Transport.Channels;
using DotNetty.Transport.Channels.Sockets;
using NetCoreUtils.TestCli.DotNettyDemo.Handlers;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace NetCoreUtils.TestCli.DotNettyDemo.Servers
{
    class EchoServer : IExecutable
    {
        public async Task ExecuteAsync()
        {
            IEventLoopGroup bossGroup = new MultithreadEventLoopGroup(1);
            IEventLoopGroup workerGroup = new MultithreadEventLoopGroup();

            var bootstrap = new ServerBootstrap();
            bootstrap.Group(bossGroup, workerGroup);

            bootstrap.Channel<TcpServerSocketChannel>();
            bootstrap
                .Option(ChannelOption.SoBacklog, 100)
                .Handler(new LoggingHandler("SRV-LSTN"))
                .ChildHandler(new ActionChannelInitializer<IChannel>(channel =>
                {
                    IChannelPipeline pipeline = channel.Pipeline;
                    pipeline.AddLast(new LoggingHandler("SRV-CONN"));
                    pipeline.AddLast("framing-enc", new LengthFieldPrepender(2));
                    pipeline.AddLast("framing-dec", new LengthFieldBasedFrameDecoder(ushort.MaxValue, 0, 2, 0, 2));

                    pipeline.AddLast("echo", new EchoServerHandler());
                }));

            IChannel boundChannel = await bootstrap.BindAsync(18007);
            Console.ReadLine();
            await boundChannel.CloseAsync();
        }
    }
}
