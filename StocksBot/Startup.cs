using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StocksBot.StocksProviders;
using StocksBot.Telegram;
using System.Threading;

namespace StocksBot
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            this.Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            services.AddHttpClient();
            services.AddStackExchangeRedisCache(options => {
                options.Configuration = this.Configuration.GetSection("Redis").GetValue<string>("Host");
                options.InstanceName = "Redis";
            });
            services.Configure<TelegramConfiguration>(this.Configuration.GetSection("Telegram"));
            services.Configure<IEXConfiguration>(this.Configuration.GetSection("IEX"));
            services.AddScoped<InvestorsExchangeStockProvider>();
            services.AddScoped<IStockProvider>(provider =>
                new CachedStockProvider(
                    provider.GetRequiredService<InvestorsExchangeStockProvider>(),
                    provider.GetRequiredService<IDistributedCache>()));
            services.AddScoped<ITelegramBotClientFactory, TelegramBotClientFactory>();
            services.AddScoped<ITelegramBot, TelegramBot>();
            services.AddScoped<IUpdateParser, UpdateParser>();
            services.AddSingleton(sp =>
            {
                using (var scope = sp.CreateScope())
                {
                    return new CompanyInfoProvider(scope.ServiceProvider.GetService<IStockProvider>().GetSymbolsAsync(CancellationToken.None).GetAwaiter().GetResult());
                }
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }
            
            app.UseMvc();
        }
    }
}
