namespace TestWebApp.Infrastructure.Database.Mssql.Tests.Internal.Mssql
{
    using Microsoft.Data.SqlClient;
    using Microsoft.Extensions.Configuration.EnvironmentVariables;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    internal sealed class MssqlSeedDatabaseManager : ISeedDatabaseManager
    {
        private readonly IEnvironmentVariableManager environmentVariableManager = new MssqlEnvironmentVariableManager();

        public void Delete()
        {
            var connStr = environmentVariableManager.Get();
            List<string> toDelete = new List<string>();
            using (var conn = new SqlConnection(connStr))
            {
                conn.Open();

                var cmd = new SqlCommand("SELECT name from SYSOBJECTS WHERE xtype = 'U' and name <> '__EFMigrationsHistory';", conn);
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                        toDelete.Add(reader.GetString(0));
                    reader.Close();
                }


                for (int i = toDelete.Count - 1; i >= 0; i--)
                {
                    var delete = new SqlCommand($"DELETE FROM {toDelete[i]}", conn);
                    delete.ExecuteNonQuery();
                }

                conn.Close();
            }
        }

        public void Execute(string command)
        {
            var connStr = environmentVariableManager.Get();

            using (var conn = new SqlConnection(connStr))
            {
                conn.Open();
                var cmd = new SqlCommand(command, conn);
                cmd.ExecuteNonQuery();
                conn.Close();
            }
        }
    }
}
