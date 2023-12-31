﻿namespace TestWebApp.Application.Users.Queries
{
    using AutoMapper;
    using FluentValidation;
    using MediatR;
    using Microsoft.Extensions.Options;
    using TestWebApp.Application.Contracts.Database;
    using TestWebApp.Application.Contracts.Database.Models;
    using TestWebApp.Application.Users.Common;
    using TestWebApp.Domain;

    public class GetUsersQuery : IRequest<List<UserResponse>>
    {
        public string? Name { get; set; }

        public DateTime? JoinDateFrom { get; set; }

        public DateTime? JoinDateTo { get; set; }

        public int Offset { get; set; }

        public int Limit { get; set; }
    }

    internal class GetUsersQueryValidator : AbstractValidator<GetUsersQuery>
    {
        private readonly ValidationSettings opts;

        public GetUsersQueryValidator(IOptions<ValidationSettings> options)
        {
            opts = options.Value;
            RuleFor(u => u.Offset).GreaterThanOrEqualTo(0);
            RuleFor(u => u.Limit).GreaterThan(0).LessThanOrEqualTo(opts.MaxLimit);
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
            filter.JoinDateFrom = request.JoinDateFrom;
            filter.JoinDateTo = request.JoinDateTo;

            List<User> users = await unitOfWork.Users.GetAsync(filter, cancellationToken);
            return mapper.Map<List<UserResponse>>(users);
        }
    }
}
