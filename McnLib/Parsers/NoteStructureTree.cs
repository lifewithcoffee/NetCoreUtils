using McnLib.Structures;

namespace McnLib.Parsers
{
    public class NoteStructureTree
    {
        public List<NoteFile> NoteFiles { get; set; } = new List<NoteFile>();

        public List<Block> GetAllBlocks()
        {
            return NoteFiles.SelectMany(f => f.Blocks).ToList();
        }
    }
}
