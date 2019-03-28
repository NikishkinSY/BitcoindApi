using Bitcoind.Core.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System.Threading.Tasks;
using Bitcoind.Core.Helpers;
using Microsoft.Extensions.Logging;

namespace Bitcoind.Service.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotifyController : ControllerBase
    {
        private readonly ITransactionService _transactionService;
        private readonly IWalletService _walletService;
        private readonly IMemoryCache _cache;
        private readonly ILogger<GlobalExceptionFilter> _logger;

        public NotifyController(
            ITransactionService transactionService,
            IWalletService walletService,
            IMemoryCache cache,
            ILogger<GlobalExceptionFilter> logger)
        {
            _transactionService = transactionService;
            _walletService = walletService;
            _cache = cache;
            _logger = logger;
        }
        
        [HttpGet("block")]
        public async Task Block()
        {
            _logger.LogInformation("New Block");
            var wallets = await _walletService.GetWalletsAsync();
            await _walletService.UpdateWalletsAsync(wallets);
        }

        //[HttpGet("wallet")]
        //public async Task Wallet()
        //{
        //    _cache.Set(CacheConsts.LastIncomeTransactionsKey,
        //        await _transactionService.GetLastIncomeTransactionsAsync());
        //}
    }
}
