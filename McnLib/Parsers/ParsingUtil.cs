using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace McnLib.Parsers
{
    public class ParsingUtil
    {
        public bool IsHeaderLine(string line)
        {
            string pattern1 = @"^-+ *$";
            string pattern2 = @"^=+ *$";

            if (Regex.IsMatch(line, pattern1) || Regex.IsMatch(line, pattern2))
                return true;
            else
                return false;
        }
    }
}
