﻿namespace TestWebApp.Presentation.Api.Controllers.V1
{
    using System;

    using MediatR;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using TestWebApp.Application.Accounts.Commands;
    using TestWebApp.Application.Accounts.Common;
    using TestWebApp.Application.Accounts.Queries;
    using TestWebApp.Presentation.Api.Internal.Constants;

    [ApiVersion(ApiVersions.V1)]
    public class AccountsController : ApiControllerBase
    {
        public AccountsController(IMediator mediator) : base(mediator) { }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateAccountCommand command)
        {
            return await this.ProcessAsync(command);
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> FromUser([FromQuery] GetAccountsQuery query)
        {
            return await this.ProcessAsync<GetAccountsQuery, List<AccountResponse>>(query);
        }

        [AllowAnonymous]
        [HttpGet("id")]
        public async Task<IActionResult> Get([FromQuery] GetAccountByIdQuery query)
        {
            return await this.ProcessAsync<GetAccountByIdQuery, AccountResponse>(query);
        }

        [AllowAnonymous]
        [HttpDelete]
        public async Task<IActionResult> Delete([FromBody] DeleteAccountByIdCommand command)
        {
            return await this.ProcessAsync(command);
        }

        [AllowAnonymous]
        [HttpPatch]
        public async Task<IActionResult> Update([FromBody] UpdateAccountCommand command)
        {
            return await this.ProcessAsync(command);
        }
    }
}
