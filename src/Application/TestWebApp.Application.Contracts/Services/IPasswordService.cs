namespace TestWebApp.Application.Contracts.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public interface IPasswordService
    {
        string Hash(string password, byte[] salt);

        public byte[] GenerateSalt();

        bool Verify(string password, string guess, string salt);
    }
}
