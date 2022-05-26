namespace McnLib.Structures
{

    /// <summary>
    /// A section is a header section in a file.
    /// Header format partially follows restructuredText's rules.
    /// </summary>
    public class Section
    {
        public string Title { get; set; } = "NewSection";
        public string? Id { get; set; }
        public List<Section> ChildSections { get; set; } = new List<Section>();
    }
}