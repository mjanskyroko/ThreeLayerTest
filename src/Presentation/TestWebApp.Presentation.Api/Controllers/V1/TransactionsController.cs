namespace TestWebApp.Presentation.Api.Controllers.V1
{
    using System;

    using MediatR;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using TestWebApp.Application.Transactions.Commands;
    using TestWebApp.Application.Transactions.Common;
    using TestWebApp.Application.Transactions.Queries;
    using TestWebApp.Presentation.Api.Internal.Constants;

    [ApiVersion(ApiVersions.V1)]
    public class TransactionsController : ApiControllerBase
    {
        public TransactionsController(IMediator mediator) : base(mediator) { }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateTransactionCommand command)
        {
            return await this.ProcessAsync(command);
        }

        [AllowAnonymous]
        [HttpGet("with-account")]
        public async Task<IActionResult> WithAccount([FromQuery] GetTransactionsWithAccountQuery query)
        {
            return await this.ProcessAsync<GetTransactionsWithAccountQuery, List<TransactionResponse>>(query);
        }

        [AllowAnonymous]
        [HttpGet("with-user")]
        public async Task<IActionResult> WithUser([FromQuery] GetTransactionsWithUserQuery query)
        {
            return await this.ProcessAsync<GetTransactionsWithUserQuery, List<TransactionResponse>>(query);
        }

        [AllowAnonymous]
        [HttpGet("id")]
        public async Task<IActionResult> Get([FromQuery] GetTransactionByIdQuery query)
        {
            return await this.ProcessAsync<GetTransactionByIdQuery, TransactionResponse>(query);
        }
    }
}
