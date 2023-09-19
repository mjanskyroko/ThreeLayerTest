namespace TestWebApp.Infrastructure.Database.Mssql.Tests.Internal.Mssql
{
    using Newtonsoft.Json.Linq;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using TestWebApp.Infrastructure.Database.Mssql.Tests.Internal.Common;

    public class MssqlScriptGenerator : ISqlScriptGenerator
    {
        public string GenerateDeleteScript(GenerationContext ctx)
        {
            var sb = new StringBuilder();

            foreach (var key in ctx.Database.Keys)
            {
                sb.AppendLine($"DELETE FROM {key};");
            }

            return sb.ToString();
        }

        void GenerateColumnsSyntax(StringBuilder builder, string key, TableContext item)
        {
            builder.AppendLine($"INSERT INTO {key}");
            builder.AppendLine("(");

            var columnNames = string.Join(",\n", item.Columns.Select(s => $"\t\"{s}\""));

            builder.AppendLine(columnNames);

            builder.AppendLine(")");
            builder.AppendLine("VALUES");
        }
        void GenerateValuesSyntax(StringBuilder builder, List<IGrouping<string, Cell>> rows)
        {
            for (int i = 0; i < rows.Count; i++)
            {
                var value = string.Join(",\n\t", rows[i].Select(s => ParseCellValue(s)).ToList());

                builder.AppendLine("(");
                builder.AppendLine($"\t{value}");
                builder.AppendLine(")");

                if (i + 1 != rows.Count)
                {
                    builder.AppendLine(",");
                }
            }
        }

        object ParseCellValue(Cell cell)
        {
            var value = cell.Value;

            if (cell.Type == JTokenType.Float)
            {
                value = $"'{cell.Value}'".Replace(',', '.');
            }

            if (cell.Type == JTokenType.String || cell.Type == JTokenType.Date || cell.Type == JTokenType.Guid)
            {
                value = $"'{cell.Value}'";
            }

            return value;
        }


        public string GenerateInsertScript(GenerationContext ctx)
        {
            var sb = new StringBuilder();

            foreach (var key in ctx.Database.Keys)
            {
                var item = ctx.Database[key];
                GenerateColumnsSyntax(sb, key, item);

                var rows = item.Cells.GroupBy(b => b.ParentPath).ToList();
                GenerateValuesSyntax(sb, rows);
            }

            return sb.ToString();
        }
    }
}
