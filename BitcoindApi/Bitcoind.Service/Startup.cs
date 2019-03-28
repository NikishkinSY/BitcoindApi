using AutoMapper;
using Bitcoind.Core.Automapper;
using Bitcoind.Core.Bitcoind;
using Bitcoind.Core.DAL;
using Bitcoind.Core.Helpers;
using Bitcoind.Core.Services;
using Bitcoind.Service.HostServices;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;

namespace Bitcoind.Service
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMemoryCache();
            services.AddDbContext<DataContext>(x => x.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
            services.AddMvc(options =>
            {
                options.Filters.Add(typeof(GlobalExceptionFilter));
            }).SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            var appSettings = Configuration.GetSection(nameof(AppSettings)).Get<AppSettings>();
            services.AddScoped(provider => Configuration.GetSection(nameof(AppSettings)).Get<AppSettings>());
            services.AddScoped<IBitcoindClient, BitcoindClient>(x => new BitcoindClient(services.BuildServiceProvider().GetService<ILogger<BitcoindClient>>(), appSettings));
            services.AddTransient<ITransactionService, TransactionService>();
            services.AddTransient<IWalletService, WalletService>();

            services.AddSingleton<IHostedService, UpdateWalletsHostedService>();
            services.AddSingleton<IHostedService, UpdateTransactionsHostedService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();

            using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetRequiredService<DataContext>();
                context.Database.EnsureCreated();
            }

            //init automapper
            Mapper.Initialize(cfg => {
                cfg.AddProfile<AutoMapperProfile>();
            });
        }
    }
}
