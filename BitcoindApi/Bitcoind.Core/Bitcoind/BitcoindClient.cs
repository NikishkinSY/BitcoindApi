using Bitcoind.Core.Bitcoind.Dto;
using RestSharp;
using System.Net;
using System.Threading.Tasks;

namespace Bitcoind.Core.Bitcoind
{
    public class BitcoindClient
    {
        private const string GetBalancePath = "/wallet/";

        private const string ListTransactionsCommand = "listtransactions";
        private const string GetBalanceCommand = "getbalance";
        private const string ListWalletsCommand = "listwallets";
        private const string SendToAddressCommand = "sendtoaddress";
        
        private readonly string _domain;
        private readonly string _login;
        private readonly string _password;
        private readonly string _version;
        private readonly RestClient _client;

        public BitcoindClient(string domain, string login, string password, string version)
        {
            _domain = domain;
            _login = login;
            _password = password;
            _version = version;
            _client = CreateClient();
        }

        public async Task<SendToAddressDto> SendToAddressAsync(string address, decimal amount)
        {
            var request = GetRequest(string.Empty);

            request.AddJsonBody(new
            {
                jsonrpc = _version,
                id = string.Empty,
                method = SendToAddressCommand,
                @params = new JsonArray { address, amount }
            });

            var response = await _client.ExecuteTaskAsync<SendToAddressDto>(request);
            return response.Data;
        }

        public async Task<ListWalletsDto> GetListWalletsAsync()
        {
            var request = GetRequest(string.Empty);

            request.AddJsonBody(new
            {
                jsonrpc = _version,
                id = string.Empty,
                method = ListWalletsCommand
            });

            var response = await _client.ExecuteTaskAsync<ListWalletsDto>(request);
            return response.Data;
        }

        public async Task<GetBalanceDto> GetBalanceAsync(string wallet)
        {
            var request = GetRequest(GetBalancePath + wallet);
            
            request.AddJsonBody(new
            {
                jsonrpc = _version,
                id = string.Empty,
                method = GetBalanceCommand,
                @params = new JsonArray { "*" }
            });

            var response = await _client.ExecuteTaskAsync<GetBalanceDto>(request);
            return response.Data;
        }

        public async Task<ListTransactionsDto> GetListTransactionsAsync(string wallet)
        {
            var request = GetRequest(GetBalancePath + wallet);

            request.AddJsonBody(new
            {
                jsonrpc = _version,
                id = string.Empty,
                method = ListTransactionsCommand
            });

            var response = await _client.ExecuteTaskAsync<ListTransactionsDto>(request);
            return response.Data;
        }

        private RestClient CreateClient()
        {
            return new RestClient(_domain);
        }

        private IRestRequest GetRequest(string path)
        {
            var request = new RestRequest(path, Method.POST)
            {
                Credentials = new NetworkCredential(_login, _password)
            };

            return request;
        }
    }
}
