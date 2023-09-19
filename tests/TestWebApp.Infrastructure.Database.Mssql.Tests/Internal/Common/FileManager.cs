namespace TestWebApp.Infrastructure.Database.Mssql.Tests.Internal.Common
{
    using Newtonsoft.Json;
    using System;
    using System.IO;
    using System.Reflection;

    internal sealed class FileManager : IFileManager
    {
        public T Read<T>(string path)
        {
            string json = this.Read(path);
            var result = JsonConvert.DeserializeObject<T>(json);
            return result ?? throw new ApplicationException(nameof(result));
        }

        public string Read(string path)
        {
            var dir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            if (dir is null)
                throw new ApplicationException(nameof(dir));

            string json = null!;
            using (StreamReader streamReader = new StreamReader(path))
            {
                json = streamReader.ReadToEnd();
            }

            return json;
        }
    }
}
