using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextNotesSearch.Services
{
    public interface ICommandService
    {
        void ReadLine();
    }

    public class CommandService : ICommandService
    {
        string[] TargetWords { get; set; }
        string[] FilterWords { get; set; }

        string State { get; set; }
        string Prompt { get { return $"{this.State} % "; } }

        public void ReadLine()
        {
            Console.WriteLine(this.Prompt);
            string line = Console.ReadLine();
            string[] target_filter_strings = line.Split('|');

            this.TargetWords = target_filter_strings[0].Trim().Split();

            this.FilterWords = null;
            if (target_filter_strings.Length > 1)
                this.FilterWords = target_filter_strings[1].Trim().Split();
        }
    }
}
