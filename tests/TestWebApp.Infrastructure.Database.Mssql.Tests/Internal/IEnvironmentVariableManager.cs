namespace TestWebApp.Infrastructure.Database.Mssql.Tests.Internal
{
    public interface IEnvironmentVariableManager
    {
        string Get();

        void Set(string variable);
    }
}
