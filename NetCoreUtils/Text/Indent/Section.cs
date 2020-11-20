using System;

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
}
