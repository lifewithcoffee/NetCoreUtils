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
    class TcpServer : IExecutable
    {
        public async Task Execute()
        {
            IEventLoopGroup bossGroup = new MultithreadEventLoopGroup(1);
            IEventLoopGroup workerGroup = new MultithreadEventLoopGroup();

            IChannel boundChannel = await new ServerBootstrap()
                .Group(bossGroup, workerGroup)
                .Channel<TcpServerSocketChannel>()
                .Option(ChannelOption.SoBacklog, 1024)
                .Option(ChannelOption.TcpNodelay, true)     // newly added
                //.Handler(new LoggingHandler("SRV-LSTN"))
                .ChildHandler(new ActionChannelInitializer<IChannel>(channel =>
                {
                    IChannelPipeline pipeline = channel.Pipeline;
                    //pipeline.AddLast(new LoggingHandler("SRV-CONN"));
                    //pipeline.AddLast("framing-enc", new LengthFieldPrepender(2));
                    //pipeline.AddLast("framing-dec", new LengthFieldBasedFrameDecoder(ushort.MaxValue, 0, 2, 0, 2));       // TODO: why no data will be received if enable this statement?

                    pipeline.AddLast(new GeneralChannelHandler());
                })).BindAsync(18007);

            Console.ReadLine();
            await boundChannel.CloseAsync();
        }
    }
}
