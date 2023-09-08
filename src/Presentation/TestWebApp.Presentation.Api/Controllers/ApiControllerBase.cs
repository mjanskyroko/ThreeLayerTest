namespace TestWebApp.Presentation.Api.Controllers
{
    using System;
    using System.Net.Mime;
    using MediatR;
    using Microsoft.AspNetCore.Mvc;

    [ApiController]
    [Produces(MediaTypeNames.Application.Json)]
    [Route("api/v{version:apiVersion}/[controller]")]
    public abstract class ApiControllerBase : ControllerBase
    {
        public IMediator Mediator { get; set; }

        protected ApiControllerBase(IMediator mediator)
        {
            this.Mediator = mediator;
        }

        protected async Task<IActionResult> ProcessAsync<TCommand, TResponse>(TCommand command) where TCommand : IRequest<TResponse>
        {
            TResponse response = await this.Mediator.Send(command);
            if (response is null)
                return this.NotFound();
            return this.Ok(response);
        }

        protected async Task<IActionResult> ProcessAsync<TCommand>(TCommand command) where TCommand : IRequest
        {
            await this.Mediator.Send(command);
            return this.NoContent();
        }
    }
}
