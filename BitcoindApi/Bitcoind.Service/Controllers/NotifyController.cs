using Bitcoind.Core.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using BitcoindApi.Cache;
using Microsoft.Extensions.Caching.Memory;

namespace Bitcoind.Service.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotifyController : ControllerBase
    {
        private readonly TransactionService _transactionService;
        private readonly WalletService _walletService;
        private readonly IMemoryCache _cache;

        public NotifyController(
            TransactionService transactionService,
            WalletService walletService,
            IMemoryCache cache)
        {
            _transactionService = transactionService;
            _walletService = walletService;
            _cache = cache;
        }

        // GET api/values
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            return new string[] { "value1", "value2" };
        }

        [HttpGet("blocknotify")]
        public async Task BlockNotify()
        {
            var wallets = await _walletService.GetWalletsAsync();
            await _walletService.UpdateWalletsAsync(wallets);
        }

        [HttpGet("walletnotify")]
        public async Task WalletNotify()
        {
            _cache.Set(CacheConsts.LastIncomeTransactionsKey,
                await _transactionService.GetLastIncomeTransactionsAsync());
        }
    }
}
