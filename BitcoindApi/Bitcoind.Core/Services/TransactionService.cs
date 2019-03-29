﻿using AutoMapper;
using Bitcoind.Core.Bitcoind;
using Bitcoind.Core.DAL;
using Bitcoind.Core.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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

            await _dataContext.SaveChangesAsync();

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
