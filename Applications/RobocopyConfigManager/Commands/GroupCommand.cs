using CoreCmd.Attributes;
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

        [Help("Create a new backup group")]
        public void Create(string groupName)
        {
            try
            {
                config.BackupGroups.Add(new BackupItemGroup { GroupName = groupName.Trim() });
                JsonConfigOperator<RobocopyConfig>.Save(RobocopyConfigParameters.fullConfigFilePath, config);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                if (ex.InnerException != null)
                    Console.WriteLine(ex.InnerException.Message);
                Console.WriteLine(ex.StackTrace);
            }
        }

        // TODO
        [Help("List available backup groups")]
        public void List()
        {

        }

        [Help("Remove a backup group")]
        public void Remove(string groupName)
        {

        }

        [Help("Create a new backup group by copying from an existing one")]
        public void Copy(string sourceGroupName, string targetGroupName)
        {

        }
    }
}
