using System;
using System.Collections.Generic;
using System.Linq;

namespace NetCoreUtils.Text.Indent
{
    public class Section
    {
        public string Header { get; set; }
        public Lines Lines { get; set; }
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

        public Lines AddSection(string name)
        {
            var lines = new Lines();
            this.sections.Add(new Section { Header = name, Lines = lines });
            return lines;
        }

        public void Print()
        {
            const int spaceNumber = 3;
            var indentNumber = sections.Select(k => k.Header.Length).Max() + spaceNumber;
            string indentSpaces = " ".PadLeft(indentNumber);

            foreach (var section in sections)
            {
                int counter = 0;
                foreach (var line in section.Lines.LineList)
                {
                    if (counter++ == 0)
                    {
                        string header = section.Header.PadRight(indentNumber);
                        Console.WriteLine($"{generalIndentSpaces}{header}{line}");
                    }
                    else
                    {
                        Console.WriteLine($"{generalIndentSpaces}{indentSpaces}{line}");
                    }
                }
            }
        }
    }
}
