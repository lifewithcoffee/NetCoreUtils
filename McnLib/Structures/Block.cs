using McnLib.FileProcessing;

namespace McnLib.Structures
{
    /// <summary>
    /// A block is a piece of notes wrapped by line "[=..." and line "..._]".
    /// A block is the basic unit of multi-keywords searching.
    /// </summary>
    public class Block
    {
        public string? Title { get; set; }

        /// <summary>
        /// If Id is null or white spaces, return create time
        /// </summary>
        public string? Id { get; set; }

        /// <summary>
        /// The create time is parsed from the block ID. For a bare block, it's null.
        /// </summary>
        public DateTime? CreateTime { get; set; } = null;
        public DateTime? UpdateTime { get; set; } = null;

        /// <summary>
        /// A block section is a child structure inside a block
        /// [design decision notes] decide to remove parsing block section, since it's not used as a search structure and unnecessary to display a block's structure
        /// </summary>
        //public List<BlockSection> BlockSections { get; set; } = new List<BlockSection>();
        public List<FileLine> FileLines { get; set; } = new List<FileLine>();

        /// <summary>
        /// A bare block is several lines (next to each other) that do not belong to any delcared block
        /// </summary>
        public bool IsBare { get; set; } = false;

    }
}