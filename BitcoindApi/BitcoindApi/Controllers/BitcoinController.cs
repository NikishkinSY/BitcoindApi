using BitcoindApi.Bitcoind;
using BitcoindApi.DAL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace BitcoindApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BitcoinController : ControllerBase
    {
        private readonly DataContext _dataContext;
        private readonly BitcoindClient _bitcoindClient;
        private IMemoryCache _cache;

        public BitcoinController(
            DataContext dataContext,
            BitcoindClient bitcoindClient,
            IMemoryCache cache)
        {
            _dataContext = dataContext;
            _bitcoindClient = bitcoindClient;
            _cache = cache;
        }

        [HttpPost("sendcoin")]
        public void SendCoin(string address, decimal amount)
        {
            
        }

        [HttpGet("getlast")]
        public void GetLast()
        {

        }

        [HttpGet("blocknotify")]
        public void BlockNotify()
        {
            
        }

        [HttpGet("walletnotify")]
        public void WalletNotify()
        {

        }
    }
}