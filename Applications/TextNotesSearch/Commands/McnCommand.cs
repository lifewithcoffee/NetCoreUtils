using McnLib.Parsers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TextNotesSearch.Services;

namespace TextNotesSearch.Commands
{
    public class McnCommand
    {
        ISearchService _searchService = new SearchService();
        IConsoleService _consoleService = new ConsoleService();
        INoteFileParser _parser = new NoteFileParser();

        public void Notes()
        {
            Console.WriteLine("debug count 5");
            Console.WriteLine("Press 'r' to reload notes, 'q' to quit");

            _parser.ParseFolder(@"C:\__dell_sync_c\mcn\sync", "mcn");

            while (true)
            {
                var input = _consoleService.ReadLine("% ", ConsoleColor.Red);
                if (input == "r")
                {
                    Console.WriteLine("Reloading notes ...\n");
                    _parser.ParseFolder(@"C:\__dell_sync_c\mcn\sync", "mcn");
                    continue;
                }
                else if (input == "q")
                    break;
                else if (!string.IsNullOrEmpty(input))
                {
                    string moreInput = _searchService.SearchNotes(_parser, input);
                    while(moreInput != null)
                        moreInput = _searchService.SearchNotes(_parser, moreInput); 
                }
            }
        }
    }
}
