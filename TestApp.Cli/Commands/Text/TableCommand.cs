using NetCoreUtils.Text.Table;
using System;
using System.Collections.Generic;
using System.Text;

namespace TestApp.Cli.Commands.Text
{
    class TableCommand
    {
        public void Print()
        {
            ConsoleTable table = new ConsoleTable();
            table.AddHeaderCell("test1", false)
                 .AddHeaderCell("test333333")
                 .AddHeaderCell("test4444444444")
                 .AddHeaderCell("test222")
                 ;

            table.AddRow(new ConsoleTableRow()
                             .AddCell("1113333 33333")
                             .AddCell("1113333 4444 43343")
                             .AddCell("11111")
                             .AddCell("111")
                             );

            table.AddRow(new ConsoleTableRow()
                             .AddCell("1113333 4444 43343")
                             .AddCell("1113333 33333")
                             .AddCell("111")
                             .AddCell("11111")
                             );

            table.AddRow(new ConsoleTableRow()
                             .AddCell("11111")
                             .AddCell("1113333 4444 43343")
                             .AddCell("1113333 33333")
                             .AddCell("111")
                             );

            table.AddRow(new ConsoleTableRow()
                             .AddCell("111")
                             .AddCell("11111")
                             .AddCell("1113333 4444 43343")
                             .AddCell("1113333 33333")
                             );

            table.Print();
        }
    }
}
