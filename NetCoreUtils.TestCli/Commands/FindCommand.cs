using NetCoreUtils.FileOperation;
using System;
using System.Collections.Generic;
using System.Text;

namespace NetCoreUtils.TestCli.Commands
{
    class FindCommand
    {
        FileSearcher _svc = new FileSearcher();

        /// <param name="keywords">Multiple keywords separated by ',' or ';'</param>
        public void File(string keywords)
        {
            var files = _svc.Find(@"e:\rp\hg\mcn\sync\", "mcn", keywords.Split(',',';'));
            foreach(var file in files)
            {
                Console.WriteLine(file);
            }
        }
    }
}
