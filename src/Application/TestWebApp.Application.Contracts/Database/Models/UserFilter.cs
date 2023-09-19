namespace TestWebApp.Application.Contracts.Database.Models
{
    using System;

    public class UserFilter
    {
        public string? Name { get; set; }

        public DateTime? JoinDateFrom { get; set; }

        public DateTime? JoinDateTo { get; set; }

        public int Offset { get; set; }

        public int Limit { get; set; }
    }
}
