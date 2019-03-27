using Microsoft.AspNetCore.Mvc;

namespace BitcoindApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BitcoinController : ControllerBase
    {
        [HttpPost]
        public void SendBtc(string address, decimal amount)
        {
            
        }

        public void GetLast()
        {
            
        }

        [HttpGet]
        public void Update()
        {
            
        }
    }
}