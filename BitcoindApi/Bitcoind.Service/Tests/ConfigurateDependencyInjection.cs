using Bitcoind.Core.Bitcoind;
using Bitcoind.Core.DAL;
using Bitcoind.Core.Services;
using BitcoindApi.Helpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BitcoindApi.Tests
{
    public class ConfigurateDependencyInjection
    {
        public static ServiceProvider Configurate()
        {
            var services = new ServiceCollection();
            var appSettingsSection = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("AppSettings");
            services.Configure<AppSettings>(appSettingsSection);
            services.AddTransient<WalletService>();

            var appSettings = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection(nameof(AppSettings)).Get<AppSettings>();
            services.AddScoped(x => new BitcoindClient(appSettings.BitcoindServer, appSettings.BitcoindUser, appSettings.BitcoindPassword, appSettings.BitcoindRpcJsonVersion));

            var connectionString = @"Server=.\SQLEXPRESS;Database=TestBitcoind;Trusted_Connection=True;ConnectRetryCount=0";
            services.AddDbContext<DataContext>(x => x.UseSqlServer(connectionString));
            return services.BuildServiceProvider();
        }
    }
}
