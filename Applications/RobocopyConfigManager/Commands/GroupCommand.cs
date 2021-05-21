using CoreCmd.Attributes;
using NetCoreUtils.MethodCall;
using NetCoreUtils.ProcessUtils;
using NetCoreUtils.Text.Json;
using RobocopyConfigManager.Misc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RobocopyConfigManager.Commands
{
    class GroupCommand
    {
        RobocopyConfig config = JsonConfigOperator<RobocopyConfig>.LoadCreate(RobocopyConfigParameters.fullConfigFilePath);

        [Help("Add a new backup group")]
        public void Add(string groupName)
        {
            SafeCall.Execute(() => {
                config.BackupGroups.Add(new BackupItemGroup { GroupName = groupName.Trim() });
                JsonConfigOperator<RobocopyConfig>.Save(RobocopyConfigParameters.fullConfigFilePath, config);
            });
        }

        [Help("List available backup groups")]
        public void List()
        {
            SafeCall.Execute(() => {
                foreach(var group in config.BackupGroups)
                {
                    Console.WriteLine(group.GroupName);
                }
            });
        }

        [Help("List backup items in the specified backup group")]
        public void List(string groupName)
        {
            SafeCall.Execute(() => {
                var group = config.GetGroup(groupName);
                if(group != null)
                {
                    foreach(var item in group.BackupItems)
                    {
                        Console.WriteLine($"Name: {item.BackupName}");
                        Console.WriteLine($"Source: {item.Source}");
                        Console.WriteLine($"Backup: {item.Target}");
                        Console.WriteLine("");
                    }
                }
            });
        }

        [Help("Remove a backup group")]
        public void Remove(string groupName)
        {
            SafeCall.Execute(() => {
                config.RemoveGroup(groupName);
                JsonConfigOperator<RobocopyConfig>.Save(RobocopyConfigParameters.fullConfigFilePath, config);
            });
        }

        [Help("Rename a backup group")]
        public void Rename(string targetGroupName, string newName)
        {
            SafeCall.Execute(() => {
                config.RenameGroup(targetGroupName, newName);
                JsonConfigOperator<RobocopyConfig>.Save(RobocopyConfigParameters.fullConfigFilePath, config);
            });
        }

        [Help("Sychronize from the source directory to the target directory")]
        public void Backup(string groupName)
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
                if (group.GroupName == groupName)
                {
                    string flags = $"/MT:16 /R:1 /W:3 /MIR /FFT /NP /LOG+:recover_{groupName}-{new Random().Next(0, 99999)}.log";

                    bool firstItem = true;
                    foreach (var backup in group.BackupItems)
                    {
                        if (!firstItem)
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
