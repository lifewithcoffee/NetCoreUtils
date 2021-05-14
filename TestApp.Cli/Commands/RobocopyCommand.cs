using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestApp.Cli.Robocopy;

namespace TestApp.Cli.Commands
{

                        //new BackupItem { Source = "s1", Target = "t1" },
                        //new BackupItem { Source = "s2", Target = "t2" },
                        //new BackupItem { Source = "s3", Target = "t3" },
    class RobocopyCommand
    {
        public void AddGroup(string groupName)
        {
            try
            {
                Console.WriteLine($"{nameof(RobocopyCommand)}.{nameof(AddGroup)}() called");

                string fullConfigFilePath = @$"d:\_temp\{RobocopyConfigParameters.ConfigFileName}";

                var config = JsonConfigOperator<RobocopyConfig>.LoadCreate(fullConfigFilePath);
                config.BackupItemGroups.Add( new BackupItemGroup { Name = groupName  });
                JsonConfigOperator<RobocopyConfig>.Save(fullConfigFilePath, config);
            }catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                if(ex.InnerException != null)
                    Console.WriteLine(ex.InnerException.Message);
                Console.WriteLine(ex.StackTrace);
            }
        }
    }
}
