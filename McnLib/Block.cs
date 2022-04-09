namespace McnLib
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
    }
}