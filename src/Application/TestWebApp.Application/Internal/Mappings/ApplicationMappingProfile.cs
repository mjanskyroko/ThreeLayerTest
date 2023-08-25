namespace TestWebApp.Application.Internal.Mappings
{
    using AutoMapper;
    using TestWebApp.Application.Transactions.Common;
    using TestWebApp.Application.Users.Common;
    using TestWebApp.Domain;

    public class ApplicationMappingProfile : Profile
    {
        public ApplicationMappingProfile()
        {
            CreateMap<User, UserResponse>();
            CreateMap<Transaction, TransactionResponse>();
            //CreateMap<Account, AccountResponse>();
        }
    }
}
