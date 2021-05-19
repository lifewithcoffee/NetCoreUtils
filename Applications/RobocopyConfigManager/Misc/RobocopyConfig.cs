using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json.Serialization;

namespace RobocopyConfigManager.Misc
{
    public class RobocopyConfig
    {
        public List<BackupItemGroup> BackupGroups { get; set; } = new List<BackupItemGroup>();

        public BackupItemGroup GetGroup(string groupName)
        {
            foreach(var group in BackupGroups)
            {
                if(group.GroupName == groupName)
                {
                    return group;
                }
            }

            return null;
        }

        public void RemoveGroup(string groupName)
        {
            BackupGroups.Remove(this.GetGroup(groupName));
        }

        public BackupItem GetItem(string fullBackupItemName)
        {
            var names = fullBackupItemName.Split('.');
            if (names.Length > 2)
            {
                throw new Exception($"Invalid full backup item name: {fullBackupItemName}");
            }
            string groupName = names[0];
            string backupItemName = names[1];

            var group = this.GetGroup(groupName);
            if(group != null)
            {
                foreach(var item in group.Backups)
                {
                    if(item.BackupName == backupItemName)
                    {
                        return item;
                    }
                }
            }

            return null;
        }
    }
}
