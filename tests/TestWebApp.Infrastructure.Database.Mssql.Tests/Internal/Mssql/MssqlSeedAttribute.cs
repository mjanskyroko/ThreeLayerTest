namespace TestWebApp.Infrastructure.Database.Mssql.Tests.Internal.Mssql
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using TestWebApp.Infrastructure.Database.Mssql.Tests.Internal.Common;
    using Xunit.Sdk;

    public sealed class MssqlSeedAttribute : BeforeAfterTestAttribute
    {
        private GenerationResult generationItem = default!;
        private readonly ISeedDatabaseManager dbConnectionUtility = new MssqlSeedDatabaseManager();
        private readonly ISqlGenerationManager sqlGenerationManager = new MssqlGenerationManager();
        private readonly IFileManager fileManager = new FileManager();

        public string FilePath { get; }

        public MssqlSeedAttribute(string filePath)
        {
            this.FilePath = filePath;
        }

        public override void After(MethodInfo methodUnderTest)
        {
            dbConnectionUtility.Delete();

            base.After(methodUnderTest);
        }

        public override void Before(MethodInfo methodUnderTest)
        {
            var obj = fileManager.Read<Dictionary<string, object>>(this.FilePath);
            generationItem = sqlGenerationManager.Generate(obj);
            dbConnectionUtility.Execute(generationItem.Insert);

            base.Before(methodUnderTest);
        }
    }
}
