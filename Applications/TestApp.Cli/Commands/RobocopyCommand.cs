using NetCoreUtils.ProcessUtils;
using NetCoreUtils.Text.Json;
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
                config.BackupGroups.Add( new BackupItemGroup { GroupName = groupName.Trim()  });
                JsonConfigOperator<RobocopyConfig>.Save(fullConfigFilePath, config);
            }catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                if(ex.InnerException != null)
                    Console.WriteLine(ex.InnerException.Message);
                Console.WriteLine(ex.StackTrace);
            }
        }


        /// <summary>
        /// TODO:
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
        public void UpdateGroup(string fullBackupItemName, string source, string target)
        {
            try
            {
                Console.WriteLine($"{nameof(RobocopyCommand)}.{nameof(UpdateGroup)}() called");

                var config = JsonConfigOperator<RobocopyConfig>.LoadCreate(fullConfigFilePath);

                string trimmedGroupName = fullBackupItemName.Trim();
                var names = trimmedGroupName.Split('.');
                if(names.Length > 2)
                {
                    throw new Exception($"Invalid full backup item name: {fullBackupItemName}");
                }
                string groupName = names[0];
                string backupItemName = names[1];

                // find the group with the specified name
                foreach(var group in config.BackupGroups)
                {
                    if(group.GroupName == groupName)
                    {
                        bool foundTargetItem = false;

                        // find the item with the same target
                        foreach(var item in group.Backups)
                        {
                            if(item.Target == target)
                            {
                                item.BackupName = backupItemName;
                                item.Source = source;
                                foundTargetItem = true;
                            }
                        }

                        if (!foundTargetItem)
                        {
                            group.Backups.Add(new BackupItem { BackupName = backupItemName, Source = source, Target = target });
                        }
                    }
                }

                JsonConfigOperator<RobocopyConfig>.Save(fullConfigFilePath, config);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public void ExecuteBackup(string groupName)
        {
            const string robocopy = "robocopy";
            if (!ProcUtil.Exists(robocopy))
            {
                Console.Error.WriteLine($"Cannot find {robocopy}, program terminated");
                return;
            }

            var config = JsonConfigOperator<RobocopyConfig>.LoadCreate(fullConfigFilePath);

            foreach (var group in config.BackupGroups)
            {
                if(group.GroupName == groupName)
                {
                    string flags = $"/MT:16 /R:1 /W:3 /MIR /FFT /TEE /NP /LOG+:recover_{groupName}-{new Random().Next(0,99999)}.log";
                    foreach(var backup in group.Backups)
                    {
                        string arguments = $"{backup.Source} {backup.Target} {flags}";
                        Console.WriteLine($"Executing: {robocopy} {arguments}");
                        //ProcUtil.Run(robocopy, arguments);
                    }
                }
            }
        }

        public void Test2(string command, string arguments)
        {
            Console.WriteLine(">>>");
            ProcUtil.Run(command, arguments);
            Console.WriteLine("<<<");
        }
    }
}
