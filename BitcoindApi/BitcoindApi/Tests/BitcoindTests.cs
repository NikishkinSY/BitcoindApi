using BitcoindApi.Bitcoind;
using NUnit.Framework;

namespace BitcoindApi.Tests
{
    [TestFixture]
    public class BitcoindTests
    {
        [SetUp]
        public void Setup()
        {
            //var provider = ConfigurateDependencyInjection.Configurate();
            //_emailService = (IEmailService)provider.GetService(typeof(IEmailService));
        }

        [Test]
        public void CanGetListTransactions()
        {
            var bitcoindClient = new BitcoindClient("http://127.0.0.1:8332", "bitcoinrpc", "6CmcZwXLNW6ciAycGQC6HTxFy4QcorY7ikxe9TiksPeW", "1.0");
            var transactions = bitcoindClient.GetListTransactionsAsync().Result;
        }
    }
}
