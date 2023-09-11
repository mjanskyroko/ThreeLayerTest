namespace TestWebApp.Application.Accounts.Common
{
    using System;
    using TestWebApp.Application.Users.Common;

    public record AccountResponse(Guid Id, Guid OwnerId, string Name, decimal Balance);
}
