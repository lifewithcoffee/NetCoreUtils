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

        /// <param name="keywords">The target searching keywords</param>
        /// <param name="fileFilterWords">Give a list of files that their file
        /// names (not inluce the pathes) contai all the specified filter
        /// words. The search will only apply to this list of files.</param>
        public List<FindNotesResult> FindNotes(string[] keywords, string[]? fileFilterWords = null)
        {
            List<FindNotesResult> results = new List<FindNotesResult>();

            List<NoteFile> filteredFiles = this.FindFilesAND(fileFilterWords);
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

        /// <summary>
        /// File files with their names contain all the keywords (AND logic)
        /// </summary>
        private List<NoteFile> FindFilesAND(string[]? keywords)
        {
            if(keywords != null)
                return NoteFiles.Where(f => keywords.All(w => f.Name.Trim().ToLower().Contains(w))).ToList();
            return NoteFiles;
        }

        // _working_ find notes only by file title
        private List<FindNotesResult> FindNoteByTitle(string[] strings, string[]? fileFilterWords = null)
        {
            var results = new List<FindNotesResult>();
            List<NoteFile> filteredFiles = this.FindFilesAND(fileFilterWords);
            foreach(var file in filteredFiles)
            {

            }

            return results;
        }
    }
}
