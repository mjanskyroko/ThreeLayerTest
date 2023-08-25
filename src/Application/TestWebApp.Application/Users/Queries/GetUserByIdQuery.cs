using AutoMapper;
using FluentValidation;
using MediatR;
using TestWebApp.Application.Contracts.Database;
using TestWebApp.Application.Users.Common;
using TestWebApp.Domain;

namespace TestWebApp.Application.Users.Queries
{
    public record GetUserByIdQuery(Guid Id) : IRequest<UserResponse>;

    public sealed class GetUserByIdQueryValidator : AbstractValidator<GetUserByIdQuery>
    {
        public GetUserByIdQueryValidator()
        {
            RuleFor(u => u.Id).NotEmpty();
        }
    }

    public sealed class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, UserResponse>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;

        public GetUserByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }

        public async Task<UserResponse> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
        {
            User u = await unitOfWork.Users.GetByIdAsync(request.Id, cancellationToken);
            return mapper.Map<UserResponse>(u);
        }
    }
}
