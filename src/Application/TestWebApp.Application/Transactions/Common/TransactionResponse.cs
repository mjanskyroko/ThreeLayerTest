﻿namespace TestWebApp.Application.Transactions.Common
{
    public record TransactionResponse(Guid Id, Guid From, Guid To, DateTime CreatedAt, decimal Amount);
}
