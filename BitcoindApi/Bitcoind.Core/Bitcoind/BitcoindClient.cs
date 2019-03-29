using System.Collections.Generic;
using Bitcoind.Core.Bitcoind.Dto;
using Bitcoind.Core.Helpers;
using Microsoft.Extensions.Logging;
using RestSharp;
using System.Net;
using System.Threading.Tasks;
using Bitcoind.Core.Bitcoind.DTO;

namespace Bitcoind.Core.Bitcoind
{
    public class BitcoindClient: IBitcoindClient
    {
        private const string ListTransactionsCommand = "listtransactions";
        private const string GetBalanceCommand = "getbalance";
        private const string ListWalletsCommand = "listwallets";
        private const string SendToAddressCommand = "sendtoaddress";
        private const string ValidateAddressCommand = "validateaddress";
        private const string GetTransactionCommand = "gettransaction";

        private readonly string _domain;
        private readonly string _login;
        private readonly string _password;
        private readonly string _version;
        private readonly RestClient _client;

        private readonly ILogger<BitcoindClient> _logger;

        public BitcoindClient(
            ILogger<BitcoindClient> logger,
            AppSettings appSettings)
        {
            _domain = appSettings.BitcoindServer;
            _login = appSettings.BitcoindUser;
            _password = appSettings.BitcoindPassword;
            _version = appSettings.BitcoindRpcJsonVersion;
            _client = CreateClient();
            _logger = logger;
        }
        
        public async Task<Response<string>> SendToAddressAsync(string address, decimal amount, string wallet = null)
        {
            var request = GetRequest(wallet);

            request.AddJsonBody(new
            {
                jsonrpc = _version,
                id = string.Empty,
                method = SendToAddressCommand,
                @params = new JsonArray { address, amount }
            });

            return await HandleRequestAsync<Response<string>>(request);
        }

        public async Task<Response<List<string>>> GetListWalletsAsync()
        {
            var request = GetRequest(string.Empty);

            request.AddJsonBody(new
            {
                jsonrpc = _version,
                id = string.Empty,
                method = ListWalletsCommand
            });

            return await HandleRequestAsync<Response<List<string>>>(request);
        }

        public async Task<Response<decimal>> GetBalanceAsync(string wallet)
        {
            var request = GetRequest(wallet);
            
            request.AddJsonBody(new
            {
                jsonrpc = _version,
                id = string.Empty,
                method = GetBalanceCommand,
                @params = new JsonArray { "*" }
            });

            return await HandleRequestAsync<Response<decimal>>(request);
        }

        public async Task<Response<List<BitcoinTransactionDto>>> GetListTransactionsAsync(string wallet, int count = 20)
        {
            var request = GetRequest(wallet);

            request.AddJsonBody(new
            {
                jsonrpc = _version,
                id = string.Empty,
                method = ListTransactionsCommand,
                @params = new JsonArray { "*", count, 0 }
            });

            return await HandleRequestAsync<Response<List<BitcoinTransactionDto>>>(request);
        }

        public async Task<Response<ValidateAddressResult>> ValidateAddressAsync(string address)
        {
            var request = GetRequest(string.Empty);

            request.AddJsonBody(new
            {
                jsonrpc = _version,
                id = string.Empty,
                method = ValidateAddressCommand,
                @params = new JsonArray { address }
            });

            return await HandleRequestAsync<Response<ValidateAddressResult>>(request);
        }

        public async Task<Response<BitcoinSingleTransactionDto>> GetTransactionAsync(string txid)
        {
            var request = GetRequest(string.Empty);

            request.AddJsonBody(new
            {
                jsonrpc = _version,
                id = string.Empty,
                method = GetTransactionCommand,
                @params = new JsonArray { txid }
            });

            return await HandleRequestAsync<Response<BitcoinSingleTransactionDto>>(request);
        }

        private async Task<T> HandleRequestAsync<T>(IRestRequest request)
        {
            var response = await _client.ExecuteTaskAsync<T>(request);
            if (!response.IsSuccessful)
            {
                throw new BitcoindException(response.Content ?? response.ErrorMessage);
            }

            return response.Data;
        }

        private RestClient CreateClient()
        {
            return new RestClient(_domain);
        }

        private IRestRequest GetRequest(string wallet)
        {
            var request = new RestRequest($"/wallet/{wallet ?? ""}", Method.POST)
            {
                Credentials = new NetworkCredential(_login, _password)
            };

            return request;
        }
    }
}
