namespace TestWebApp.Presentation.Api.Controllers.V1
{
    using MediatR;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using TestWebApp.Application.Users.Commands;
    using TestWebApp.Application.Users.Common;
    using TestWebApp.Application.Users.Queries;
    using TestWebApp.Presentation.Api.Internal.Constants;

    [ApiVersion(ApiVersions.V1)]
    public class UsersController : ApiControllerBase
    {
        public UsersController(IMediator mediator) : base(mediator) { }

        [AllowAnonymous]
        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] CreateUserCommand command)
        {
            return await this.ProcessAsync(command);
        }

        [AllowAnonymous]
        [HttpGet("users")]
        public async Task<IActionResult> List([FromQuery] GetUsersQuery query)
        {
            return await this.ProcessAsync<GetUsersQuery, List<UserResponse>>(query);
        }


        [AllowAnonymous]
        [HttpGet("user")]
        public async Task<IActionResult> Get([FromQuery] GetUserByIdQuery query)
        {
            return await this.ProcessAsync<GetUserByIdQuery, UserResponse>(query);
        }

        [AllowAnonymous]
        [HttpDelete("delete")]
        public async Task<IActionResult> Delete([FromBody] DeleteUserByIdCommand command)
        {
            return await this.ProcessAsync(command);
        }

        [AllowAnonymous]
        [HttpPut("update")]
        public async Task<IActionResult> Update([FromBody] UpdateUserCommand command)
        {
            return await this.ProcessAsync(command);
        }
    }
}
