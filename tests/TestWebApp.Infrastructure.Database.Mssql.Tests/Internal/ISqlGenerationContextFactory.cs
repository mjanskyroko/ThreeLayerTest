namespace TestWebApp.Infrastructure.Database.Mssql.Tests.Internal
{
    using System.Collections.Generic;
    using TestWebApp.Infrastructure.Database.Mssql.Tests.Internal.Common;

    public interface ISqlGenerationContextFactory
    {
        GenerationContext CreateContext(Dictionary<string, object> obj);
    }
}
