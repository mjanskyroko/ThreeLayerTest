using FluentValidation;
using MediatR;
using TestWebApp.Application.Contracts.Database;
using TestWebApp.Domain;

namespace TestWebApp.Application.Transactions.Commands
{
    public class CreateTransactionCommand : IRequest
    {
        public Guid Id { get; set; }

        public Guid From { get; set; }

        public Guid To { get; set; }

        public decimal Amount { get; set; }
    }

    internal sealed class CreateTransactionCommandValidator : AbstractValidator<CreateTransactionCommand>
    {
        public CreateTransactionCommandValidator()
        {
            RuleFor(t => t.From).NotEmpty();
            RuleFor(t => t.To).NotEmpty();
            RuleFor(t => t.Amount).GreaterThan(0m);
        }
    }

    internal sealed class CreateTransactionCommandHandler : IRequestHandler<CreateTransactionCommand>
    {
        private readonly IUnitOfWork unitOfWork;

        public CreateTransactionCommandHandler(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public async Task Handle(CreateTransactionCommand request, CancellationToken cancellationToken)
        {
            Transaction t = new Transaction();
            t.Id = Guid.NewGuid();
            t.From = await unitOfWork.Accounts.GetByIdAsync(request.From, cancellationToken);
            t.To = await unitOfWork.Accounts.GetByIdAsync(request.To, cancellationToken);
            t.Amount = request.Amount;

            unitOfWork.Transactions.Create(t);
            await unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}
