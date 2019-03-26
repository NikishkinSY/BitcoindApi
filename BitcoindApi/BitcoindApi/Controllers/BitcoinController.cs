using Microsoft.AspNetCore.Mvc;

namespace BitcoindApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BitcoinController : ControllerBase
    {
        public void SendBtc(string address, decimal amount)
        {
            
        }
    }
}