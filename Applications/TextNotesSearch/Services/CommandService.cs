using McnLib.States;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextNotesSearch.Services
{
    public interface ICommandService
    {
        void ReadLine();
    }

    public class CommandService : ICommandService
    {
        string[] TargetWords { get; set; }
        string[] FilterWords { get; set; }

        string State { get; set; }
        string Prompt { get { return $"{this.State} % "; } }

        public void ReadLine()
        {
            Console.WriteLine(this.Prompt);
            string line = Console.ReadLine();
            string[] target_filter_strings = line.Split('|');

            this.TargetWords = target_filter_strings[0].Trim().Split();

            this.FilterWords = null;
            if (target_filter_strings.Length > 1)
                this.FilterWords = target_filter_strings[1].Trim().Split();
        }
    }

    public class SelectService
    {
        IConsoleService _consoleService = new ConsoleService();
        public void PrintFoundNotes(List<FindNotesResult> found)
        {
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
        }

        public void PrintFoundFiles() // for file name search
        {
            throw new NotImplementedException();
        }

        public void PrintFoundContent() // for file content search
        {
            throw new NotImplementedException();
        }
    }
}
