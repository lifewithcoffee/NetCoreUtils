using NetCoreUtils.FileOperation;
using System;
using System.Collections.Generic;
using System.Text;

namespace TestApp.Cli.Commands
{
    class FindCommand
    {
        FileSearcher _svc = new FileSearcher();

        /// <param name="keywords">Multiple keywords separated by ',' or ';'</param>
        public void File(string keywords)
        {
            var files = _svc.Find(@"e:\rp\mcn\sync\", "mcn", keywords.Split(',',';'));
            foreach(var file in files)
            {
                Console.WriteLine(file);
            }
        }
    }
}
