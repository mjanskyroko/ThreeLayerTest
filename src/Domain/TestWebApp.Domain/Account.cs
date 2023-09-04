namespace TestWebApp.Domain
{
    using System;

    public class Account
    {
        public Guid Id { get; set; }

        public User Owner { get; set; } = default!;

        public string Name { get; set; } = default!;

        public bool IsActive { get; set; }

        public decimal Balance { get; set; }
    }
}
