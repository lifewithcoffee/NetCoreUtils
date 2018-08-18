using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace NetCoreUtils.FileOperation
{
    public class FileSearcher
    {
        public IEnumerable<string> Find(string baseDir, string extensionName, params string[] keywords)
        {
            var matched = new DirectoryInfo(baseDir).EnumerateFiles($"*.{extensionName}", SearchOption.AllDirectories)
                       .AsParallel()
                       .Where(f => keywords.All(k => f.Name.ToLowerInvariant().Contains(k.ToLowerInvariant())));

            return matched.Select(f => f.FullName);
        }

        public string CreateIndex(string path)
        {
            throw new NotImplementedException();
        }
    }
}
