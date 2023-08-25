namespace TestWebApp.Application.Accounts.Common
{
    using System;

    public record AccountResponse(Guid Id, Guid Owner, string Name, decimal Balance);
}
