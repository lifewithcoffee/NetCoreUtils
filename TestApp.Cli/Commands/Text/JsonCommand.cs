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

        public void PrintAll(string jsonFilePath)
        {
            try
            {
                _svc.PrintJsonByTokenType(File.ReadAllBytes(jsonFilePath).AsSpan());
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.InnerException?.Message);
            }
        }

        public void PrintVivaldiBookmark(int maxLevel)
        {
            const string vivaldi_bookmark_path = @"C:\Users\Ron\appdata\local\Vivaldi\User Data\Default\Bookmarks";
            _svc.PrintVivaldiBookmarkJson(File.ReadAllText(vivaldi_bookmark_path), maxLevel);
        }
    }
}
