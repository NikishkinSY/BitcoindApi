using AutoMapper;
using BitcoindApi.Bitcoind;
using BitcoindApi.DAL;
using BitcoindApi.DAL.Managers;
using BitcoindApi.Helpers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BitcoindApi
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
            services.AddDbContext<DataContext>(x => x.UseSqlite(Configuration.GetConnectionString("DefaultConnection")));
            services.AddMvc(options =>
            {
                options.Filters.Add(typeof(GlobalExceptionFilter));
            }).SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            services.AddScoped<BitcoindClient>();
            services.AddScoped<TransactionManager>();
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
