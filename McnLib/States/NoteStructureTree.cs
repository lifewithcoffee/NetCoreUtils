using McnLib.Structures;

namespace McnLib.States
{
    public class NoteStructureTree
    {
        public List<NoteFile>? NoteFiles { get; set; }

        public List<Note> GetAllNotes()
        {
            return NoteFiles!.SelectMany(f => f.Notes).ToList();
        }
    }
}
