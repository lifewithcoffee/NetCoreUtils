using McnLib.Parsers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestApp.Cli.Commands
{
    public class McnCommand
    {
        public void Notes(string keywords)
        {
            var parser = new NoteFileParser();
            parser.ParseFolder(@"C:\__dell_sync_c\mcn\sync", "mcn");

            var found = parser.NST.FindNotes(keywords.Split(',',';'));

            foreach(var note in found)
            {
                Console.WriteLine(note.FilePath);
                foreach(var line in note.LinesFound)
                {
                    Console.WriteLine($"    {line.LineNumber}\t: {line.Text}");
                }
                Console.WriteLine();
            }
        }
    }
}
