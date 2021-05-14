using System.Collections.Generic;

namespace TestApp.Cli.Robocopy
{
    public class BackupItemGroup
    {
        public string Name { get; set; }
        public List<BackupItem> BackupItems { get; set; } = new List<BackupItem>();
    }
}
