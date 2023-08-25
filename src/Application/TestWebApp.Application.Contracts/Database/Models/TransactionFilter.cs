namespace TestWebApp.Application.Contracts.Database.Models
{
    using System;

    public class TransactionFilter
    {
        public Guid? AccountFrom { get; set; }

        public Guid? AccountTo { get; set; }

        public decimal? MinAmount { get; set; }

        public decimal? MaxAmount { get; set; }

        public int Offset { get; set; }

        public int Limit { get; set; }
    }
}
