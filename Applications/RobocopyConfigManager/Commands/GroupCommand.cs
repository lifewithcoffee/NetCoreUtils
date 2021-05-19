using CoreCmd.Attributes;
using NetCoreUtils.MethodCall;
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
                    foreach(var item in group.Backups)
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

        // TODO: group copy
        //[Help("Create a new backup group by copying from an existing one")]
        //public void Copy(string sourceGroupName, string targetGroupName)
        //{
        //}

        [Help("Rename a backup group")]
        public void Rename(string targetGroupName, string newName)
        {
            SafeCall.Execute(() => {
                config.RenameGroup(targetGroupName, newName);
                JsonConfigOperator<RobocopyConfig>.Save(RobocopyConfigParameters.fullConfigFilePath, config);
            });
        }
    }
}
