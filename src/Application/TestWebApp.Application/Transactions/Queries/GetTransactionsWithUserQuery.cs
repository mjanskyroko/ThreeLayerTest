namespace TestWebApp.Application.Transactions.Queries
{
    using AutoMapper;
    using FluentValidation;
    using MediatR;
    using Microsoft.Extensions.Options;
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using TestWebApp.Application.Contracts.Database;
    using TestWebApp.Application.Transactions.Common;
    using TestWebApp.Domain;

    public record GetTransactionsWithUserQuery(Guid UserId, int Offset, int Limit) : IRequest<List<TransactionResponse>>;

    internal sealed class GetTransactionsWithUserQueryValidator : AbstractValidator<GetTransactionsWithUserQuery>
    {
        private readonly ValidationSettings opts;

        public GetTransactionsWithUserQueryValidator(IOptions<ValidationSettings> options)
        {
            opts = options.Value;
            RuleFor(t => t.UserId).NotEmpty();
            RuleFor(t => t.Offset).GreaterThanOrEqualTo(0);
            RuleFor(t => t.Limit).GreaterThan(0).LessThanOrEqualTo(opts.MaxLimit);
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
