using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using BitcoindApi.Bitcoind.Dto;
using BitcoindApi.Helpers;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Serialization;
using RestSharp;
using RestSharp.Deserializers;

namespace BitcoindApi.Bitcoind
{
    public class BitcoindClient
    {
        private const string SendBtcCommand = "";
        private const string ListTransactionsCommand = "listtransactions";
        private const string ListAddressGroupingsCommand = "listaddressgroupings";

        private readonly string _domain;
        private readonly string _login;
        private readonly string _password;
        private readonly string _version;
        private readonly RestClient _client;

        public BitcoindClient(IOptions<AppSettings> appSettings)
        {
            _domain = appSettings.Value.BitcoindServer;
            _login = appSettings.Value.BitcoindUser;
            _password = appSettings.Value.BitcoindPassword;
            _version = appSettings.Value.BitcoindRpcJsonVersion;
            _client = CreateClient();
        }

        public async Task<ListAddressGroupings> GetListAddressGroupingsAsync()
        {
            var request = GetRequest();
            request.AddJsonBody(new
            {
                jsonrpc = _version,
                id = string.Empty,
                method = ListAddressGroupingsCommand
            });

            var response = await _client.ExecuteTaskAsync<ListAddressGroupings>(request);
            return response.Data;
        }

        public async Task<ListTransactions> GetListTransactionsAsync()
        {
            var request = GetRequest();

            request.AddJsonBody(new
            {
                jsonrpc = _version,
                id = string.Empty,
                method = ListTransactionsCommand
            });

            var response = await _client.ExecuteTaskAsync<ListTransactions>(request);
            return response.Data;
        }

        private RestClient CreateClient()
        {
            return new RestClient(_domain);
        }

        private IRestRequest GetRequest()
        {
            var request = new RestRequest(string.Empty, Method.POST)
            {
                Credentials = new NetworkCredential(_login, _password)
            };
            
            return request;
        }
    }
}
