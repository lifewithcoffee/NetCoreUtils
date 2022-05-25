using McnLib.Structures;

namespace McnLib.Parsers
{
    public class ParsingState
    {
        public NoteFile? CurrentFile { get; set; }
        public Block? CurrentBlock { get; set; }
        public Section? CurrentSection { get; set; }
    }
}
