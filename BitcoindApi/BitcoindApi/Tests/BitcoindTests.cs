using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using BitcoindApi.Bitcoind;
using BitcoindApi.DAL;
using BitcoindApi.DAL.Entities;
using BitcoindApi.Helpers;
using EFCore.BulkExtensions;
using NUnit.Framework;

namespace BitcoindApi.Tests
{
    [TestFixture]
    public class BitcoindTests
    {
        private BitcoindClient _bitcoindClient;
        private DataContext _dataContext;

        [SetUp]
        public void Setup()
        {
            var provider = ConfigurateDependencyInjection.Configurate();
            _bitcoindClient = (BitcoindClient)provider.GetService(typeof(BitcoindClient));

            _dataContext = (DataContext)provider.GetService(typeof(DataContext));

            //init automapper
            Mapper.Initialize(cfg => {
                cfg.AddProfile<AutoMapperProfile>();
            });
        }

        [Test]
        public void CanGetListTransactions()
        {
            var transactions = _bitcoindClient.GetListTransactionsAsync().Result;
        }

        [Test]
        public void CanGetListAddressGroupings()
        {
            var addresses = _bitcoindClient.GetListAddressGroupingsAsync().Result;
            var parsedAddresses = Mapper.Map<List<HotWallet>>(addresses);
            _dataContext.Database.EnsureCreated();
            var walls = _dataContext.HotWallets.ToList();

            _dataContext.BulkInsertOrUpdate(parsedAddresses);
            var newwalls = _dataContext.HotWallets.ToList();
        }
    }
}
