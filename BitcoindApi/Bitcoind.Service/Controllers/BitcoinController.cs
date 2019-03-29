using System;
using System.Collections;
using Bitcoind.Core.Bitcoind;
using BitcoindApi.Cache;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Bitcoind.Core.DAL;
using Bitcoind.Core.Helpers;
using Bitcoind.Core.Services;

namespace Bitcoind.Service.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BitcoinController : ControllerBase
    {
        private readonly ITransactionService _transactionService;
        private readonly IBitcoindClient _bitcoindClient;

        public BitcoinController(
            ITransactionService transactionService,
            IBitcoindClient bitcoindClient)
        {
            _transactionService = transactionService;
            _bitcoindClient = bitcoindClient;
        }

        [HttpPost("sendbtc")]
        public async Task<IActionResult> SendBtc([FromQuery] string address, [FromQuery] decimal amount, [FromQuery] string fromWallet = null)
        {
            if (!BitcoinHelper.CheckAddress(address)
                && !(await _bitcoindClient.ValidateAddressAsync(address)).Result.Isvalid)
                return StatusCode(400, $"invalid address ({address})");

            if (amount <= 0)
                return StatusCode(400, $"invalid amount ({amount})");

            await _bitcoindClient.SendToAddressAsync(address, amount, fromWallet);

            return StatusCode(200);
        }

        [HttpGet("getlast")]
        public async Task<IEnumerable<Core.Dto.TransactionDto>> GetLast()
        {
            var lastIncomeTransactions = await _transactionService.GetLastIncomeTransactionsAsync();
            return Mapper.Map<List<Core.Dto.TransactionDto>>(lastIncomeTransactions);
        }
    }
}