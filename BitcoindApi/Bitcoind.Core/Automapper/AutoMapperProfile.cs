using AutoMapper;
using Bitcoind.Core.Bitcoind.Dto;
using Bitcoind.Core.DAL.Entities;
using BitcoindApi.Helpers;

namespace Bitcoind.Core.Automapper
{
    public class AutoMapperProfile: Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<TransactionDto, Transaction>()
                .ForMember(dest => dest.Date, opt => opt.MapFrom(src => src.Time.FromUnixToDateTime()));
        }
    }
}
