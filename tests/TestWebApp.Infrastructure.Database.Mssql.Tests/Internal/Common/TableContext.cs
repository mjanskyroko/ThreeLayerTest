namespace TestWebApp.Infrastructure.Database.Mssql.Tests.Internal.Common
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public sealed class TableContext
    {
        public TableContext(string table, List<string> columns, List<Cell> cells)
        {
            Table = table;
            Columns = columns;
            Cells = cells;
        }

        public string Table { get; set; }
        public List<string> Columns { get; set; }
        public List<Cell> Cells { get; set; }
    }
}
