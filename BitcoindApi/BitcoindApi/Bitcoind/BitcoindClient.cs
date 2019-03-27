using System.Net;
using System.Threading.Tasks;
using BitcoindApi.Bitcoind.Dto;
using Newtonsoft.Json.Serialization;
using RestSharp;
using RestSharp.Deserializers;

namespace BitcoindApi.Bitcoind
{
    public class BitcoindClient
    {
        private const string SendBtcCommand = "";
        private const string ListTransactionsCommand = "listtransactions";

        private readonly string _domain;
        private readonly string _login;
        private readonly string _password;
        private readonly string _version;

        public BitcoindClient(string domain, string login, string password, string version)
        {
            _domain = domain;
            _login = login;
            _password = password;
            _version = version;
        }

        public async Task<ListTransactions> GetListTransactionsAsync()
        {
            var client = CreateClient();
            var request = GetRequest();

            request.AddJsonBody(new
            {
                jsonrpc = _version,
                id = string.Empty,
                method = ListTransactionsCommand
            });

            var response = await client.ExecuteTaskAsync<ListTransactions>(request);
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
