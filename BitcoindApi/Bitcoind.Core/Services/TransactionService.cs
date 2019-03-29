using System;
using AutoMapper;
using Bitcoind.Core.Bitcoind;
using Bitcoind.Core.DAL;
using Bitcoind.Core.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bitcoind.Core.Bitcoind.DTO;

namespace Bitcoind.Core.Services
{
    public class TransactionService: ITransactionService
    {
        private readonly DataContext _dataContext;
        private readonly IBitcoindClient _bitcoindClient;

        public TransactionService(
            DataContext dataContext,
            IBitcoindClient bitcoindClient)
        {
            _dataContext = dataContext;
            _bitcoindClient = bitcoindClient;
        }

        public async Task<IEnumerable<Transaction>> PullTransactionsAsync(int maxGetTransactions = 20, int updateTransactionsWithConfirmationLessThan = 6)
        {
            var newTransactions = new List<Transaction>();
            var wallets = await _bitcoindClient.GetListWalletsAsync();

            foreach (var wallet in wallets)
            {
                var transactionsDto = await _bitcoindClient.GetListTransactionsAsync(wallet, maxGetTransactions);
                var transactionsDb = Mapper.Map<List<Transaction>>(transactionsDto);

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
                    else if (tran.Confirmations <= updateTransactionsWithConfirmationLessThan)
                    {
                        tran.Confirmations = transaction.Confirmations;
                    }
                }
            }

            await _dataContext.SaveChangesAsync();

            return newTransactions;
        }

        public async Task<IEnumerable<Transaction>> GetLastIncomeTransactionsAsync(int incomeTransactionsWithConfirmationsLessThan = 3)
        {
            var lastIncomeTransactions = await _dataContext.Transactions.Where(x => x.Category == Category.Receive && (x.Confirmations < incomeTransactionsWithConfirmationsLessThan || !x.IsShown)).ToListAsync();
            foreach (var transaction in lastIncomeTransactions)
            {
                transaction.IsShown = true;
            }

            await _dataContext.SaveChangesAsync();
            return lastIncomeTransactions.OrderBy(x => x.Date);
        }

        public async Task<bool> IsNewSendReceiveTransactionAsync(string txid)
        {
            var wallets = await _bitcoindClient.GetListWalletsAsync();

            foreach (var wallet in wallets)
            {
                try
                {
                    var transactionDto = await _bitcoindClient.GetTransactionAsync(wallet, txid);
                    var transaction = Mapper.Map<Transaction>(transactionDto);

                    return transaction.Category != Category.Unknown;
                }
                catch (BitcoindException e)
                {
                    // if code=-5 it means wallet doesn't contain this transaction
                    var error = (Error)e.Data["error"];
                    if (error?.Code != -5)
                        throw;
                }
            }

            return false;
        }
    }
}
