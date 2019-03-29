using Bitcoind.Core.Bitcoind;
using Bitcoind.Core.Helpers;
using Bitcoind.Core.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Bitcoind.Service.HostServices
{
    public class UpdateTransactionsHostedService: BackgroundService
    {
        private readonly ITransactionService _transactionService;
        private readonly ILogger<UpdateWalletsHostedService> _logger;
        private readonly AppSettings _appSettings;

        public UpdateTransactionsHostedService(
            ILogger<UpdateWalletsHostedService> logger,
            IServiceScopeFactory scopeFactory)
        {
            _logger = logger;
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
                await _transactionService.PullTransactionsAsync(_appSettings.MaxGetTransactions);
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
