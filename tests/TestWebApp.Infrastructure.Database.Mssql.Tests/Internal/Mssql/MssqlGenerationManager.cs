namespace TestWebApp.Infrastructure.Database.Mssql.Tests.Internal.Mssql
{
    using System.Collections.Generic;
    using TestWebApp.Infrastructure.Database.Mssql.Tests.Internal.Common;

    internal sealed class MssqlGenerationManager : ISqlGenerationManager
    {
        private readonly ISqlGenerationContextFactory contextFactory = new SqlGenerationContextFactory();
        private readonly ISqlScriptGenerator sqlScriptGenerator = new MssqlScriptGenerator();

        public GenerationResult Generate(Dictionary<string, object> obj)
        {
            var ctxt = contextFactory.CreateContext(obj);
            var insert = sqlScriptGenerator.GenerateInsertScript(ctxt);
            var delete = sqlScriptGenerator.GenerateDeleteScript(ctxt);
            return new GenerationResult(insert, delete);
        }
    }
}
