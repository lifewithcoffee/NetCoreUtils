using NetCoreUtils.Text.Json;
using RobocopyConfigManager.Misc;
using System;

namespace RobocopyConfigManager.Commands
{
    class ItemCommand
    {
        RobocopyConfig config = JsonConfigOperator<RobocopyConfig>.LoadCreate(RobocopyConfigParameters.fullConfigFilePath);

        /// <summary>
        /// TODO: backup update
        /// - group name shall be unique
        /// - names in a group shall be unique
        /// - update-group shall work even if the group doesn't exist
        ///
        /// Done:
        /// - multiple sources shouldn't point to the same target
        /// </summary>
        /// <param name="fullBackupItemName">Backup item name with group prefix, e.g. SomeGroupName.SomeBackupName</param>
        /// <param name="source">Backup source</param>
        /// <param name="target">Backup target</param>
        public void Update(string fullBackupItemName, string source, string target)
        {
            try
            {
                // TODO: to be refactored using RobocopyConfig.GetGroup/GetItem

                string trimmedGroupName = fullBackupItemName.Trim();
                var names = trimmedGroupName.Split('.');
                if (names.Length > 2)
                {
                    throw new Exception($"Invalid full backup item name: {fullBackupItemName}");
                }
                string groupName = names[0];
                string backupItemName = names[1];

                // find the group with the specified name
                foreach (var group in config.BackupGroups)
                {
                    if (group.GroupName == groupName)
                    {
                        bool foundTargetItem = false;

                        // find the item with the same target
                        foreach (var item in group.BackupItems)
                        {
                            if (item.Target == target)
                            {
                                item.BackupName = backupItemName;
                                item.Source = source;
                                foundTargetItem = true;
                            }
                        }

                        if (!foundTargetItem)
                        {
                            group.BackupItems.Add(new BackupItem { BackupName = backupItemName, Source = source, Target = target });
                        }
                    }
                }

                JsonConfigOperator<RobocopyConfig>.Save(RobocopyConfigParameters.fullConfigFilePath, config);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public void Remove(string fullBackupItemName)
        {
            var (group, item) = config.ConvertToGroupAndItem(fullBackupItemName);

            if(group != null)
            {
                group.BackupItems.Remove(item);
            }

            JsonConfigOperator<RobocopyConfig>.Save(RobocopyConfigParameters.fullConfigFilePath, config);
        }
    }
}
