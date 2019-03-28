using AutoMapper;
using Bitcoind.Core.Bitcoind.Dto;
using Bitcoind.Core.DAL.Entities;
using Bitcoind.Core.Helpers;

namespace Bitcoind.Core.Automapper
{
    public class AutoMapperProfile: Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<BitcoinTransactionDto, Transaction>()
                .ForMember(dest => dest.Date, opt => opt.MapFrom(src => src.Time.FromUnixToDateTime()))
                .ForMember(dest => dest.Category, opt => opt.MapFrom<CategoryResolver>());
            CreateMap<Transaction, Dto.TransactionDto>();
        }
    }
}
