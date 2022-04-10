using McnLib.FileReader;

namespace McnLib.Blocks
{
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