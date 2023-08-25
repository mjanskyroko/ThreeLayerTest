﻿namespace TestWebApp.Application.Contracts.Database.Models
{
    using System;

    public class AccountFilter
    {
        public string? Name { get; set; }

        public Guid? OwnerId { get; set; }

        public decimal? Balance { get; set; }

        public int Offset { get; set; }

        public int Limit { get; set; }
    }
}
