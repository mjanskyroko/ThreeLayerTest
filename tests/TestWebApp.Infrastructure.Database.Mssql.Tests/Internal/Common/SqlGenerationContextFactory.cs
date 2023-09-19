namespace TestWebApp.Infrastructure.Database.Mssql.Tests.Internal.Common
{
    using Newtonsoft.Json.Linq;
    using System.Collections.Generic;
    using System.Linq;

    public sealed class SqlGenerationContextFactory : ISqlGenerationContextFactory
    {
        public GenerationContext CreateContext(Dictionary<string, object> obj)
        {
            var tableCtxts = new List<TableContext>();
            var tableNames = obj.Keys.ToList();
            var dictionary = new Dictionary<string, TableContext>();

            foreach (var tableName in tableNames)
            {
                var table = obj[tableName];

                var arr = JArray.FromObject(table);
                var cells = arr.SelectMany(s => s.Children<JProperty>().Select(
                    s => new Cell(s.Path, s.Values().First().Type, s.Values().First(), s.Name, s.Parent?.Path ?? string.Empty))).ToList();

                var columns = cells.DistinctBy(s => s.Name).Select(s => s.Name).ToList();
                dictionary.Add(tableName, new TableContext(tableName, columns, cells));
            }
            return new GenerationContext(dictionary);
        }
    }
}
