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
        public async Task<IActionResult> SendBtc([FromQuery] string address, [FromQuery] decimal amount, [FromQuery] string fromWallet = null)
        {
            var result = await _bitcoindClient.ValidateAddressAsync(address);
            if (!result.Result.Isvalid)
                return StatusCode(400, $"invalid address ({address})");

            if (amount <= 0)
                return StatusCode(400, $"invalid amount ({amount})");

            var response = await _bitcoindClient.SendToAddressAsync(address, amount, fromWallet);

            if (response.Error != null)
            {
                return StatusCode(400, response.Error.Message);
            }

            return StatusCode(200);
        }

        [HttpGet("getlast")]
        public IEnumerable<Core.Dto.TransactionDto> GetLast()
        {
            return _cache.Get<IEnumerable<Core.Dto.TransactionDto>>(CacheConsts.LastIncomeTransactionsKey);
        }
    }
}