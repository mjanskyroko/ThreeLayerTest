namespace TestWebApp.Domain
{
    using System;

    public class User
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = default!;

        public string PasswordHash { get; set; } = default!;

        public string Salt { get; set; } = default!;

        public DateTime CreatedAt { get; set; }
    }
}
