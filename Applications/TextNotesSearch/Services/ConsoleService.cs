using System;

namespace TextNotesSearch.Services
{
    public interface IConsoleService
    {
        void WriteLine(string msg, ConsoleColor color);
    }
    public class ConsoleService : IConsoleService
    {
        public void WriteLine(string msg, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.WriteLine(msg);
            Console.ResetColor();
        }
    }
}
