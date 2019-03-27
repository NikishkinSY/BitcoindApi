using AutoMapper;
using BitcoindApi.Bitcoind.Dto;
using BitcoindApi.DAL.Entities;
using System.Collections.Generic;

namespace BitcoindApi.Helpers.Automapper
{
    public class HotWalletConverter : ITypeConverter<ListAddressGroupings, IEnumerable<HotWallet>>
    {
        public IEnumerable<HotWallet> Convert(ListAddressGroupings source, IEnumerable<HotWallet> destination, ResolutionContext context)
        {
            var hotWallets = new List<HotWallet>();
            foreach (var addressGroup in source.Result)
            {
                foreach (var address in addressGroup)
                {
                    if (address.Count >= 2)
                    {
                        if (string.IsNullOrWhiteSpace(address[0])
                            && decimal.TryParse(address[1], out decimal result))
                        {
                            hotWallets.Add(new HotWallet
                            {
                                Address = address[0],
                                Balance = result
                            });
                        }
                    }
                }
            }

            return hotWallets;
        }
    }
}
