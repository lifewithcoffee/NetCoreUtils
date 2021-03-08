using CoreCmd.Attributes;
using NetCoreUtils.FileOperation;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace TestApp.Cli.Commands
{
    class FileLineDict
    {
        Dictionary<string, List<string>> fileLineDicts = new Dictionary<string, List<string>>();

        public void AddLine(string fileFullPath, string line)
        {
            fileLineDicts[fileFullPath].Add(line);
        }
    }

    [Help("Search file")]
    [Alias("fs")]
    class FileSearchCommand
    {
        FileSearcher _svc = new FileSearcher();

        /// <param name="keywords">Multiple keywords separated by ',' or ';'</param>
        public void Name(string keywords)
        {
            var files = _svc.Find(@"i:\rp\mcn\sync\", "mcn", keywords.Split(',',';'));
            foreach(var file in files)
            {
                Console.WriteLine(file);
            }
        }

        public void Content(string keywords)
        {
            string baseDir = @"i:\rp\mcn\sync\";
            string extensionName = "mcn";
            string[] keywordArray = keywords.Split(',', ';');

            var filesWithAllKeywords = new DirectoryInfo(baseDir).EnumerateFiles($"*.{extensionName}", SearchOption.AllDirectories)
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
                                                       .SelectMany(f => File.ReadLines(f.FullName).Select(line => (f.FullName, line))
                                                       .Where(t => keywordArray.Any(k => t.line.ToLowerInvariant().Contains(k.ToLowerInvariant()))))
                                                       .GroupBy(t => t.FullName);

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
                    Console.WriteLine($"{count}|{line.line.Trim()}");
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
                    int select = Convert.ToInt32(readline);

                    if (select < 1 || select > foundFileNumber)
                    {
                        Console.WriteLine("Invalid input.");
                    }
                    else
                    {
                        Process.Start(new ProcessStartInfo(files[select - 1])
                        {
                            UseShellExecute = true  // UseShellExecute is false by default on .NET Core.
                        });
                    }
                }
                catch(Exception ex)
                {
                    Console.WriteLine("Invalid input.");
                }
                Console.Write("Please input the selection number to open a file (input 'q' to quit):");
                readline = Console.ReadLine();
            }
        }
    }
}
