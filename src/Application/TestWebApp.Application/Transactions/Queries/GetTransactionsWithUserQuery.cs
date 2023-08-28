namespace TestWebApp.Application.Transactions.Queries
{
    using AutoMapper;
    using FluentValidation;
    using MediatR;
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using TestWebApp.Application.Contracts.Database;
    using TestWebApp.Application.Transactions.Common;
    using TestWebApp.Domain;

    public record GetTransactionsWithUserQuery(Guid UserId) : IRequest<List<TransactionResponse>>;

    internal sealed class GetTransactionsWithUserQueryValidator : AbstractValidator<GetTransactionsWithUserQuery>
    {
        public GetTransactionsWithUserQueryValidator()
        {
            RuleFor(t => t.UserId).NotEmpty();
        }
    }

    internal sealed class GetTransactionsWithUserQueryHandler : IRequestHandler<GetTransactionsWithUserQuery, List<TransactionResponse>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;

        public GetTransactionsWithUserQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }

        public async Task<List<TransactionResponse>> Handle(GetTransactionsWithUserQuery request, CancellationToken cancellationToken)
        {
            List<Transaction> transactions = await unitOfWork.Transactions.GetWithUserAsync(request.UserId, cancellationToken);
            return mapper.Map<List<TransactionResponse>>(transactions);
        }
    }
}
