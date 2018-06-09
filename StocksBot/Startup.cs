﻿using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
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
            services.Configure<TelegramConfiguration>(this.Configuration.GetSection("Telegram"));
            services.AddTransient<IStockProvider, InvestorsExchangeStockProvider>();
            services.AddTransient<ITelegramBotClientFactory, TelegramBotClientFactory>();
            services.AddTransient<ITelegramBot, TelegramBot>();
            services.AddTransient<IUpdateParser, UpdateParser>();
            services.AddSingleton(sp => new CompanyInfoProvider(sp.GetService<IStockProvider>().GetSymbolsAsync(CancellationToken.None).GetAwaiter().GetResult()));
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
