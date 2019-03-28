using Bitcoind.Core.Bitcoind;
using Bitcoind.Core.DAL.Entities;
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
        private readonly BitcoindClient _bitcoindClient;
        private readonly IMemoryCache _cache;

        public BitcoinController(
            BitcoindClient bitcoindClient,
            IMemoryCache cache)
        {
            _bitcoindClient = bitcoindClient;
            _cache = cache;
        }

        [HttpPost("sendbtc")]
        public async Task SendBtc(string address, decimal amount)
        {
            await _bitcoindClient.SendToAddressAsync(address, amount);
        }

        [HttpGet("getlast")]
        public IEnumerable<Transaction> GetLast()
        {
            return _cache.Get<IEnumerable<Transaction>>(CacheConsts.LastIncomeTransactionsKey);
        }
    }
}