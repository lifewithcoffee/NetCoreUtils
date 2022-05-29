using McnLib.Structures;

namespace McnLib.States
{
    public class ParsingState
    {
        public NoteFile? CurrentFile { get; set; }
        public Note? CurrentNote { get; set; }
        public Section? CurrentSection { get; set; }

        public void ResetCurrentNote(bool isBareNote = false)
        {
            if (CurrentNote != null && CurrentNote.FileLines.Count != 0)
                CurrentFile!.Notes.Add(CurrentNote);
            CurrentNote = new Note { IsBare = isBareNote };
        }
    }
}
