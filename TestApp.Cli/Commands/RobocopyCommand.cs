using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestApp.Cli.Robocopy;

namespace TestApp.Cli.Commands
{
    class RobocopyCommand
    {
        string fullConfigFilePath = @$"d:\_temp\{RobocopyConfigParameters.ConfigFileName}";

        public void AddGroup(string groupName)
        {
            try
            {
                Console.WriteLine($"{nameof(RobocopyCommand)}.{nameof(AddGroup)}() called");

                var config = JsonConfigOperator<RobocopyConfig>.LoadCreate(fullConfigFilePath);
                config.BackupItemGroups.Add( new BackupItemGroup { Name = groupName.Trim()  });
                JsonConfigOperator<RobocopyConfig>.Save(fullConfigFilePath, config);
            }catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                if(ex.InnerException != null)
                    Console.WriteLine(ex.InnerException.Message);
                Console.WriteLine(ex.StackTrace);
            }
        }

        // TODO:
        // - multiple sources shouldn't point to the same target
        // - group name should be unique
        public void UpdateGroup(string groupName, string source, string target)
        {
            Console.WriteLine($"{nameof(RobocopyCommand)}.{nameof(UpdateGroup)}() called");

            var config = JsonConfigOperator<RobocopyConfig>.LoadCreate(fullConfigFilePath);

            string trimmedGroupName = groupName.Trim(); 

            // find the group with the specified name
            foreach(var group in config.BackupItemGroups)
            {
                if(group.Name == trimmedGroupName)
                {
                    bool foundTargetItem = false;

                    // find the item with the same target
                    foreach(var item in group.BackupItems)
                    {
                        if(item.Target == target)
                        {
                            item.Source = source;
                            foundTargetItem = true;
                        }
                    }

                    if (!foundTargetItem)
                    {
                        group.BackupItems.Add(new BackupItem { Source = source, Target = target });
                    }
                }
            }

            JsonConfigOperator<RobocopyConfig>.Save(fullConfigFilePath, config);
        }
    }
}
