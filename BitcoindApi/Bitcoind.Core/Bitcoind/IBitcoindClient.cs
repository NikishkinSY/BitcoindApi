using System.Threading.Tasks;
using Bitcoind.Core.Bitcoind.Dto;
using Bitcoind.Core.Bitcoind.DTO;

namespace Bitcoind.Core.Bitcoind
{
    public interface IBitcoindClient
    {
        Task<SendToAddressDto> SendToAddressAsync(string address, decimal amount, string wallet = null);
        Task<ListWalletsDto> GetListWalletsAsync();
        Task<GetBalanceDto> GetBalanceAsync(string wallet);
        Task<ListTransactionsDto> GetListTransactionsAsync(string wallet);
        Task<ValidateAddressResponse> ValidateAddressAsync(string address);
    }
}
