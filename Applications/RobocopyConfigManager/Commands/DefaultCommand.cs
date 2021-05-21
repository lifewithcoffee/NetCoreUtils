using CoreCmd.Attributes;
using NetCoreUtils.MethodCall;
using NetCoreUtils.ProcessUtils;
using NetCoreUtils.Text.Json;
using RobocopyConfigManager.Misc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RobocopyConfigManager.Commands
{
    class DefaultCommand
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

        private bool AreValidSourceAndTarget(string source, string target)
        {
            if (!System.IO.Directory.Exists(source))
            {
                Console.WriteLine($"Invalid source path: {source}");
                return false;
            }

            if (!System.IO.Directory.Exists(target))
            {
                Console.WriteLine($"Invalid target path: {target}");
                return false;
            }

            string normalizedSource = new DirectoryInfo(source).FullName.Trim('\\');
            string normalizedTarget = new DirectoryInfo(target).FullName.Trim('\\');

            if (normalizedSource == normalizedTarget)
            {
                Console.WriteLine($"The source and target pathes should be different.");
                return false;
            }

            return true;
        }

        /// <summary>
        /// - If the specified item does not exist in the specified group, a new item will be created.
        /// - Multiple sources cannot point to the same target.
        /// </summary>
        /// <param name="fullBackupItemName">Backup item name with group prefix, e.g. SomeGroupName.SomeBackupName</param>
        /// <param name="source">Backup source</param>
        /// <param name="target">Backup target</param>
        public void Update(string groupName, string itemName, string source, string target)
        {
            if (!AreValidSourceAndTarget(source, target))
                return;

            var group = config.GetGroup(groupName);
            if (group != null)
            {
                // validate to make sure there is not any existing backup configured with the same target
                string lowerTarget = target.ToLower();
                foreach (var backupItem in group.BackupItems)
                {
                    // skip over the specified item, only apply validation to other items
                    if (backupItem.BackupName != itemName)
                    {
                        if (backupItem.Target.ToLower() == lowerTarget)
                        {
                            Console.WriteLine("A backup item with the same target has already existed.");
                            return;
                        }
                    }
                }

                // do update
                var item = group.GetItem(itemName);
                if (item != null)
                {
                    item.Source = source;
                    item.Target = target;
                }
                else
                {
                    Console.WriteLine($"The specified backup item '{itemName}' does not exist. A new backup item is created.");
                    group.BackupItems.Add(new BackupItem { BackupName = itemName, Source = source, Target = target });
                }
            }
            else
            {
                Console.WriteLine($"The specified group '{groupName}' does not exist.");
            }

            JsonConfigOperator<RobocopyConfig>.Save(RobocopyConfigParameters.fullConfigFilePath, config);
        }

        public void Remove(string groupName, string itemName)
        {
            var group = config.GetGroup(groupName);

            if (group != null)
            {
                group.BackupItems.Remove(group.GetItem(itemName));
            }
            else
            {
                Console.WriteLine($"The specified group '{groupName}' does not exist.");
            }

            JsonConfigOperator<RobocopyConfig>.Save(RobocopyConfigParameters.fullConfigFilePath, config);
        }

        public void Rename(string groupName, string currentItemName, string newItemName)
        {
            var group = config.GetGroup(groupName);

            if (group != null)
            {
                // check if an backup item with the same new name has existed
                foreach (var backupItem in group.BackupItems)
                {
                    if (backupItem.BackupName == newItemName)
                    {
                        Console.WriteLine($"An backup item with the same new name has existed.");
                        return;
                    }
                }

                // do renaming
                var item = group.GetItem(currentItemName);
                if (item != null)
                {
                    item.BackupName = newItemName;
                }
                else
                {
                    Console.WriteLine($"Cannot find the specified backup item '{currentItemName}'");
                    return;
                }
            }
            else
            {
                Console.WriteLine($"The specified group '{groupName}' does not exist.");
            }

            JsonConfigOperator<RobocopyConfig>.Save(RobocopyConfigParameters.fullConfigFilePath, config);
        }
    }
}
