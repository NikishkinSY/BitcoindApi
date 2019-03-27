﻿using BitcoindApi.Bitcoind;
using BitcoindApi.DAL;
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
            services.AddScoped<BitcoindClient>();
            var connectionString = @"Server=.\SQLEXPRESS;Database=TestBitcoind;Trusted_Connection=True;ConnectRetryCount=0";
            services.AddDbContext<DataContext>(x => x.UseSqlServer(connectionString));
            return services.BuildServiceProvider();
        }
    }
}
