using System;
using System.Collections.Generic;

namespace NetCoreUtils.Text.Indent
{
    public class SectionContent
    {
        public List<string> LineList { get; } = new List<string>();

        private string generalIndentSpaces;

        public SectionContent(int indentNumber = 0)
        {
            if (indentNumber != 0)
                generalIndentSpaces = " ".PadLeft(indentNumber);
            else
                generalIndentSpaces = "";
        }

        public SectionContent AddLine(string line)
        {
            LineList.Add(line);
            return this;
        }

        public void Print()
        {
            foreach (var line in LineList)
            {
                Console.WriteLine($"{generalIndentSpaces}{line}");
            }
        }
    }
}
