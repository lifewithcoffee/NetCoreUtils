using NetCoreUtils.TestCli.JsonDemo;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace NetCoreUtils.TestCli.Commands
{
    class JsonCommand
    {
        JsonService _svc = new JsonService();

        //const string vivaldi_bookmark_path = @"C:\Users\Ron\appdata\local\Vivaldi\User Data\Default\Bookmarks";
        const string vivaldi_bookmark_path = @"C:\_temp\Bookmarks";

        public void Test1()
        {
            _svc.PrintJson(File.ReadAllBytes(vivaldi_bookmark_path).AsSpan());
        }

        public void Test2()
        {
            //_svc.ReadJson(File.ReadAllText(@"C:\_temp\vivaldi-bookmarks"));
            _svc.ReadJson(File.ReadAllText(vivaldi_bookmark_path));
        }
    }
}
