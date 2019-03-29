using System.Collections.Generic;
using System.Threading.Tasks;
using Bitcoind.Core.Bitcoind.Dto;
using Bitcoind.Core.Bitcoind.DTO;

namespace Bitcoind.Core.Bitcoind
{
    public interface IBitcoindClient
    {
        Task<Response<string>> SendToAddressAsync(string address, decimal amount, string wallet = null);
        Task<Response<List<string>>> GetListWalletsAsync();
        Task<Response<decimal>> GetBalanceAsync(string wallet);
        Task<Response<List<BitcoinTransactionDto>>> GetListTransactionsAsync(string wallet, int count = 20);
        Task<Response<ValidateAddressResult>> ValidateAddressAsync(string address);
        Task<Response<BitcoinSingleTransactionDto>> GetTransactionAsync(string txid);
    }
}
