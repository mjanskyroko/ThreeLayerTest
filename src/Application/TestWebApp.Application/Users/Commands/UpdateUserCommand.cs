﻿namespace TestWebApp.Application.Users.Commands
{
    using FluentValidation;
    using MediatR;
    using Microsoft.Extensions.Options;
    using TestWebApp.Application.Contracts.Database;
    using TestWebApp.Application.Contracts.Services;
    using TestWebApp.Application.Internal;
    using TestWebApp.Domain;

    public class UpdateUserCommand : IRequest
    {
        public Guid Id { get; set; }

        public string Password { get; set; } = default!;
    }

    internal class UpdateUserCommandValidator : AbstractValidator<UpdateUserCommand>
    {
        private readonly ValidationSettings opts;

        public UpdateUserCommandValidator(IOptions<ValidationSettings> options)
        {
            opts = options.Value;
            RuleFor(u => u.Id).NotEmpty();
            When(u => !string.IsNullOrEmpty(u.Password), () =>
                {
                    RuleFor(u => u.Password).MinimumLength(opts.MinimumPasswordLength);
                });
        }
    }

    internal class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IPasswordService passwordService;

        public UpdateUserCommandHandler(IUnitOfWork unitOfWork, IPasswordService passwordService)
        {
            this.unitOfWork = unitOfWork;
            this.passwordService = passwordService;
        }

        public async Task Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            User u = await unitOfWork.Users.GetByIdSafeAsync(request.Id, cancellationToken);

            if (request.Password is not null && request.Password.Length > 0)
            {
                byte[] salt = passwordService.GenerateSalt();
                u.Salt = Convert.ToBase64String(salt);
                u.PasswordHash = passwordService.Hash(request.Password, salt);
            }

            unitOfWork.Users.Update(u);
            await unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}
