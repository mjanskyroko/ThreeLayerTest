namespace TestWebApp.Infrastructure.Services.Tests
{
    using Microsoft.Extensions.Options;
    using System;
    using TestWebApp.Infrastructure.Services.Internal;
    using Xunit;
    using Xunit.Abstractions;

    public class PasswordServiceTests
    {
        private class Options : IOptions<ServicesSettings>
        {
            private readonly ServicesSettings settings;
            public Options() { settings = new ServicesSettings(); }
            public ServicesSettings Value => settings;
        }

        Options opt = new Options();
        ITestOutputHelper StdOut;

        public PasswordServiceTests(ITestOutputHelper output)
        {
            StdOut = output;
        }

        [Fact]
        public void SaltLengthTest()
        {
            ServicesSettings settings = opt.Value;
            PasswordService ps = new PasswordService(opt);

            byte[] salt = ps.GenerateSalt();
            Assert.Equal(settings.SaltLength, salt.Length);
        }

        [Fact]
        public void SaltRandomnessTest()
        {
            PasswordService ps = new PasswordService(opt);

            byte[] salt1 = ps.GenerateSalt();
            byte[] salt2 = ps.GenerateSalt();
            Assert.NotEqual(salt1, salt2);
        }

        [Fact]
        public void HashTest()
        {
            PasswordService ps = new PasswordService(opt);
            byte[] salt = new byte[8] { 91, 12, 74, 62, 97, 83, 94, 38 };
            string password = "neValjatiUredjaj??6";

            Assert.Equal("rg/6+Y2+7eLev6/XgiRzQCkbqVcpbzhbXDvmNGO106s=", ps.Hash(password, salt));
        }

        [Fact]
        public void VerificationTest()
        {
            PasswordService ps = new PasswordService(opt);
            byte[] salt = new byte[8] { 91, 12, 74, 62, 97, 83, 94, 38 };
            string guess = "neValjatiUredjaj??6";

            Assert.True(ps.Verify("rg/6+Y2+7eLev6/XgiRzQCkbqVcpbzhbXDvmNGO106s=", guess, Convert.ToBase64String(salt)));
        }
    }
}