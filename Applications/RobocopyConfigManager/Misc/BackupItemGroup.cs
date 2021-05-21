using System.Collections.Generic;

namespace RobocopyConfigManager.Misc
{
    public class BackupItemGroup
    {
        public string GroupName { get; set; }
        public List<BackupItem> BackupItems { get; set; } = new List<BackupItem>();
    }
}
