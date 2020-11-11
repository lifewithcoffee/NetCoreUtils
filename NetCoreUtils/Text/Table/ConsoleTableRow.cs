using System;
using System.Collections.Generic;
using System.Text;

namespace NetCoreUtils.Text.Table
{
    public class ConsoleTableRow
    {
        List<string> row = new List<string>();

        public ConsoleTableRow AddCell(string cell)
        {
            row.Add(cell);
            return this;
        }

        public int CellNumber { get { return row.Count; } }

        public int[] CalcCellWidth()
        {
            int[] result = new int[row.Count];

            int index = 0;
            foreach (var cell in row)
            {
                result[index++] = cell.Length;
            }

            return result;
        }

        public void Print(int[] width, bool[] leftAlignSettings = null)
        {
            StringBuilder sb = new StringBuilder();
            int count = 0;
            foreach (var cell in row)
            {
                string paddedCell;
                if (leftAlignSettings != null)
                {
                    if (leftAlignSettings[count])
                        paddedCell = cell.PadRight(width[count++]);
                    else
                        paddedCell = cell.PadLeft(width[count++]);
                }
                else
                    paddedCell = cell.PadRight(width[count++]);

                sb.Append(paddedCell);

                if (count < row.Count)
                    sb.Append(" | ");
            }

            Console.WriteLine(sb.ToString());
        }
    }
}
