using McnLib.Structures;

namespace McnLib.States
{
    public class NoteFile
    {
        public string FullPath { get; set; } = "";
        public List<Block> Blocks { get; set; } = new List<Block>();
        public List<Section> Sections { get; set; } = new List<Section>();
    }
}
