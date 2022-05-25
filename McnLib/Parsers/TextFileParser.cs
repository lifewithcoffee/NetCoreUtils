﻿using McnLib.States;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace McnLib.Parsers
{
    public class TextFileParser
    {
        FileLineReader fileReader = new FileLineReader();
        FileLineParser lineParser = new FileLineParser();
        public NoteStructureTree NST { get; set; } = new NoteStructureTree();

        private void ParseFile(string filePath)
        {
            NoteFile currentFile = new NoteFile { FullPath = filePath };
            NST.NoteFiles.Add(currentFile);

            var lines = fileReader.ReadFile(filePath);

            lineParser.ResetState(currentFile);
            lineParser.ParseLines(lines);
        }

        public void ParseFolder(string folderPath)
        {
            // TODO: parse folder
        }
    }
}
