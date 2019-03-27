using System.Collections.Generic;
using AutoMapper;
using BitcoindApi.Bitcoind.Dto;
using BitcoindApi.DAL.Entities;
using BitcoindApi.Helpers.Automapper;

namespace BitcoindApi.Helpers
{
    public class AutoMapperProfile: Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<ListAddressGroupings, IEnumerable<HotWallet>>().ConvertUsing(new HotWalletConverter());
            CreateMap<Bitcoind.Dto.Transaction, DAL.Entities.Transaction>()
                .ForMember(dest => dest.Date, opt => opt.MapFrom(src => src.Time.FromUnixToDateTime()));
        }
    }
}
