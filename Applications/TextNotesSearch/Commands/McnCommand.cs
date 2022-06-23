using McnLib.Parsers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextNotesSearch.Commands
{
    public class McnCommand
    {
        ISearchService _searchService = new SearchService();

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
                {
                    string moreInput = _searchService.SearchNotes(parser, input);
                    while(moreInput != null)
                        moreInput = _searchService.SearchNotes(parser, moreInput); 
                }
            }
        }
    }
}
