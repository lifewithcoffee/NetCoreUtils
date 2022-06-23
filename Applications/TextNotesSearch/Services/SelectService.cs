using McnLib.States;
using System;
using System.Collections.Generic;

namespace TextNotesSearch.Services
{
    public interface ISelectService
    {
        void PrintFoundNotes(List<FindNotesResult> found);
    }

    public class SelectService : ISelectService
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
