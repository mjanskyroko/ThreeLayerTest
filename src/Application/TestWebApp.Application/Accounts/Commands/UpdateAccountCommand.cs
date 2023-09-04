namespace TestWebApp.Application.Accounts.Commands
{
    using FluentValidation;
    using MediatR;
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using TestWebApp.Application.Contracts.Database;
    using TestWebApp.Domain;

    public class UpdateAccountCommand : IRequest
    {
        public Guid Id { get; set; }

        public string? Name { get; set; }

        public decimal? Balance { get; set; }

        public bool? IsActive { get; set; }
    }

    internal sealed class UpdateAccountCommandValidator : AbstractValidator<UpdateAccountCommand>
    {
        private readonly IUnitOfWork unitOfWork;

        public UpdateAccountCommandValidator(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
            RuleFor(a => a.Id).NotEmpty();
            When(a => a.Name is not null, () =>
                {
                    RuleFor(a => a.Name).MinimumLength(3);
                });
        }
    }

    internal sealed class UpdateAccountCommandHandler : IRequestHandler<UpdateAccountCommand>
    {
        private readonly IUnitOfWork unitOfWork;

        public UpdateAccountCommandHandler(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public async Task Handle(UpdateAccountCommand request, CancellationToken cancellationToken)
        {
            Account a = await unitOfWork.Accounts.GetByIdSafeAsync(request.Id, cancellationToken);

            if (request.Name is not null)
                a.Name = request.Name;
            if (request.Balance is not null)
                a.Balance = request.Balance.Value;
            if (request.IsActive is not null)
                a.IsActive = request.IsActive.Value;

            unitOfWork.Accounts.Update(a);
            await unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}
