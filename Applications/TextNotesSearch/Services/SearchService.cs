using McnLib.Parsers;
using System;
using System.Diagnostics;

namespace TextNotesSearch.Services
{
    public interface ISearchService
    {
        string SearchNotes(NoteFileParser parser, string searchString);
    }

    public class SearchService : ISearchService
    {

        IConsoleService _consoleService = new ConsoleService();
        Stopwatch sw = new Stopwatch();

        /// <returns>The updated keywords (input from its "open" mode) for another search.</returns>
        public string SearchNotes(NoteFileParser parser, string searchString)
        {
            //Console.WriteLine($"DEBUG|keywords: {searchString}");
            sw.Restart();

            string[] keywords_and_filter = searchString.Split('|');
            string[] keywords = keywords_and_filter[0].Trim().Split();
            string[] filter = null;
            if (keywords_and_filter.Length > 1)
                filter = keywords_and_filter[1].Trim().Split();

            var found = parser.NST.FindNotes(keywords, filter);

            int fileCount = 0;
            foreach (var note in found)
            {
                _consoleService.WriteLine($"{fileCount} : {note.FilePath}", ConsoleColor.Green);

                note.NotesFound.ForEach(n =>
                {
                    _consoleService.WriteLine($"{fileCount} : {n.Note.Id} {n.Note.Title}", ConsoleColor.DarkCyan);

                    foreach (var line in n.LinesFound)
                    {
                        Console.WriteLine($"{fileCount} {line.LineNumber} : {line.Text}");
                    }
                    Console.WriteLine();
                });

                fileCount++;
            }
            sw.Stop();
            Console.WriteLine("Time elapse: {0}\n", sw.Elapsed);

            while (found.Count > 0)
            {
                Console.Write("Open: ");
                string inputForOpen = Console.ReadLine();
                string[] select = inputForOpen.ToLower().Trim().Split();
                if (select.Length > 2)
                    return inputForOpen;
                else if (select.Length == 1) // select.Length is always >= 1
                {
                    if (select[0] == "q")
                        break;

                    if (!int.TryParse(select[0], out int selectFile))
                        return inputForOpen;
                    else
                    {
                        if (selectFile > found.Count - 1)
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
                else if (select.Length == 2)
                {
                    if (!int.TryParse(select[0], out int selectFile) || !int.TryParse(select[1], out int selectLine))
                        return inputForOpen;
                    else
                    {
                        if (selectFile > found.Count - 1)
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

            return null;
        }
    }
}
