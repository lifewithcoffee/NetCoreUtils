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
        //public List<PathAlias> Pathes { get; set; } = new List<PathAlias>();
        public List<BackupItemGroup> BackupGroups { get; set; } = new List<BackupItemGroup>();

        //public PathAlias GetAlias(string name)
        //{
        //    foreach(var alias in Pathes)
        //    {
        //        if(alias.Name == name)
        //        {
        //            return alias;
        //        }
        //    }

        //    return null;
        //}

        //public void RemoveAlias(string name)
        //{
        //    Pathes.Remove(GetAlias(name));
        //}


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

        public void RenameGroup(string targetGroupName, string newName)
        {
            var group = this.GetGroup(targetGroupName);
            if (group != null)
                group.GroupName = newName;
            else
                Console.WriteLine($"Cannot find group: {targetGroupName}");
        }

        public void RemoveGroup(string groupName)
        {
            BackupGroups.Remove(this.GetGroup(groupName));
        }
    }
}
