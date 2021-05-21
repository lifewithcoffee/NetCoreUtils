using NetCoreUtils.Text.Json;
using RobocopyConfigManager.Misc;
using System;

namespace RobocopyConfigManager.Commands
{
    class ItemCommand
    {
        RobocopyConfig config = JsonConfigOperator<RobocopyConfig>.LoadCreate(RobocopyConfigParameters.fullConfigFilePath);

        /// <summary>
        /// If the specified item does not exist in the specified group, a new item will be created.
        /// 
        /// Multiple sources cannot point to the same target.
        ///
        /// TODO: backup update
        /// - group name shall be unique
        /// - names in a group shall be unique
        /// </summary>
        /// <param name="fullBackupItemName">Backup item name with group prefix, e.g. SomeGroupName.SomeBackupName</param>
        /// <param name="source">Backup source</param>
        /// <param name="target">Backup target</param>
        public void Update(string groupName, string itemName, string source, string target)
        {
            var group = config.GetGroup(groupName);
            if (group != null)
            {
                // validate to make sure there is not any existing backup configured with the same target
                string lowerTarget = target.ToLower();
                foreach(var backupItem in group.BackupItems)
                {
                    // skip over the specified item, only apply validation to other items
                    if(backupItem.BackupName != itemName)
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
                if(item != null)
                {
                    item.Source = source;
                    item.Target = lowerTarget;
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

            if(group != null)
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

            if(group != null)
            {
                // check if an backup item with the same new name has existed
                foreach(var backupItem in group.BackupItems)
                {
                    if (backupItem.BackupName == newItemName)
                    {
                        Console.WriteLine($"An backup item with the same new name has existed.");
                        return;
                    }
                }

                // do renaming
                var item = group.GetItem(currentItemName);
                if(item != null)
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
