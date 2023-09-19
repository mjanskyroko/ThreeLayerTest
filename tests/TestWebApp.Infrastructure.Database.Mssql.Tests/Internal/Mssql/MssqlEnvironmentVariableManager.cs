namespace TestWebApp.Infrastructure.Database.Mssql.Tests.Internal.Mssql
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    internal sealed class MssqlEnvironmentVariableManager : IEnvironmentVariableManager
    {
        private const string TestConnString = "MSSQL_CONN_STRING";

        public string Get()
        {
            return Environment.GetEnvironmentVariable(TestConnString) ?? throw new ArgumentNullException(nameof(TestConnString));
        }

        public void Set(string value)
        {
            Environment.SetEnvironmentVariable(TestConnString, value);
        }
    }
}
