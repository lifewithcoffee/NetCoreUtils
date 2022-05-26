using McnLib.States;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace McnLib.Parsers
{
    public class NoteFileParser
    {
        NoteLineParser lineParser = new NoteLineParser();
        public NoteStructureTree NST { get; set; } = new NoteStructureTree();

        public NoteFile ParseFile(string fullPath)
        {
            return lineParser.ParseLines(new NoteFile { FullPath = fullPath, Content = File.ReadAllLines(fullPath) });
        }

        // TODO: need to handle UnauthorizedAccessException & PathTooLongException?
        //       see https://docs.microsoft.com/en-us/dotnet/api/system.io.file.readlines?redirectedfrom=MSDN&view=net-6.0#overloads
        public void ParseFolder(string folderPath, string extensionName = "*")
        {
            NST.NoteFiles = new DirectoryInfo(folderPath)
                .EnumerateFiles($"*.{extensionName}", SearchOption.AllDirectories)
                .AsParallel()
                .Select(f => new NoteFile { FullPath = f.FullName, Content = File.ReadAllLines(f.FullName) })
                .ToList();
                
            NST.NoteFiles.ForEach(f => f = lineParser.ParseLines(f));
        }
    }
}
