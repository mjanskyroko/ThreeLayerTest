using FluentValidation;
using MediatR;
using TestWebApp.Application.Contracts.Database;
using TestWebApp.Domain;

namespace TestWebApp.Application.Users.Commands
{
    public class DeleteUserByIdCommand : IRequest
    {
        public Guid Id { get; set; }
    }

    internal class DeleteUserByIdCommandValidator : AbstractValidator<DeleteUserByIdCommand>
    {
        public DeleteUserByIdCommandValidator()
        {
            RuleFor(u => u.Id).NotEmpty();
        }
    }

    internal class DeleteUserByIdCommandHandler : IRequestHandler<DeleteUserByIdCommand>
    {
        private readonly IUnitOfWork unitOfWork;

        public DeleteUserByIdCommandHandler(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public async Task Handle(DeleteUserByIdCommand request, CancellationToken cancellationToken)
        {
            User u = await unitOfWork.Users.GetByIdAsync(request.Id, cancellationToken);
            unitOfWork.Users.Delete(u);
            await unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}
