namespace TestWebApp.Application.Transactions.Queries
{
    using AutoMapper;
    using AutoMapper.Configuration.Conventions;
    using FluentValidation;
    using MediatR;
    using Microsoft.Extensions.Options;
    using System;
    using System.Collections.Generic;
    using System.Data.SqlTypes;
    using System.Linq;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using TestWebApp.Application.Contracts.Database;
    using TestWebApp.Application.Contracts.Database.Models;
    using TestWebApp.Application.Transactions.Common;
    using TestWebApp.Domain;

    public class GetTransactionsQuery : IRequest<List<TransactionResponse>>
    {
        public Guid? AccountFrom { get; set; }

        public Guid? AccountTo { get; set; }

        public DateTime? DateFrom { get; set; }

        public DateTime? DateTo { get; set; }

        public decimal? AmountMinimum { get; set; }

        public decimal? AmountMaximum { get; set; }

        public int Limit { get; set; }

        public int Offset { get; set; }
    }

    internal sealed class GetTransactionsQueryValidator : AbstractValidator<GetTransactionsQuery>
    {
        private readonly ValidationSettings opts;

        public GetTransactionsQueryValidator(IOptions<ValidationSettings> options)
        {
            opts = options.Value;
            RuleFor(t => t.Limit).GreaterThan(0).LessThanOrEqualTo(opts.MaxLimit);
            RuleFor(t => t.Offset).GreaterThanOrEqualTo(0);
        }
    }

    internal sealed class GetTransactionsQueryHandler : IRequestHandler<GetTransactionsQuery, List<TransactionResponse>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;

        public GetTransactionsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }

        public async Task<List<TransactionResponse>> Handle(GetTransactionsQuery request, CancellationToken cancellationToken)
        {
            TransactionFilter filter = new TransactionFilter();
            filter.AccountFrom = request.AccountFrom;
            filter.AccountTo = request.AccountTo;
            filter.AmountMinimum = request.AmountMinimum;
            filter.AmountMaximum = request.AmountMaximum;
            filter.DateFrom = request.DateFrom;
            filter.DateTo = request.DateTo;
            filter.Offset = request.Offset;
            filter.Limit = request.Limit;

            List<Transaction> transactions = await unitOfWork.Transactions.GetAsync(filter, cancellationToken);
            return mapper.Map<List<TransactionResponse>>(transactions);
        }
    }
}
