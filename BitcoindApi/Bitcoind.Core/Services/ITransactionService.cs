using Bitcoind.Core.DAL.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Bitcoind.Core.Services
{
    public interface ITransactionService
    {
        Task<IEnumerable<Transaction>> PullTransactionsAsync(int count);
        Task<IEnumerable<Transaction>> GetLastIncomeTransactionsAsync();
        Task<bool> IsNewSendReceiveTransactionAsync(string txid);
    }
}
