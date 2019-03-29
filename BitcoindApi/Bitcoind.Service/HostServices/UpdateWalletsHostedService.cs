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
    public class UpdateWalletsHostedService: BackgroundService
    {
        private readonly IWalletService _walletService;
        private readonly ILogger<UpdateWalletsHostedService> _logger;
        private readonly AppSettings _appSettings;

        public UpdateWalletsHostedService(
            ILoggerFactory loggerFactory,
            IServiceScopeFactory scopeFactory)
        {
            _serviceScope = scopeFactory.CreateScope();
            _logger = loggerFactory.CreateLogger<UpdateWalletsHostedService>();
            _walletService = _serviceScope.ServiceProvider.GetRequiredService<IWalletService>();
            _appSettings = _serviceScope.ServiceProvider.GetRequiredService<AppSettings>();
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.Register(() => {});
            if (_appSettings.UpdateWalletsDelayInSeconds > 0)
            {
                while (!stoppingToken.IsCancellationRequested)
                {
                    await UpdateWallets();
                    await Delay(_appSettings.UpdateWalletsDelayInSeconds);
                }
            }
        }

        private async Task UpdateWallets()
        {
            try
            {
                var wallets = await _walletService.GetWalletsAsync();
                await _walletService.UpdateWalletsAsync(wallets);
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
