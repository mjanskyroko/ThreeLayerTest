namespace TestWebApp.Infrastructure.Database.Mssql.Tests.Internal
{
    using TestWebApp.Infrastructure.Database.Mssql.Tests.Internal.Common;

    public interface ISqlScriptGenerator
    {
        string GenerateInsertScript(GenerationContext ctx);
        string GenerateDeleteScript(GenerationContext ctx);
    }
}
