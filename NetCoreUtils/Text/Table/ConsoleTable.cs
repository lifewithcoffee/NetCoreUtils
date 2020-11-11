using System.Collections.Generic;
using System.Linq;

namespace NetCoreUtils.Text.Table
{
    public class ConsoleTable
    {
        ConsoleTableRow header = new ConsoleTableRow();
        List<bool> leftAlignSetting = new List<bool>();
        List<ConsoleTableRow> rows = new List<ConsoleTableRow>();

        public ConsoleTable AddHeaderCell(string headerCell, bool leftAlign = true)
        {
            header.AddCell(headerCell);
            leftAlignSetting.Add(leftAlign);
            return this;
        }

        public void AddRow(ConsoleTableRow row)
        {
            rows.Add(row);
        }

        private int[] CalcMaxColumnWitdhSetting()
        {
            int columnNumber = rows.Select(r => r.CellNumber).Max();
            int[] result = new int[columnNumber];

            foreach (var row in rows)
            {
                int[] cellWidth = row.CalcCellWidth();

                for (int idx = 0; idx < cellWidth.Length; idx++)
                {
                    if (cellWidth[idx] > result[idx])
                        result[idx] = cellWidth[idx];
                }
            }

            return result;
        }

        private void PrintDivider(int[] widthSetting)
        {
            ConsoleTableRow dividerRow = new ConsoleTableRow();

            foreach (int width in widthSetting)
            {
                dividerRow.AddCell("".PadLeft(width, '-'));
            }

            dividerRow.Print(widthSetting);
        }

        public void Print()
        {
            int[] widthSetting = CalcMaxColumnWitdhSetting();
            bool[] alignSetting = leftAlignSetting.ToArray();
            header.Print(widthSetting, alignSetting);
            PrintDivider(widthSetting);
            foreach (var row in rows)
                row.Print(widthSetting, alignSetting);
        }
    }
}
