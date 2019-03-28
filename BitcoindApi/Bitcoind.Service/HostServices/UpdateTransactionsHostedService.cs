using Bitcoind.Core.Helpers;
using Bitcoind.Core.Services;
using BitcoindApi.Cache;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Bitcoind.Service.HostServices
{
    public class UpdateTransactionsHostedService: BackgroundService
    {
        private readonly TransactionService _transactionService;
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
            _transactionService = _serviceScope.ServiceProvider.GetRequiredService<TransactionService>();
            _appSettings = _serviceScope.ServiceProvider.GetRequiredService<AppSettings>();
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogDebug("UpdateTransactionsHostedService is starting.");

            stoppingToken.Register(() =>
                _logger.LogDebug("UpdateTransactions background task is stopping."));
            
            if (_appSettings.UpdateTransactionsDelayInSeconds > 0)
            {
                while (!stoppingToken.IsCancellationRequested)
                {
                    _logger.LogDebug("UpdateTransactions task doing background work.");

                    await UpdateTransactions();

                    await Task.Delay(_appSettings.UpdateTransactionsDelayInSeconds * 1000, stoppingToken);
                }
            }
            else
            {
                _logger.LogDebug("UpdateTransactionsDelayInSeconds is not set in config file.");
            }

            _logger.LogDebug("UpdateTransactions background task is stopping.");
        }

        private async Task UpdateTransactions()
        {
            try
            {
                _cache.Set(CacheConsts.LastIncomeTransactionsKey,
                    await _transactionService.GetLastIncomeTransactionsAsync());
            }
            catch (Exception e)
            {
                _logger.LogError(e, "UpdateTransactionsHostedService");
            }
        }
    }
}
