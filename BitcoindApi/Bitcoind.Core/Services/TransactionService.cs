using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Bitcoind.Core.Bitcoind;
using Bitcoind.Core.Bitcoind.Dto;
using Bitcoind.Core.DAL;
using Bitcoind.Core.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace Bitcoind.Core.Services
{
    public class TransactionService
    {
        private readonly DataContext _dataContext;
        private readonly BitcoindClient _bitcoindClient;
        private readonly IMemoryCache _cache;

        public TransactionService(
            DataContext dataContext,
            BitcoindClient bitcoindClient,
            IMemoryCache cache)
        {
            _dataContext = dataContext;
            _bitcoindClient = bitcoindClient;
            _cache = cache;
        }

        public async Task<List<Transaction>> PullTransactionsAsync()
        {
            var newTransactions = new List<Transaction>();

            var wallets = await _bitcoindClient.GetListWalletsAsync();
            foreach (var wallet in wallets.Result)
            {
                var transactions = await _bitcoindClient.GetListTransactionsAsync(wallet);
                var dbListTransactions = Mapper.Map<List<Transaction>>(transactions.Result);
                
                foreach (var transaction in dbListTransactions)
                {
                    var tran = _dataContext.Transactions.Find(transaction.Txid);
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

                newTransactions.AddRange(newTransactions);
            }
            
            await _dataContext.SaveChangesAsync();

            return newTransactions;
        }

        public async Task<IEnumerable<Transaction>> GetLastIncomeTransactionsAsync()
        {
            var newTransactions = await PullTransactionsAsync();
            var newIncomeTransactions = newTransactions.Where(x => x.Category == Category.Receive);
            var notConfirmedIncomeTransactions = await _dataContext.Transactions.Where(x => x.Confirmations < 3).ToListAsync();
            var lastIncomeTransactions = notConfirmedIncomeTransactions.Union(newIncomeTransactions).ToList();
            return lastIncomeTransactions;
        }
    }
}
