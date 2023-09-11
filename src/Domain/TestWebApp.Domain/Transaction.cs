namespace TestWebApp.Domain
{
    using System;

    public class Transaction
    {
        public Guid Id { get; set; }

        public Account From { get; set; } = default!;

        public Guid FromId { get; set; }

        public Account To { get; set; } = default!;

        public Guid ToId { get; set; }

        public DateTime CreatedAt { get; set; }

        public decimal Amount { get; set; }
    }
}
