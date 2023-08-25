namespace TestWebApp.Application.Accounts.Commands
{
    using FluentValidation;
    using MediatR;
    using System.Threading;
    using System.Threading.Tasks;
    using TestWebApp.Application.Contracts.Database;
    using TestWebApp.Domain;

    public class DeleteAccountByIdCommand : IRequest
    {
        public Guid Id { get; set; }
    }

    internal sealed class DeleteAccountByIdCommandValidator : AbstractValidator<DeleteAccountByIdCommand>
    {
        public DeleteAccountByIdCommandValidator()
        {
            RuleFor(a => a.Id).NotEmpty();
        }
    }

    internal sealed class DeleteAccountByIdCommandHandler : IRequestHandler<DeleteAccountByIdCommand>
    {
        private readonly IUnitOfWork unitOfWork;

        public DeleteAccountByIdCommandHandler(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public async Task Handle(DeleteAccountByIdCommand request, CancellationToken cancellationToken)
        {
            Account a = await unitOfWork.Accounts.GetByIdAsync(request.Id, cancellationToken);
            unitOfWork.Accounts.Delete(a);
            await unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}
