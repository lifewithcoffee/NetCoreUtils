namespace McnLib.Structures
{
    public class Section
    {
        public string Title { get; set; } = "NewSection";
        public string? Id { get; set; }
        public List<Section> ChildSections { get; set; } = new List<Section>();
    }
}