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
        public NoteStructureTree NST { get; set; } = new NoteStructureTree();

        public NoteFile ParseFile(string fullPath)
        {
            return new NoteLineParser().ParseLines(new NoteFile { FullPath = fullPath, Content = File.ReadAllLines(fullPath) });
        }

        // TODO: need to handle UnauthorizedAccessException & PathTooLongException?
        //       see https://docs.microsoft.com/en-us/dotnet/api/system.io.file.readlines?redirectedfrom=MSDN&view=net-6.0#overloads
        public void ParseFolder(string folderPath, string extensionName = "*")
        {
            NST.NoteFiles = new DirectoryInfo(folderPath)
                .EnumerateFiles($"*.{extensionName}", SearchOption.AllDirectories)
                .AsParallel()
                .Select(f => new NoteFile { FullPath = f.FullName, Name = f.Name , Content = File.ReadAllLines(f.FullName) })
                .ToList();

            // code in AsParallel().ForAll() must be thread safe, so need to new NoteLineParser()
            NST.NoteFiles.AsParallel().ForAll(f => f = new NoteLineParser().ParseLines(f));
        }
    }
}
