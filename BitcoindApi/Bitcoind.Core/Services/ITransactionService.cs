using Bitcoind.Core.DAL.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Bitcoind.Core.Services
{
    public interface ITransactionService
    {
        Task<List<Transaction>> PullTransactionsAsync();
        Task<IEnumerable<Dto.TransactionDto>> GetLastIncomeTransactionsAsync();
    }
}
