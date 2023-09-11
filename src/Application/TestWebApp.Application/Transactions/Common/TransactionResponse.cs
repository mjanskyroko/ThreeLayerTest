namespace TestWebApp.Application.Transactions.Common
{
    public record TransactionResponse(Guid Id, Guid FromId, Guid ToId, DateTime CreatedAt, decimal Amount);
}
