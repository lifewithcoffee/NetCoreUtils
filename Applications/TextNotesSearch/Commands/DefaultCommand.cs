using CoreCmd.Attributes;
using NetCoreUtils.FileOperation;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using TextNotesSearch.Core;
using TextNotesSearch.Configuration;

namespace TextNotesSearch.Commands
{
    public class DefaultCommand
    {
        Configs _configs = new Configs();
        string BaseDir { get { return _configs.DefaultFolder.FolderPath; } }
        string ExtensionName { get { return _configs.DefaultFolder.FileExtentions; } }

        FileSearcher _svc = new FileSearcher();

        [Help("Alias of 'name' command")]
        public void N(string keywords)
        {
            Name(keywords);
        }

        /// <param name="keywords">Multiple keywords separated by ',' or ';'</param>
        [Help("Search by name")]
        public void Name(string keywords)
        {
            var files = _svc.Find(BaseDir, ExtensionName, keywords.Split(',',';'));
            foreach(var file in files)
            {
                Console.WriteLine(file);
            }
        }

        [Help("Alias of 'content' command")]
        public void C(string keywords)
        {
            Content(keywords);
        }

        [Help("Search by content")]
        public void Content(string keywords)
        {
            string[] keywordArray = keywords.Split(',', ';');

            var filesWithAllKeywords = new DirectoryInfo(BaseDir).EnumerateFiles($"*.{ExtensionName}", SearchOption.AllDirectories)
                                                                 .AsParallel()     // RL: it does increase the performance
                                                                 .Select(f => (f.FullName, File.ReadAllText(f.FullName)))
                                                                 .Where(t => keywordArray.All(k => t.Item2.ToLowerInvariant().Contains(k.ToLowerInvariant())));

            Dictionary<string, List<string>> fileLineDicts = new Dictionary<string, List<string>>();

            // init dict: generate list instance for all keys
            foreach (var filePath in filesWithAllKeywords)
            {
                fileLineDicts[filePath.FullName] = new List<string>();
            }

            //IEnumerable<string> linesWithAnyKeywords = filesWithAllKeywords
            var linesWithAnyKeywords = filesWithAllKeywords
                .SelectMany(f =>
                 {
                     int lineCount = 1;
                     return File.ReadLines(f.FullName)
                                .Select(line => new LineInfo{ FileFullName = f.FullName, LineNumber = lineCount++, LineText = line });
                 })
                .Where(t => keywordArray.Any(k => t.LineText.ToLowerInvariant().Contains(k.ToLowerInvariant())))
                .GroupBy(t => t.FileFullName);

            int count = 1;
            int foundFileNumber = linesWithAnyKeywords.Count();
            string[] files = new string[foundFileNumber];

            foreach (var lines in linesWithAnyKeywords)
            {
                string fileFullName = lines.Key.Trim();
                files[count - 1] = fileFullName;

                Console.WriteLine($"{count}|{fileFullName}");
                Console.WriteLine($"{count}|{"".PadLeft(fileFullName.Length, '-')}");

                var lineEnumerator = lines.ToList();
                foreach (var line in lineEnumerator)
                {
                    Console.WriteLine($"{count}|({line.LineNumber}) {line.LineText.Trim()}");
                }
                count++;
                Console.WriteLine();
            }

            Console.Write("Please input the selection number to open a file (input 'q' to quit):");
            string readline = Console.ReadLine();

            while (!readline.Equals("q"))
            {
                try
                {
                    string[] splittedReadLine = readline.Split("/");
                    if(splittedReadLine.Length != 2)
                    {
                        throw new Exception("Invalid selection");
                    }

                    int selectFile = Convert.ToInt32(splittedReadLine[0]);
                    int lineNumber = Convert.ToInt32(splittedReadLine[1]);

                    if (selectFile < 1 || selectFile > foundFileNumber)
                    {
                        Console.WriteLine("Invalid input.");
                    }
                    else
                    {
                        // use gvim to open the file and jump the to the corresponding line
                        var startInfo = new ProcessStartInfo
                        {
                            FileName = @"D:\apps_dell\Vim\vim82\gvim.exe",
                            Arguments = $"--remote-tab-silent +{lineNumber} \"{files[selectFile - 1]}\"",
                            UseShellExecute = true,  // UseShellExecute is false by default on .NET Core.
                        };

                        Process.Start(startInfo);
                    }
                }
                catch(Exception ex)
                {
                    Console.WriteLine($"Invalid input: {ex.Message}");
                }
                Console.Write("Please input the selection number to open a file (input 'q' to quit):");
                readline = Console.ReadLine();
            }
        }
    }
}
