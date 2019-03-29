using AutoMapper;
using Bitcoind.Core.Bitcoind;
using Bitcoind.Core.DAL;
using Bitcoind.Core.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BitcoindApi.Cache;
using Microsoft.Extensions.Caching.Memory;

namespace Bitcoind.Core.Services
{
    public class TransactionService: ITransactionService
    {
        private readonly DataContext _dataContext;
        private readonly IBitcoindClient _bitcoindClient;
        private readonly IMemoryCache _cache;

        public TransactionService(
            DataContext dataContext,
            IBitcoindClient bitcoindClient,
            IMemoryCache cache)
        {
            _dataContext = dataContext;
            _bitcoindClient = bitcoindClient;
            _cache = cache;
        }

        public async Task<IEnumerable<Transaction>> PullTransactionsAsync()
        {
            var newTransactions = new List<Transaction>();
            var wallets = await _bitcoindClient.GetListWalletsAsync();

            foreach (var wallet in wallets.Result)
            {
                var transactionsDto = await _bitcoindClient.GetListTransactionsAsync(wallet);
                var transactionsDb = Mapper.Map<List<Transaction>>(transactionsDto.Result);

                foreach (var transaction in transactionsDb)
                {
                    if (transaction.Category == Category.Unknown)
                    {
                        continue;
                    }

                    var tran = await _dataContext.Transactions
                        .FirstOrDefaultAsync(x => x.Txid == transaction.Txid && x.Category == transaction.Category);
                    if (tran == null)
                    {
                        transaction.Wallet = wallet;
                        _dataContext.Transactions.Add(transaction);
                        newTransactions.Add(transaction);
                    }
                    else if (tran.Confirmations <= 6)
                    {
                        tran.Confirmations = transaction.Confirmations;
                    }
                }
            }

            var amount = await _dataContext.SaveChangesAsync();

            if (amount > 0)
            {
                _cache.Remove(CacheConsts.LastIncomeTransactionsKey);
            }

            return newTransactions;
        }

        public async Task<IEnumerable<Transaction>> GetLastIncomeTransactionsAsync()
        {
            var lastIncomeTransactions = await _dataContext.Transactions.Where(x => x.Confirmations < 3 || !x.IsShown).ToListAsync();
            foreach (var transaction in lastIncomeTransactions)
            {
                transaction.IsShown = true;
            }

            await _dataContext.SaveChangesAsync();
            return lastIncomeTransactions.OrderBy(x => x.Date);
        }

        public async Task<bool> IsNewSendReceiveTransactionAsync(string txid)
        {
            var transactionDto = await _bitcoindClient.GetTransactionAsync(txid);
            var transaction = Mapper.Map<Transaction>(transactionDto.Result);

            return transaction.Category != Category.Unknown;
        }
    }
}
