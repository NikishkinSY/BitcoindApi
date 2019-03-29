using System;
using Bitcoind.Core.Bitcoind;
using BitcoindApi.Cache;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
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
        private readonly ILogger<BitcoinController> _logger;

        public BitcoinController(
            ILogger<BitcoinController> logger,
            IBitcoindClient bitcoindClient,
            IMemoryCache cache)
        {
            _bitcoindClient = bitcoindClient;
            _cache = cache;
            _logger = logger;
        }

        [HttpPost("sendbtc")]
        public async Task<IActionResult> SendBtc([FromQuery] string address, [FromQuery] decimal amount, [FromQuery] string fromWallet = null)
        {
            throw new BitcoindException("sdfsf");
            var result = await _bitcoindClient.ValidateAddressAsync(address);
            if (!result.Result.Isvalid)
                return StatusCode(400, $"invalid address ({address})");

            if (amount <= 0)
                return StatusCode(400, $"invalid amount ({amount})");

            await _bitcoindClient.SendToAddressAsync(address, amount, fromWallet);

            return StatusCode(200);
        }

        [HttpGet("getlast")]
        public IEnumerable<Core.Dto.TransactionDto> GetLast()
        {
            return _cache.Get<IEnumerable<Core.Dto.TransactionDto>>(CacheConsts.LastIncomeTransactionsKey);
        }
    }
}