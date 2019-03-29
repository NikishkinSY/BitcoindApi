using System.Collections.Generic;
using System.Threading.Tasks;
using Bitcoind.Core.Bitcoind.Dto;
using Bitcoind.Core.Bitcoind.DTO;

namespace Bitcoind.Core.Bitcoind
{
    public interface IBitcoindClient
    {
        Task<string> SendToAddressAsync(string address, decimal amount, string wallet = null);
        Task<List<string>> GetListWalletsAsync();
        Task<decimal> GetBalanceAsync(string wallet);
        Task<List<BitcoinTransactionDto>> GetListTransactionsAsync(string wallet, int count = 20);
        Task<ValidateAddressResult> ValidateAddressAsync(string address);
        Task<BitcoinSingleTransactionDto> GetTransactionAsync(string wallet, string txid);
    }
}
