namespace TestWebApp.Application.Contracts.Database.Models
{
    using System;

    public class UserFilter
    {
        public string? Name { get; set; }

        public DateTime? JoinDate { get; set; }

        public int Offset { get; set; }

        public int Limit { get; set; }
    }
}
