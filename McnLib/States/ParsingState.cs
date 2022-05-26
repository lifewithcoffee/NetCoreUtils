using McnLib.Structures;

namespace McnLib.States
{
    public class ParsingState
    {
        public NoteFile? CurrentFile { get; set; }
        public Note? CurrentNote { get; set; }
        public Section? CurrentSection { get; set; }
    }
}
