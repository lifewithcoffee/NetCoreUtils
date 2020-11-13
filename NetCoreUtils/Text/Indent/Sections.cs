using System;
using System.Collections.Generic;
using System.Linq;

namespace NetCoreUtils.Text.Indent
{
    public class Section
    {
        public string Header { get; set; }
        public SectionContent Content { get; set; } = new SectionContent();

        public Section(string header)
        {
            this.Header = header;
        }

        public void Print(int indentNumber, string generalIndentSpaces, string indentSpaces)
        {
            int counter = 0;
            foreach (var line in Content.LineList)
            {
                if (counter++ == 0)
                {
                    string header = Header.PadRight(indentNumber);
                    Console.WriteLine($"{generalIndentSpaces}{header}{line}");
                }
                else
                {
                    Console.WriteLine($"{generalIndentSpaces}{indentSpaces}{line}");
                }
            }
        }
    }

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
