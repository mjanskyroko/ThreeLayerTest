namespace TestWebApp.Domain
{
    using System;

    public class Transaction
    {
        public Guid Id { get; set; }

        public Account From { get; set; } = default!;

        public Account To { get; set; } = default!;

        public decimal Amount { get; set; }
    }
}
