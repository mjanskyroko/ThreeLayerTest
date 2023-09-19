namespace TestWebApp.Infrastructure.Database.Mssql.Tests.Internal.Common
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public sealed class GenerationContext
    {
        public GenerationContext(Dictionary<string, TableContext> db)
        {
            Database = db;
        }

        public Dictionary<string, TableContext> Database { get; set; }
    }
}
