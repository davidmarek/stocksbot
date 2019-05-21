using Microsoft.Extensions.Options;
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
        private const string BaseUrl = "https://cloud.iexapis.com/v1/";

        private readonly IHttpClientFactory httpClientFactory;
        private readonly IEXConfiguration options;

        public InvestorsExchangeStockProvider(IHttpClientFactory httpClientFactory, IOptionsSnapshot<IEXConfiguration> options)
        {
            this.httpClientFactory = httpClientFactory;
            this.options = options.Value;
        }

        public async Task<Company> GetCompanyAsync(string symbol, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(symbol))
                throw new ArgumentException("Invalid symbol", nameof(symbol));

            var url = new Flurl.Url(BaseUrl).AppendPathSegments("stock", symbol, "company").SetQueryParam("token", this.options.Token);
            var httpClient = this.httpClientFactory.CreateClient();
            var response = await httpClient.GetAsync(url, cancellationToken);
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            var companyInfo = JsonConvert.DeserializeObject<Company>(content);
            return companyInfo;
        }

        public async Task<List<SymbolDescription>> GetSymbolsAsync(CancellationToken cancellationToken)
        {
            var url = new Flurl.Url(BaseUrl).AppendPathSegment("/ref-data/symbols").SetQueryParam("token", this.options.Token);
            var httpClient = this.httpClientFactory.CreateClient();
            var response = await httpClient.GetAsync(url, cancellationToken);
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            var symbolDescriptions = JsonConvert.DeserializeObject<List<SymbolDescription>>(content);
            return symbolDescriptions;
        }

        public async Task<IDictionary<string, BatchResponse>> GetQuotesAsync(IEnumerable<string> symbols, CancellationToken cancellationToken)
        {
            if (!symbols.Any())
            {
                return new Dictionary<string, BatchResponse>();
            }

            var url = new Flurl.Url(BaseUrl)
                .AppendPathSegment("/stock/market/batch")
                .SetQueryParams(new
                {
                    types = "quote",
                    symbols = string.Join(',', symbols),
                    token = this.options.Token
                });
            var httpClient = this.httpClientFactory.CreateClient();
            var response = await httpClient.GetAsync(url, cancellationToken);
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            var quotes = JsonConvert.DeserializeObject<Dictionary<string, BatchResponse>>(content);
            return quotes;
        }

        public string GetLogo(string symbol)
        {
            return $"https://storage.googleapis.com/iex/api/logos/{symbol}.png";
        }
    }
}
