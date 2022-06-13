using McnLib.Structures;

namespace McnLib.States
{
    public class FindNotesResult
    {
        public string FilePath { get; set; } = "";
        public List<NoteLine> LinesFound { get; set; } = new List<NoteLine>();
    }

    public class NoteStructureTree
    {
        public List<NoteFile> NoteFiles { get; set; } = new List<NoteFile>();

        public List<Note> GetAllNotes()     // TODO: useless, remove this method
        {
            return NoteFiles!.SelectMany(f => f.Notes).ToList();
        }

        public List<FindNotesResult> FindNotes(string[] keywords)
        {
            List<FindNotesResult> results = new List<FindNotesResult>();
            foreach(var file in NoteFiles)
            {
                FindNotesResult result = new FindNotesResult { FilePath = file.FullPath };
                file.Notes.ForEach(n => {
                    var lines = n.FindLinesAND(keywords);
                    if (lines != null)
                        result.LinesFound.AddRange(lines);
                });
            }
            return results;
        }
    }
}
