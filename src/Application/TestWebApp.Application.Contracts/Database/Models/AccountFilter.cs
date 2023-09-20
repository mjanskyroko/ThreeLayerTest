namespace TestWebApp.Application.Contracts.Database.Models
{
    using System;

    public class AccountFilter
    {
        public string? Name { get; set; }

        public Guid? OwnerId { get; set; }

        public decimal? BalanceMinimum { get; set; }

        public decimal? BalanceMaximum { get; set; }

        public bool? IsActive { get; set; }

        public int Offset { get; set; }

        public int Limit { get; set; }
    }
}
