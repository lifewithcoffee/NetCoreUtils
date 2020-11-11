using TestApp.Cli.JsonDemo;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace TestApp.Cli.Commands.Text
{
    class JsonCommand
    {
        JsonService _svc = new JsonService();

        //const string vivaldi_bookmark_path = @"C:\Users\Ron\appdata\local\Vivaldi\User Data\Default\Bookmarks";
        const string vivaldi_bookmark_path = @"C:\_temp\Bookmarks";

        public void PrintAll()
        {
            _svc.PrintJsonByTokenType(File.ReadAllBytes(vivaldi_bookmark_path).AsSpan());
        }

        public void PrintVivaldiBookmark(int maxLevel)
        {
            _svc.PrintVivaldiBookmarkJson(File.ReadAllText(vivaldi_bookmark_path), maxLevel);
        }
    }
}
