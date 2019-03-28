using AutoMapper;
using Bitcoind.Core.Automapper;
using Bitcoind.Core.Bitcoind;
using Bitcoind.Core.DAL;
using Bitcoind.Core.Services;
using Bitcoind.Service.Helpers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace Bitcoind.Service.Tests
{
    [TestFixture]
    public class BitcoindTests
    {
        private IBitcoindClient _bitcoindClient;
        private DataContext _dataContext;
        private WalletService _walletService;

        [SetUp]
        public void Setup()
        {
            var services = new ServiceCollection();
            var configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            DIConfiguration.Configurate(services, configuration);
            var provider = services.BuildServiceProvider();

            _bitcoindClient = provider.GetRequiredService<IBitcoindClient>();
            //_walletService = (WalletService)provider.GetService(typeof(WalletService));
            //_dataContext = (DataContext)provider.GetService(typeof(DataContext));

            //init automapper
            Mapper.Initialize(cfg => {
                cfg.AddProfile<AutoMapperProfile>();
            });
        }

        [TestCase("first")]
        public void CanGetListTransactions(string wallet)
        {
            var transactions = _bitcoindClient.GetListTransactionsAsync(wallet).Result;
        }

        [Test]
        public void CanGetListAddressGroupings()
        {
            var wallets = _bitcoindClient.GetListWalletsAsync().Result;
            //_dataContext.Database.EnsureCreated();
            //var walls = _dataContext.HotWallets.ToList();

            //_dataContext.BulkInsertOrUpdate(parsedAddresses);
            //var newwalls = _dataContext.HotWallets.ToList();
        }

        [TestCase("first")]
        public void CanGetBalance(string wallet)
        {
            var wallets = _bitcoindClient.GetBalanceAsync(wallet).Result;
            //_dataContext.Database.EnsureCreated();
            //var walls = _dataContext.HotWallets.ToList();

            //_dataContext.BulkInsertOrUpdate(parsedAddresses);
            //var newwalls = _dataContext.HotWallets.ToList();
        }

        [TestCase("", 1, "")]
        public void CanSendToAddress(string address, int amount, string fromWallet)
        {
            var result = _bitcoindClient.SendToAddressAsync(address, amount, fromWallet).Result;
            //_dataContext.Database.EnsureCreated();
            //var walls = _dataContext.HotWallets.ToList();

            //_dataContext.BulkInsertOrUpdate(parsedAddresses);
            //var newwalls = _dataContext.HotWallets.ToList();
        }

        [Test]
        public void UpdateWallets()
        {
            var wallets = _walletService.GetWalletsAsync().Result;
            _walletService.UpdateWalletsAsync(wallets).Wait();
            //_dataContext.Database.EnsureCreated();
            //var walls = _dataContext.HotWallets.ToList();

            //_dataContext.BulkInsertOrUpdate(parsedAddresses);
            //var newwalls = _dataContext.HotWallets.ToList();
        }

        [TestCase("2NCEZmYTnzCDdpGqxN5UyyN7XM5REA3AREsL")]
        public void ValidateAddress(string address)
        {
            var result = _bitcoindClient.ValidateAddressAsync(address).Result;
            //_dataContext.Database.EnsureCreated();
            //var walls = _dataContext.HotWallets.ToList();

            //_dataContext.BulkInsertOrUpdate(parsedAddresses);
            //var newwalls = _dataContext.HotWallets.ToList();
        }
    }
}
