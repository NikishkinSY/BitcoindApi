using Bitcoind.Core.DAL.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Bitcoind.Core.Services
{
    public interface IWalletService
    {
        Task<List<HotWallet>> GetWalletsAsync();
        Task UpdateWalletsAsync(List<HotWallet> wallets);
    }
}
