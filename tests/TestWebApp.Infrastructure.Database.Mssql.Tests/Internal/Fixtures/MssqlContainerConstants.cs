namespace TestWebApp.Infrastructure.Database.Mssql.Tests.Internal.Fixtures
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public static class MssqlContainerConstants
    {
        public const string Image = "mcr.microsoft.com/mssql/server:2022-latest";
        public const string Username = "SA";
        public const string Password = "yourStrong(!)Password";
        public const string Database = "TestWebAppTests";
        public const int Port = 1433;
        public const bool UseContainer = false;
    }
}
