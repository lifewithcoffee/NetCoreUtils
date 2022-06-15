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
        Stopwatch sw = new Stopwatch();
        public void Notes()
        {
            Console.WriteLine("debug count 5");
            Console.WriteLine("Press 'r' to reload notes, 'q' to quit");

            NoteFileParser parser = new NoteFileParser();
            parser.ParseFolder(@"C:\__dell_sync_c\mcn\sync", "mcn");

            while (true)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write("% ");
                var input = Console.ReadLine().ToLower().Trim();
                Console.ResetColor();

                if (input == "r")
                {
                    Console.WriteLine("Reloading notes ...\n");
                    parser = new NoteFileParser();
                    parser.ParseFolder(@"C:\__dell_sync_c\mcn\sync", "mcn");
                    continue;
                }
                else if (input == "q")
                    break;
                else if (!string.IsNullOrEmpty(input))
                    Search(parser, input);
            }
        }

        private void Search(NoteFileParser parser,string keywords)
        {
            sw.Restart();
            var found = parser.NST.FindNotes(keywords.Split());

            int fileCount = 0;
            foreach(var note in found)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"{fileCount} : {note.FilePath}");
                Console.ResetColor();

                note.NotesFound.ForEach(n => {
                    Console.ForegroundColor = ConsoleColor.DarkCyan;
                    Console.WriteLine($"{fileCount} : {n.Note.Id} {n.Note.Title}");
                    Console.ResetColor();
                    foreach(var line in n.LinesFound)
                    {
                        Console.WriteLine($"{fileCount} {line.LineNumber} : {line.Text}");
                    }
                    Console.WriteLine();
                });

                fileCount++;
            }
            sw.Stop();
            Console.WriteLine("Time elapse: {0}\n", sw.Elapsed);

            while(found.Count > 0)
            {
                Console.Write("Open: ");
                string[] select = Console.ReadLine().ToLower().Trim().Split();
                if (select.Length > 2)
                    continue;
                else if (select.Length == 1) // select.Length is always >= 1
                {
                    if (select[0] == "q")
                        break;

                    if (!Int32.TryParse(select[0], out int selectFile))
                        continue;
                    else
                    {
                        if(selectFile > found.Count - 1)
                            continue;

                        int selectLine = found[selectFile].NotesFound[0].LinesFound[0].LineNumber;

                        var startInfo = new ProcessStartInfo
                        {
                            FileName = @"D:\apps_dell\Vim\vim82\gvim.exe",
                            Arguments = $"--remote-tab-silent +{selectLine} \"{found[selectFile].FilePath}\"",
                            UseShellExecute = true,  // UseShellExecute is false by default on .NET Core.
                        };
                        Process.Start(startInfo);
                    }
                }
                else if(select.Length == 2)
                { 
                    if (!Int32.TryParse(select[0], out int selectFile) || !Int32.TryParse(select[1], out int selectLine))
                        continue;
                    else
                    {
                        if(selectFile > found.Count - 1)
                            continue;

                        var startInfo = new ProcessStartInfo
                        {
                            FileName = @"D:\apps_dell\Vim\vim82\gvim.exe",
                            Arguments = $"--remote-tab-silent +{selectLine} \"{found[selectFile].FilePath}\"",
                            UseShellExecute = true,  // UseShellExecute is false by default on .NET Core.
                        };

                        Process.Start(startInfo);

                    }
                }
            }
            Console.WriteLine("");
        }
    }
}
