namespace TestWebApp.Application.Contracts.Database.Models
{
    using System;

    public class TransactionFilter
    {
        public Guid? AccountFrom { get; set; }

        public Guid? AccountTo { get; set; }

        public DateTime? DateFrom { get; set; }

        public DateTime? DateTo { get; set; }

        public decimal? AmountMinimum { get; set; }

        public decimal? AmountMaximum { get; set; }

        public int Offset { get; set; }

        public int Limit { get; set; }
    }
}
