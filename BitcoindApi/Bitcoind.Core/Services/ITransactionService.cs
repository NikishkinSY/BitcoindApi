using Bitcoind.Core.DAL.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Bitcoind.Core.Services
{
    public interface ITransactionService
    {
        Task<IEnumerable<Transaction>> PullTransactionsAsync(int maxGetTransactions = 20,
            int updateTransactionsWithConfirmationLessThan = 6);
        Task<IEnumerable<Transaction>> GetLastIncomeTransactionsAsync(
            int incomeTransactionsWithConfirmationsLessThan = 3);
        Task<bool> IsNewSendReceiveTransactionAsync(string txid);
    }
}
