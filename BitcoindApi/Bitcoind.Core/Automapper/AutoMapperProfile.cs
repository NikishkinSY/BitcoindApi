using System.Linq;
using AutoMapper;
using Bitcoind.Core.Bitcoind.Dto;
using Bitcoind.Core.Bitcoind.DTO;
using Bitcoind.Core.DAL.Entities;
using Bitcoind.Core.Helpers;

namespace Bitcoind.Core.Automapper
{
    public class AutoMapperProfile: Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<BitcoinSingleTransactionDto, Transaction>()
                .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.Details.First().Address))
                .ForMember(dest => dest.Amount, opt => opt.MapFrom(src => src.Details.First().Amount))
                .ForMember(dest => dest.Date, opt => opt.MapFrom(src => src.Time.FromUnixToDateTime()))
                .ForMember(dest => dest.Category, opt => opt.ConvertUsing(new CategoryConverter(), x => x.Details.First().Category));
            CreateMap<BitcoinTransactionDto, Transaction>()
                .ForMember(dest => dest.Date, opt => opt.MapFrom(src => src.Time.FromUnixToDateTime()))
                .ForMember(dest => dest.Category, opt => opt.ConvertUsing(new CategoryConverter(), x => x.Category));
            CreateMap<Transaction, Dto.TransactionDto>();
        }
    }
}
