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
    class UdpServer : IExecutable
    {
        public async Task ExecuteAsync()
        {
            var clientChannel = await new Bootstrap()
                .Group(new MultithreadEventLoopGroup())
                .Channel<SocketDatagramChannel>()
                .Option(ChannelOption.SoBroadcast, true)
                .Handler(new ActionChannelInitializer<IChannel>(channel =>
                {
                    IChannelPipeline pipeline = channel.Pipeline;
                    pipeline.AddLast(new GeneralChannelHandler());
                }))
                .BindAsync(12808);

            Console.ReadLine();
            await clientChannel.CloseAsync();
        }
    }
}
