using System;

namespace TextNotesSearch.Services
{
    public interface IConsoleService
    {
        string ReadLine(string prompt, ConsoleColor color);
        void WriteLine(string msg, ConsoleColor color);
    }
    public class ConsoleService : IConsoleService
    {
        public string ReadLine(string prompt, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.Write(prompt);
            var input = Console.ReadLine().ToLower().Trim();
            Console.ResetColor();

            return input;
        }

        public void WriteLine(string msg, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.WriteLine(msg);
            Console.ResetColor();
        }
    }
}
