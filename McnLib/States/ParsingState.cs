using McnLib.Structures;

namespace McnLib.States
{
    public class ParsingState
    {
        public NoteFile? CurrentFile { get; set; }
        public Note? CurrentNote { get; private set; }
        public int CurrentNoteLineCount { get; set; } = 0;
        public Section? CurrentSection { get; set; }


        public void ResetCurrentNote(bool isBareNote = false)
        {
            this.CurrentNote = new Note { IsBare = isBareNote };
            this.CurrentFile!.Notes.Add(this.CurrentNote);
            this.CurrentNoteLineCount = 0;
        }

        public void AddLine(NoteLine line)
        {
            this.CurrentNote!.FileLines.Add(line);
            this.CurrentNoteLineCount++;
        }
    }
}
