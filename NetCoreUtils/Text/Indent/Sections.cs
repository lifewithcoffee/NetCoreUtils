using System.Collections.Generic;
using System.Linq;

namespace NetCoreUtils.Text.Indent
{
    public class Sections
    {
        private List<Section> sections = new List<Section>();

        private string generalIndentSpaces;

        public Sections(int indentNumber = 0)
        {
            if (indentNumber != 0)
                generalIndentSpaces = " ".PadLeft(indentNumber);
            else
                generalIndentSpaces = "";
        }

        public SectionContent AddSection(string name)
        {
            var section = new Section(name);
            this.sections.Add(section);
            return section.Content;
        }

        public void Print()
        {
            const int spaceNumber = 3;
            var indentNumber = sections.Select(k => k.Header.Length).Max() + spaceNumber;
            string indentSpaces = " ".PadLeft(indentNumber);

            foreach (var section in sections)
            {
                section.Print(indentNumber, generalIndentSpaces, indentSpaces);
            }
        }
    }
}
