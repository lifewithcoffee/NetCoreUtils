using McnLib.Parsers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestApp.Cli.Commands
{
    public class McnCommand
    {
        public void Notes(string keywords)
        {
            Stopwatch sw = new Stopwatch();
            sw.Restart();

            Console.WriteLine("debug 5");
            var parser = new NoteFileParser();
            parser.ParseFolder(@"C:\__dell_sync_c\mcn\sync", "mcn");

            var found = parser.NST.FindNotes(keywords.Split(',',';'));

            foreach(var note in found)
            {
                Console.WriteLine(note.FilePath);

                note.NotesFound.ForEach(n => { 
                    Console.WriteLine("---" + n.Note.Title, n.Note.Id);
                    foreach(var line in n.LinesFound)
                    {
                        Console.WriteLine($"    {line.LineNumber}\t: {line.Text}");
                    }
                    Console.WriteLine();
                });
            }

            sw.Stop();
            Console.WriteLine("Time elapse: " + sw.Elapsed);
        }
    }
}
