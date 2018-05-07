using Newtonsoft.Json;
using StocksBot.StocksProviders.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace StocksBot.StocksProviders
{
    public class InvestorsExchangeStockProvider : IStockProvider
    {
        private const string BaseUrl = "https://api.iextrading.com/1.0/";

        private readonly IHttpClientFactory httpClientFactory;

        public InvestorsExchangeStockProvider(IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory;
        }

        public async Task<Company> GetCompanyAsync(string symbol, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(symbol))
                throw new ArgumentException("Invalid symbol", nameof(symbol));

            var url = new Flurl.Url(BaseUrl).AppendPathSegments("stock", symbol, "company");
            var httpClient = this.httpClientFactory.CreateClient();
            var response = await httpClient.GetAsync(url, cancellationToken);
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            var companyInfo = JsonConvert.DeserializeObject<Company>(content);
            return companyInfo;
        }

        public async Task<List<SymbolDescription>> GetSymbolsAsync(CancellationToken cancellationToken)
        {
            var url = new Flurl.Url(BaseUrl).AppendPathSegment("/ref-data/symbols");
            var httpClient = this.httpClientFactory.CreateClient();
            var response = await httpClient.GetAsync(url, cancellationToken);
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            var symbolDescriptions = JsonConvert.DeserializeObject<List<SymbolDescription>>(content);
            return symbolDescriptions;
        }
    }
}
