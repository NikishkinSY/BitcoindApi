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
        private readonly WalletService _walletService;
        private readonly ILogger<UpdateWalletsHostedService> _logger;
        private readonly AppSettings _appSettings;

        public UpdateWalletsHostedService(
            ILogger<UpdateWalletsHostedService> logger,
            IServiceScopeFactory scopeFactory)
        {
            _logger = logger;
            _serviceScope = scopeFactory.CreateScope();
            _walletService = _serviceScope.ServiceProvider.GetRequiredService<WalletService>();
            _appSettings = _serviceScope.ServiceProvider.GetRequiredService<AppSettings>();
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogDebug("UpdateWalletsHostedService is starting.");

            stoppingToken.Register(() =>
                _logger.LogDebug("UpdateWallets background task is stopping."));

            if (_appSettings.UpdateWalletsDelayInSeconds > 0)
            {
                while (!stoppingToken.IsCancellationRequested)
                {
                    _logger.LogDebug("UpdateWallets task doing background work.");

                    await UpdateWallets();
                    await Task.Delay(_appSettings.UpdateWalletsDelayInSeconds * 1000, stoppingToken);
                }
            }
            else
            {
                _logger.LogDebug("UpdateWalletsDelayInSeconds is not set in config file.");
            }

            _logger.LogDebug("UpdateWallets background task is stopping.");
        }

        private async Task UpdateWallets()
        {
            try
            {
                var wallets = await _walletService.GetWalletsAsync();
                await _walletService.UpdateWalletsAsync(wallets);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "UpdateWalletsHostedService");
            }
        }

        public override void Dispose()
        {
            _serviceScope.Dispose();
            base.Dispose();
        }
    }
}
