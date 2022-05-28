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

        /// <summary>
        /// A "header line" is lines of either "-----" or "====="
        /// </summary>
        /// 
        /// <param name="line0">The 1st line being processed, usually the
        /// "current" line, but may be null if the real current line is the
        /// first line of a file.</param>
        /// 
        /// <param name="line1">The 2nd line being processed, if line0 is the
        /// real current line, then it's the next 1st line</param>
        /// 
        /// <param name="line2">The 3nd line being processed, if line0 is the
        /// real current line, then it's the next 2st line</param>
        /// 
        /// <param name="line3">The 4nd line being processed, if line0 is the
        /// real current line, then it's the next 3st line</param>
        /// <returns></returns>
        public bool IsSectionHeader(string? line0, string? line1, string? line2, string? line3)
        {
            // the current line must be a blank line
            if (string.IsNullOrWhiteSpace(line0))
            {
                // the next 2nd (possible header title) and 3rd must not be blank lines
                if (string.IsNullOrWhiteSpace(line2) || string.IsNullOrWhiteSpace(line3))
                    return false;

                // the next 3rd line must be a header line
                if (!IsHeaderLine(line3))
                    return false;

                // if the next 1st line is a blank line
                if (string.IsNullOrWhiteSpace(line1))
                    return true;

                // if the next 1st line is a header line
                if (IsHeaderLine(line1) && line3 == line1)
                    return true;
            }

            return false;
        }
    }
}
