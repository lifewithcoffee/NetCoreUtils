﻿using McnLib.Structures;

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

        public List<FindNotesResult> FindNotes(string[] keywords)
        {
            List<FindNotesResult> results = new List<FindNotesResult>();
            foreach(var file in NoteFiles)
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
