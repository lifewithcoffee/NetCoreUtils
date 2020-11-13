using NetCoreUtils.Text.Indent;
using System;
using System.Collections.Generic;
using System.Text;

namespace TestApp.Cli.Commands.Text
{
    class IndentInfoCommand
    {

        private void AddSectionContent(Sections sections)
        {
            sections.AddSection("file")
                    .AddLine("bla bla bla bla bla bla bla bla bla bla bla bla bla bla bla bla bla bla")
                    .AddLine("bla bla bla bla bla bla bla bla bla bla bla bla bla bla bla bla bla bla")
                    .AddLine("bla bla bla bla bla bla bla bla bla bla bla bla bla");

            sections.AddSection("project")
                    .AddLine("bla bla bla bla bla bla bla bla bla bla bla bla bla bla bla bla bla bla")
                    .AddLine("bla bla bla bla bla bla bla bla bla bla bla bla bla bla bla bla bla bla")
                    .AddLine("bla bla bla bla bla bla bla bla bla bla bla bla bla");

            sections.AddSection("extension")
                    .AddLine("bla bla bla bla bla bla bla bla bla bla bla bla bla bla bla bla bla bla")
                    .AddLine("bla bla bla bla bla bla bla bla bla bla bla bla bla bla bla bla bla bla")
                    .AddLine("bla bla bla bla bla bla bla bla bla bla bla bla bla");

            sections.AddSection("beautiful-window")
                    .AddLine("bla bla bla bla bla bla bla bla bla bla bla bla bla bla bla bla bla bla")
                    .AddLine("bla bla bla bla bla bla bla bla bla bla bla bla bla bla bla bla bla bla")
                    .AddLine("bla bla bla bla bla bla bla bla bla bla bla bla bla");

            sections.AddSection("analyze")
                    .AddLine("bla bla bla bla bla bla bla bla bla bla bla bla bla bla bla bla bla bla")
                    .AddLine("bla bla bla bla bla bla bla bla bla bla bla bla bla bla bla bla bla bla")
                    .AddLine("bla bla bla bla bla bla bla bla bla bla bla bla bla");
        }

        private void AddLinesContent(SectionContent lines)
        {
            lines.AddLine("bla bla bla bla bla bla bla bla bla bla bla bla bla bla bla bla bla bla")
                 .AddLine("bla bla bla bla bla bla bla bla bla bla bla bla bla bla bla bla bla bla")
                 .AddLine("bla bla bla bla bla bla bla bla bla bla bla bla bla");
            lines.Print();
        }

        public void Sections()
        {
            Sections sections = new Sections();
            AddSectionContent(sections);
            sections.Print();

            Console.WriteLine("----------------------------");

            sections = new Sections(4);
            AddSectionContent(sections);
            sections.Print();
        }

        public void Lines()
        {
            SectionContent lines = new SectionContent();
            AddLinesContent(lines);
            lines.Print();

            Console.WriteLine("----------------------------");

            lines = new SectionContent(4);
            AddLinesContent(lines);
            lines.Print();
        }
    }
}
