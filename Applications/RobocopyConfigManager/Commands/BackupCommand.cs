using NetCoreUtils.ProcessUtils;
using NetCoreUtils.Text.Json;
using RobocopyConfigManager.Misc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestApp.Cli.Commands
{
    class BackupCommand
    {
        // TODO: backup the default group
        public void Default()
        {
            Console.WriteLine("backup default");
        }

        // TODO: backup item
        public void Item(string fullBackupItemName)
        {
            Console.WriteLine($"backup item: {fullBackupItemName}");
        }

        public void Group(string groupName)
        {
            const string robocopy = "robocopy";
            if (!ProcUtil.Exists(robocopy))
            {
                Console.Error.WriteLine($"Cannot find {robocopy}, program terminated");
                return;
            }

            var config = JsonConfigOperator<RobocopyConfig>.LoadCreate(RobocopyConfigParameters.fullConfigFilePath);

            foreach (var group in config.BackupGroups)
            {
                if(group.GroupName == groupName)
                {
                    string flags = $"/MT:16 /R:1 /W:3 /MIR /FFT /NP /LOG+:recover_{groupName}-{new Random().Next(0,99999)}.log";

                    bool firstItem = true;
                    foreach(var backup in group.BackupItems)
                    {
                        if(!firstItem)
                        {
                            Console.WriteLine("-----------------------------------------------");
                        }
                        string arguments = $"\"{backup.Source}\" \"{backup.Target}\" {flags}";
                        Console.WriteLine($"Executing: {robocopy} {arguments}");
                        ProcUtil.Run(robocopy, arguments);
                        firstItem = false;
                    }
                }
            }
        }
    }
}
