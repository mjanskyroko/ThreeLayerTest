namespace TestWebApp.Application.Users.Queries
{
    using AutoMapper;
    using FluentValidation;
    using MediatR;
    using TestWebApp.Application.Contracts.Database;
    using TestWebApp.Application.Contracts.Database.Models;
    using TestWebApp.Application.Users.Common;
    using TestWebApp.Domain;

    public class GetUsersQuery : IRequest<List<UserResponse>>
    {
        public string? Name { get; set; }

        public DateTime? JoinDate { get; set; }

        public int Offset { get; set; }

        public int Limit { get; set; }
    }

    internal class GetUsersQueryValidator : AbstractValidator<GetUsersQuery>
    {
        public GetUsersQueryValidator()
        {
            RuleFor(u => u.Offset).GreaterThanOrEqualTo(0);
            RuleFor(u => u.Limit).GreaterThan(0).LessThanOrEqualTo(100);
        }
    }

    internal class GetUsersQueryHandler : IRequestHandler<GetUsersQuery, List<UserResponse>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;

        public GetUsersQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }

        public async Task<List<UserResponse>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
        {
            UserFilter filter = new UserFilter();
            filter.Offset = request.Offset;
            filter.Limit = request.Limit;
            filter.Name = request.Name;
            filter.JoinDate = request.JoinDate;

            List<User> users = await unitOfWork.Users.GetAsync(filter, cancellationToken);
            return mapper.Map<List<UserResponse>>(users);
        }
    }
}
