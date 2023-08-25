namespace TestWebApp.Application.Accounts.Queries
{
    using AutoMapper;
    using FluentValidation;
    using MediatR;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using TestWebApp.Application.Accounts.Common;
    using TestWebApp.Application.Contracts.Database;
    using TestWebApp.Application.Contracts.Database.Models;
    using TestWebApp.Domain;

    public record GetAccountsFromUserQuery(Guid UserId, int Offset, int Limit) : IRequest<List<AccountResponse>>;

    internal sealed class GetAccountsFromUserQueryValidator : AbstractValidator<GetAccountsFromUserQuery>
    {
        public GetAccountsFromUserQueryValidator()
        {
            RuleFor(a => a.UserId).NotEmpty();
            RuleFor(a => a.Offset).GreaterThanOrEqualTo(0);
            RuleFor(a => a.Limit).GreaterThan(0).LessThanOrEqualTo(100);
        }
    }

    internal sealed class GetAccountsFromUserQueryHandler : IRequestHandler<GetAccountsFromUserQuery, List<AccountResponse>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;

        public GetAccountsFromUserQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }

        public async Task<List<AccountResponse>> Handle(GetAccountsFromUserQuery request, CancellationToken cancellationToken)
        {
            AccountFilter filter = new AccountFilter();
            filter.OwnerId = request.UserId;
            filter.Offset = request.Offset;
            filter.Limit = request.Limit;

            List<Account> accounts = await unitOfWork.Accounts.GetAsync(filter, cancellationToken);
            return mapper.Map<List<AccountResponse>>(accounts);
        }
    }
}
