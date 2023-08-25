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

    public record GetTransactionByIdQuery(Guid Id) : IRequest<TransactionResponse>;

    internal sealed class GetTransactionByIdQueryValidator : AbstractValidator<GetTransactionByIdQuery>
    {
        public GetTransactionByIdQueryValidator()
        {
            RuleFor(t => t.Id).NotEmpty();
        }
    }

    internal sealed class GetTransactionByIdQueryHandler : IRequestHandler<GetTransactionByIdQuery, TransactionResponse>
    {
        private readonly IUnitOfWork unitOfOfWork;
        private readonly IMapper mapper;

        public GetTransactionByIdQueryHandler(IUnitOfWork unitOfOfWork, IMapper mapper)
        {
            this.unitOfOfWork = unitOfOfWork;
            this.mapper = mapper;
        }

        public async Task<TransactionResponse> Handle(GetTransactionByIdQuery request, CancellationToken cancellationToken)
        {
            Transaction t = await unitOfOfWork.Transactions.GetByIdAsync(request.Id, cancellationToken);
            return mapper.Map<TransactionResponse>(t);
        }
    }
}
