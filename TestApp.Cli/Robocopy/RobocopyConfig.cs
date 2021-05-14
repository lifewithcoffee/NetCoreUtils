using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json.Serialization;

namespace TestApp.Cli.Robocopy
{
    public class RobocopyConfig
    {
        public List<BackupItemGroup> BackupItemGroups { get; set; } = new List<BackupItemGroup>();
    }
}
