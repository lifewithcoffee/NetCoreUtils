using McnLib.Structures;

namespace McnLib.States
{
    public class NoteFile
    {
        public string FullPath { get; set; } = "";

        /// <summary>
        /// All notes of this file.
        /// </summary>
        public List<Note> Notes { get; set; } = new List<Note>();

        public List<Section> Sections { get; set; } = new List<Section>();
    }
}
