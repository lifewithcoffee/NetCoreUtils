using McnLib.FileProcessing;

namespace McnLib.Blocks
{
    /// <summary>
    /// A block is a piece of notes wrapped by line "[=..." and line "..._]"
    /// </summary>
    public class Block
    {
        public string? Title { get; set; }

        /// <summary>
        /// If Id is null or white spaces, return create time
        /// </summary>
        public string? Id { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime? UpdateTime { get; set; }
        public string? Content { get; set; }
        public List<BlockSection> BlockSections { get; set; } = new List<BlockSection>();
        public List<FileLine> FileLines { get; set; } = new List<FileLine>();
    }
}