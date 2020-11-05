using CoreCmd.Attributes;
using NetCoreUtils.TestCli.FifoDemo;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace NetCoreUtils.TestCli.Commands
{
    [Help("Demo of threading channels")]
    class FifoCommand
    {
        ThreadChannelService _threadChannel = new ThreadChannelService();

        public void Default()
        {
            Task.Run(async () =>
            {
                await _threadChannel.ChannelRun(3000, 1000, 2, 5, 3);
            }).Wait();
        }
    }
}
