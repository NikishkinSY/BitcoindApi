using Bitcoind.Core.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System.Threading.Tasks;

namespace Bitcoind.Service.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotifyController : ControllerBase
    {
        private readonly ITransactionService _transactionService;
        private readonly IWalletService _walletService;
        private readonly IMemoryCache _cache;

        public NotifyController(
            ITransactionService transactionService,
            IWalletService walletService,
            IMemoryCache cache)
        {
            _transactionService = transactionService;
            _walletService = walletService;
            _cache = cache;
        }
        
        [HttpGet("blocknotify")]
        public async Task BlockNotify()
        {
            var wallets = await _walletService.GetWalletsAsync();
            await _walletService.UpdateWalletsAsync(wallets);
        }

        //[HttpGet("walletnotify")]
        //public async Task WalletNotify()
        //{
        //    _cache.Set(CacheConsts.LastIncomeTransactionsKey,
        //        await _transactionService.GetLastIncomeTransactionsAsync());
        //}
    }
}
