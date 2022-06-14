using McnLib.States;

namespace McnLib.Structures
{
    /// <summary>
    /// A note is a piece of notes wrapped by line "[=..." and line "..._]".
    /// A note is the basic unit of multi-keywords searching.
    /// </summary>
    public class Note
    {
        public string? Title { get; set; }

        /// <summary>
        /// If Id is null or white spaces, return create time
        /// </summary>
        public string? Id { get; set; }

        /// <summary>
        /// The create time is parsed from the note ID. For a bare note, it's null.
        /// </summary>
        public DateTime? CreateTime { get; set; } = null;

        public DateTime? UpdateTime { get; set; } = null;

        public List<NoteLine> FileLines { get; set; } = new List<NoteLine>();

        /// <summary>
        /// A bare note is several lines (next to each other) that do not belong to any delcared note
        /// </summary>
        public bool IsBare { get; set; } = false;

        /// <summary>
        /// Find lines with all input keywords, i.e. apply the 'AND' but not 'OR' logic
        /// </summary>
        public List<NoteLine>? FindLinesAND(string[] keywords)
        {
            var array = keywords.Select(k => k.Trim().ToLowerInvariant()).Distinct().ToArray();
            var list = array.ToList();

            var lines = FileLines.FindAll(fileLine =>
            {
                var line = fileLine.Text.ToLowerInvariant();
                bool result = false;
                for (int i = 0; i < array.Length; i++)
                {
                    if (line.Contains(array[i]))
                    {
                        list.Remove(array[i]);
                        result = true;
                    }
                }
                return result;
            }).ToList();

            if (list.Count == 0)
                return lines;
            else
                return null;
        }
    }
}