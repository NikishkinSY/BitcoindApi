using Bitcoind.Core.Bitcoind;
using Bitcoind.Core.DAL;
using Bitcoind.Core.Helpers;
using Bitcoind.Core.Services;
using Bitcoind.Service.HostServices;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Bitcoind.Service.Helpers
{
    public static class DIConfiguration
    {
        public static void Configurate(IServiceCollection services, IConfiguration configuration)
        {
            services.AddMemoryCache();
            services.AddDbContext<DataContext>(x => x.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            var appSettings = configuration.GetSection(nameof(AppSettings)).Get<AppSettings>();
            services.AddScoped(provider => configuration.GetSection(nameof(AppSettings)).Get<AppSettings>());
            services.AddScoped<IBitcoindClient, BitcoindClient>(x => new BitcoindClient(services.BuildServiceProvider().GetService<ILogger<BitcoindClient>>(), appSettings));
            services.AddTransient<ITransactionService, TransactionService>();
            services.AddTransient<IWalletService, WalletService>();

            services.AddSingleton<IHostedService, UpdateWalletsHostedService>();
            //services.AddSingleton<IHostedService, UpdateTransactionsHostedService>();
        }
    }
}
