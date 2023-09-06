namespace TestWebApp.Application.Transactions.Queries
{
    using AutoMapper;
    using FluentValidation;
    using MediatR;
    using Microsoft.Extensions.Options;
    using System.Threading;
    using System.Threading.Tasks;
    using TestWebApp.Application.Contracts.Database;
    using TestWebApp.Application.Transactions.Common;
    using TestWebApp.Domain;

    public record GetTransactionsWithAccountQuery(Guid AccountId, int Offset, int Limit) : IRequest<List<TransactionResponse>>;

    internal sealed class GetTransactionsWithAccountQueryValidator : AbstractValidator<GetTransactionsWithAccountQuery>
    {
        private readonly ValidationSettings opts;

        public GetTransactionsWithAccountQueryValidator(IOptions<ValidationSettings> options)
        {
            this.opts = options.Value;
            RuleFor(t => t.AccountId).NotEmpty();
            RuleFor(t => t.Offset).GreaterThanOrEqualTo(0);
            RuleFor(t => t.Limit).GreaterThan(0).LessThanOrEqualTo(opts.MaxLimit);
        }
    }

    internal sealed class GetTransactionsWithAccountQueryHandler : IRequestHandler<GetTransactionsWithAccountQuery, List<TransactionResponse>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;

        public GetTransactionsWithAccountQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }

        public async Task<List<TransactionResponse>> Handle(GetTransactionsWithAccountQuery request, CancellationToken cancellationToken)
        {
            List<Transaction> result = await unitOfWork.Transactions.GetWithAccountAsync(request.AccountId, cancellationToken);
            return mapper.Map<List<TransactionResponse>>(result);
        }
    }
}
