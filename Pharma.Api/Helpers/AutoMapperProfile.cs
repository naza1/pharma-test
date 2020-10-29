using AutoMapper;
using IPharmaAuth0.Models;
using Pharma.Api.Models;
using Pharma.Common.Model;

namespace Pharma.Api.Helpers
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Account, AccountModel>();
            CreateMap<RegisterModel, Account>();
            CreateMap<UpdateModel, Account>();
        }
    }
}
