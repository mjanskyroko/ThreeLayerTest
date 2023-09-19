namespace TestWebApp.Infrastructure.Database.Mssql.Tests.Internal
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public interface IFileManager
    {
        T Read<T>(string path);

        string Read(string path);
    }
}
