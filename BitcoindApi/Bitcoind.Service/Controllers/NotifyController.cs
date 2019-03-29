using System;
using System.Collections.Generic;
using System.Linq;
using Bitcoind.Core.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System.Threading.Tasks;
using Bitcoind.Core.Bitcoind;
using Bitcoind.Core.Helpers;
using Bitcoind.Service.HostServices;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Bitcoind.Service.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotifyController : ControllerBase
    {
        private readonly ILogger<GlobalExceptionFilter> _logger;
        private readonly IEnumerable<IHostedService> _hostedServices;

        public NotifyController(
            ILogger<GlobalExceptionFilter> logger,
            IEnumerable<IHostedService> hostedServices)
        {
            _logger = logger;
            _hostedServices = hostedServices;
        }

        // block notify when new block come
        [HttpGet("block")]
        public IActionResult Block()
        {
            _logger.LogInformation("New Block");
            var updateWalletHostedService = _hostedServices.FirstOrDefault(x => x is UpdateWalletsHostedService) as UpdateWalletsHostedService;
            updateWalletHostedService?.ContinueLoop();
            return StatusCode(200);
        }

        // wallet notify when new transaction come
        [HttpGet("wallet")]
        public IActionResult Wallet()
        {
            _logger.LogInformation("New Transaction");
            var updateTransactionsHostedService = _hostedServices.FirstOrDefault(x => x is UpdateTransactionsHostedService) as UpdateTransactionsHostedService;
            updateTransactionsHostedService?.ContinueLoop();
            return StatusCode(200);
        }
    }
}
