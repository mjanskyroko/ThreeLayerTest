namespace TestWebApp.Infrastructure.Database.Mssql.Tests.Internal.Common
{
    using Newtonsoft.Json.Linq;

    public sealed record Cell(string Path, JTokenType Type, object Value, string Name, string ParentPath);
}
