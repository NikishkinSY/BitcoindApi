using Bitcoind.Core.Helpers;
using Bitcoind.Core.Services;
using BitcoindApi.Cache;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;
using Bitcoind.Core.Bitcoind;

namespace Bitcoind.Service.HostServices
{
    public class UpdateTransactionsHostedService: BackgroundService
    {
        private readonly ITransactionService _transactionService;
        private readonly ILogger<UpdateWalletsHostedService> _logger;
        private readonly AppSettings _appSettings;
        private readonly IMemoryCache _cache;

        public UpdateTransactionsHostedService(
            ILogger<UpdateWalletsHostedService> logger,
            IServiceScopeFactory scopeFactory,
            IMemoryCache cache)
        {
            _logger = logger;
            _cache = cache;
            _serviceScope = scopeFactory.CreateScope();
            _transactionService = _serviceScope.ServiceProvider.GetRequiredService<ITransactionService>();
            _appSettings = _serviceScope.ServiceProvider.GetRequiredService<AppSettings>();
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.Register(() => { });
            if (_appSettings.UpdateTransactionsDelayInSeconds > 0)
            {
                while (!stoppingToken.IsCancellationRequested)
                {
                    await UpdateTransactions();
                    await Delay(_appSettings.UpdateTransactionsDelayInSeconds);
                }
            }
        }

        private async Task UpdateTransactions()
        {
            try
            {
                _cache.Set(CacheConsts.LastIncomeTransactionsKey,
                    await _transactionService.GetLastIncomeTransactionsAsync());
            }
            catch (BitcoindException e)
            {
                _logger.LogError(e, $"Error during proccesing bitcoind command ({e.Message})");
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Unexpected Error ({e.Message})");
            }
        }
    }
}
