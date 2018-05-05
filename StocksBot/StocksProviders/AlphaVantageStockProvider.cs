using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using StocksBot.StocksProviders.Models;
using System;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace StocksBot.StocksProviders
{
    public class AlphaVantageStockProvider : IStockProvider
    {
        private const string BaseUrl = "https://www.alphavantage.co/query";
        private const string TimeSeriesDaily = "TIME_SERIES_DAILY";

        private readonly IHttpClientFactory httpClientFactory;
        private readonly IConfiguration configuration;

        public AlphaVantageStockProvider(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            this.httpClientFactory = httpClientFactory;
            this.configuration = configuration;
        }

        public async Task<DailyPrices> GetDailyPrices(string symbol, CancellationToken cancellationToken)
        {
            var uriQueryBuilder = new StringBuilder(BaseUrl + "?");
            uriQueryBuilder.AppendFormat("function={0}", TimeSeriesDaily);
            uriQueryBuilder.AppendFormat("&symbol={0}", symbol);
            uriQueryBuilder.AppendFormat("&apikey={0}", this.configuration["AlphaVantageApiKey"]);

            var httpClient = this.httpClientFactory.CreateClient();
            var response = await httpClient.GetAsync(uriQueryBuilder.ToString(), cancellationToken);

            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            var timeSeries = JsonConvert.DeserializeObject<AlphaVantageTimeSeriesDailyResponse>(content);
            return timeSeries.ToDailyPrices();
        }
    }

}
