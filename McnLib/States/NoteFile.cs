using McnLib.Structures;

namespace McnLib.States
{
    public class NoteFile
    {
        public string FullPath { get; set; } = string.Empty;

        /// <summary>
        /// File name without path
        /// </summary>
        public string Name { get; set; } = string.Empty;

        public string[] Content { get; set; } = new string[0];

        /// <summary>
        /// All notes of this file.
        /// </summary>
        public List<Note> Notes { get; set; } = new List<Note>();

        public List<Section> Sections { get; set; } = new List<Section>();
    }
}
