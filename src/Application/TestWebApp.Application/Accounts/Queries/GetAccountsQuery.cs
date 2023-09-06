namespace TestWebApp.Application.Accounts.Queries
{
    using AutoMapper;
    using FluentValidation;
    using MediatR;
    using Microsoft.Extensions.Options;
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using TestWebApp.Application.Accounts.Common;
    using TestWebApp.Application.Contracts.Database;
    using TestWebApp.Application.Contracts.Database.Models;
    using TestWebApp.Domain;

    public record GetAccountsQuery(Guid? OwnerId, string? Name, bool? IsActive, int Offset, int Limit) : IRequest<List<AccountResponse>>;

    internal sealed class GetAccountsQueryValidator : AbstractValidator<GetAccountsQuery>
    {
        private readonly ValidationSettings opts;

        public GetAccountsQueryValidator(IOptions<ValidationSettings> options)
        {
            opts = options.Value;
            RuleFor(q => q.Offset).GreaterThanOrEqualTo(0);
            RuleFor(q => q.Limit).GreaterThan(0).LessThanOrEqualTo(opts.MaxLimit);
        }
    }

    internal sealed class GetAccountsQueryHandler : IRequestHandler<GetAccountsQuery, List<AccountResponse>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;

        public GetAccountsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }

        public async Task<List<AccountResponse>> Handle(GetAccountsQuery request, CancellationToken cancellationToken)
        {
            AccountFilter filter = new AccountFilter();
            filter.OwnerId = request.OwnerId;
            filter.Name = request.Name;
            filter.IsActive = request.IsActive;
            filter.Offset = request.Offset;
            filter.Limit = request.Limit;

            List<Account> result = await unitOfWork.Accounts.GetAsync(filter, cancellationToken);
            return mapper.Map<List<AccountResponse>>(result);
        }
    }
}
