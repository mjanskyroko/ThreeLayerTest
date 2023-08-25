using MediatR;

namespace TestWebApp.Application.Accounts.Commands
{
    public class CreateAccountCommand : IRequest
    {
        public Guid Id { get; set; }

        public Guid Owner { get; set; }

        public string Name { get; set; }

        public decimal Amount { get; set; }
    }
}
