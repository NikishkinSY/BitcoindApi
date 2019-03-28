using Bitcoind.Core.Bitcoind;
using Bitcoind.Core.DAL.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;
using Bitcoind.Core.DAL;
using EFCore.BulkExtensions;

namespace Bitcoind.Core.Services
{
    public class WalletService
    {
        private readonly BitcoindClient _bitcoindClient;
        private readonly DataContext _dataContext;

        public WalletService(
            BitcoindClient bitcoindClient,
            DataContext dataContext)
        {
            _bitcoindClient = bitcoindClient;
            _dataContext = dataContext;
        }

        public async Task<List<HotWallet>> GetWalletsAsync()
        {
            var balances = new List<HotWallet>();
            var wallets = await _bitcoindClient.GetListWalletsAsync();

            foreach (var wallet in wallets.Result)
            {
                var balance = await _bitcoindClient.GetBalanceAsync(wallet);
                balances.Add(new HotWallet
                {
                    Address = wallet,
                    Balance = balance.Result
                });
            }

            return balances;
        }

        public async Task UpdateWalletsAsync(List<HotWallet> wallets)
        {
            await _dataContext.BulkInsertOrUpdateAsync(wallets);
        }
    }
}
