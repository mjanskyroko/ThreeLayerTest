namespace TestWebApp.Infrastructure.Database.Mssql.Tests.Internal
{
    public interface ISeedDatabaseManager
    {
        void Execute(string command);
        void Delete();
    }
}
