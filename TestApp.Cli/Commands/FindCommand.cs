using CoreCmd.Attributes;
using NetCoreUtils.FileOperation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace TestApp.Cli.Commands
{
    [Help("Search file")]
    class FileCommand
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

        public void Content(string k)
        {
            string baseDir = @"i:\rp\mcn\sync\";
            string extensionName = "mcn";
            string[] keywords = k.Split(',', ';');

            IEnumerable<string> matched = new DirectoryInfo(baseDir).EnumerateFiles($"*.{extensionName}", SearchOption.AllDirectories)
                                          .AsParallel()         // RL: it does increase the performance
                                          .SelectMany(f => File.ReadLines(f.FullName).Select(line => (f.FullName, line))
                                          .Where(t => keywords.All(k => t.line.ToLowerInvariant().Contains(k.ToLowerInvariant()))))
                                          .OrderBy(t=> t.FullName)
                                          .Select(t => $"{t.FullName}|{t.line.Trim()}");

            foreach(var match in matched)
            {
                Console.WriteLine(match);
            }
        }
    }
}
