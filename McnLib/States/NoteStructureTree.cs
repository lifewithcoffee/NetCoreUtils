using McnLib.Structures;

namespace McnLib.States
{
    public class FindNotesResult
    {
        public string FilePath { get; set; } = "";

        public List<(Note Note, List<NoteLine> LinesFound)> NotesFound { get; set; } = new List<(Note, List<NoteLine>)>();
    }

    public class NoteStructureTree  // aka. NST
    {
        public List<NoteFile> NoteFiles { get; set; } = new List<NoteFile>();

        public List<Note> GetAllNotes()     // TODO: useless, remove this method
        {
            return NoteFiles!.SelectMany(f => f.Notes).ToList();
        }

        public List<FindNotesResult> FindNotes(string[] keywords, string[]? fileFilterWords = null)
        {
            List<FindNotesResult> results = new List<FindNotesResult>();

            // get files with their names contain all file filter words
            List<NoteFile> filteredFiles = NoteFiles;
            if(fileFilterWords != null)
                filteredFiles = NoteFiles.Where(f => fileFilterWords.All(w => f.Name.Trim().ToLower().Contains(w))).ToList();

            foreach(var file in filteredFiles)
            {
                FindNotesResult result = new FindNotesResult { FilePath = file.FullPath };
                file.Notes.ForEach(n => {
                    var lines = n.FindLinesAND(keywords);
                    if (lines != null)
                        result.NotesFound.Add((n, lines));
                });

                if(result.NotesFound.Count > 0)
                    results.Add(result);
            }
            return results;
        }
    }
}
