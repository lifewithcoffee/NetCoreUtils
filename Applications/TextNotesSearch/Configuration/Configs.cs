using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextNotesSearch.Configuration
{
    public class FolderConfig
    {
        public string FolderPath { get; set; } = @"C:\__dell_sync_c\mcn\sync";
        public string FileExtentions { get; set; } = "mcn";
    }
    public class Configs
    {
        public FolderConfig DefaultFolder = new FolderConfig();
    }
}
