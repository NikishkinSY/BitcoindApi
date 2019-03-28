using Bitcoind.Core.Bitcoind;
using Bitcoind.Core.Helpers;
using BitcoindApi.Cache;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Bitcoind.Service.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BitcoinController : ControllerBase
    {
        private readonly IBitcoindClient _bitcoindClient;
        private readonly IMemoryCache _cache;

        public BitcoinController(
            IBitcoindClient bitcoindClient,
            IMemoryCache cache)
        {
            _bitcoindClient = bitcoindClient;
            _cache = cache;
        }

        [HttpPost("sendbtc")]
        public async Task SendBtc([FromQuery] string address, [FromQuery] decimal amount, [FromQuery] string fromWallet = null)
        {
            var result = await _bitcoindClient.ValidateAddressAsync(address);
            if (!result.Result.Isvalid)
                return;

            if (amount <= 0)
                return;

            await _bitcoindClient.SendToAddressAsync(address, amount, fromWallet);
        }

        [HttpGet("getlast")]
        public IEnumerable<Core.Dto.TransactionDto> GetLast()
        {
            return _cache.Get<IEnumerable<Core.Dto.TransactionDto>>(CacheConsts.LastIncomeTransactionsKey);
        }
    }
}