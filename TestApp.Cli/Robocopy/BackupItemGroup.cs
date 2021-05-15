using System.Collections.Generic;

namespace TestApp.Cli.Robocopy
{
    public class BackupItemGroup
    {
        public string GroupName { get; set; }
        public List<BackupItem> Backups { get; set; } = new List<BackupItem>();
    }
}
