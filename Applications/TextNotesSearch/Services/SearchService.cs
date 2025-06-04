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
        ISelectService _selectService = new SelectService();
        Stopwatch sw = new Stopwatch();

        /// <returns>The updated keywords (input from its "open" mode) for another search.</returns>
        public string SearchNotes(NoteFileParser parser, string searchString)
        {
            //Console.WriteLine($"DEBUG|keywords: {searchString}");
            sw.Restart();

            // _working_ refactoring in CommandService.cs
            string[] keywords_and_filter = searchString.Split('|');
            string[] keywords = keywords_and_filter[0].Trim().Split();
            string[] filter = null;
            if (keywords_and_filter.Length > 1)
                filter = keywords_and_filter[1].Trim().Split();

            var found = parser.NST.FindNotes(keywords, filter);
            _selectService.PrintFoundNotes(found);

            sw.Stop();
            Console.WriteLine("Time elapse: {0}\n", sw.Elapsed);

            const string vimDirectory = @"D:\apps_dell\Vim\vim91";
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
                            FileName = @$"{vimDirectory}\gvim.exe",
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
                            FileName = @$"{vimDirectory}\gvim.exe",
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
