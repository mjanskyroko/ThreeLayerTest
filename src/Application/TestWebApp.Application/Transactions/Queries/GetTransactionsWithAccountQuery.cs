namespace TestWebApp.Application.Transactions.Queries
{
    using AutoMapper;
    using FluentValidation;
    using MediatR;
    using System.Threading;
    using System.Threading.Tasks;
    using TestWebApp.Application.Contracts.Database;
    using TestWebApp.Application.Transactions.Common;
    using TestWebApp.Domain;

    public record GetTransactionsWithAccountQuery(Guid AccountId) : IRequest<List<TransactionResponse>>;

    internal sealed class GetTransactionsWithAccountQueryValidator : AbstractValidator<GetTransactionsWithAccountQuery>
    {
        public GetTransactionsWithAccountQueryValidator()
        {
            RuleFor(t => t.AccountId).NotEmpty();
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
