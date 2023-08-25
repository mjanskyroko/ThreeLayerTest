namespace TestWebApp.Application.Accounts.Queries
{
    using AutoMapper;
    using FluentValidation;
    using MediatR;
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using TestWebApp.Application.Accounts.Common;
    using TestWebApp.Application.Contracts.Database;
    using TestWebApp.Domain;

    public record GetAccountByIdQuery(Guid Id) : IRequest<AccountResponse>;

    internal sealed class GetAccountByIdQueryValidator : AbstractValidator<GetAccountByIdQuery>
    {
        public GetAccountByIdQueryValidator()
        {
            RuleFor(a =>  a.Id).NotEmpty();
        }
    }

    internal sealed class GetAccountByIdQueryHandler : IRequestHandler<GetAccountByIdQuery, AccountResponse>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;

        public GetAccountByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }

        public async Task<AccountResponse> Handle(GetAccountByIdQuery request, CancellationToken cancellationToken)
        {
            Account a = await unitOfWork.Accounts.GetByIdAsync(request.Id, cancellationToken);
            return mapper.Map<AccountResponse>(a);
        }
    }
}
