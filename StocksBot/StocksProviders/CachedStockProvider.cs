using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using StocksBot.StocksProviders.Models;

namespace StocksBot.StocksProviders
{
    public class CachedStockProvider : IStockProvider
    {
        private const string GetCompanyKeyFormat = "gc:{0}";
        private const string GetQuotesKeyFormat = "gq:{0}";
        private const string GetSymbolsKey = "gs";
        private readonly IStockProvider stockProvider;
        private readonly IDistributedCache cache;
        private readonly DistributedCacheEntryOptions cacheOptions;

        public CachedStockProvider(IStockProvider stockProvider, IDistributedCache cache)
        {
            this.stockProvider = stockProvider ?? throw new ArgumentNullException(nameof(stockProvider));
            this.cache = cache ?? throw new ArgumentNullException(nameof(cache));

            this.cacheOptions = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5)
            };
        }

        public async Task<Company> GetCompanyAsync(string symbol, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(symbol))
            {
                throw new ArgumentException("Symbol is invalid", nameof(symbol));
            }

            var serializedCachedCompany = await this.cache.GetStringAsync(string.Format(GetCompanyKeyFormat, symbol), cancellationToken);
            if (serializedCachedCompany != null)
            {
                return JsonConvert.DeserializeObject<Company>(serializedCachedCompany);
            }

            var company = await this.stockProvider.GetCompanyAsync(symbol, cancellationToken);
            await this.cache.SetStringAsync(string.Format(GetCompanyKeyFormat, symbol), JsonConvert.SerializeObject(company), this.cacheOptions, cancellationToken);
            return company;
        }

        public string GetLogo(string symbol)
        {
            if (string.IsNullOrWhiteSpace(symbol))
            {
                throw new ArgumentException("Symbol is invalid", nameof(symbol));
            }

            return this.stockProvider.GetLogo(symbol);
        }

        public async Task<IDictionary<string, BatchResponse>> GetQuotesAsync(IEnumerable<string> symbols, CancellationToken cancellationToken)
        {
            if (symbols == null)
            {
                throw new ArgumentNullException(nameof(symbols));
            }

            var result = new ConcurrentDictionary<string, BatchResponse>();
            var symbolsToFetch = new ConcurrentBag<string>();
            var tasks = new List<Task>();
            foreach (var symbol in symbols)
            {
                tasks.Add(FetchFromCache(symbol));
            }
            await Task.WhenAll(tasks);

            var quotes = await this.stockProvider.GetQuotesAsync(symbolsToFetch, cancellationToken);
            tasks = new List<Task>();
            foreach (var keyValue in quotes)
            {
                tasks.Add(SaveToCache(keyValue.Key, keyValue.Value));
                result[keyValue.Key] = keyValue.Value;
            }
            await Task.WhenAll(tasks);

            return result;

            async Task FetchFromCache(string symbol)
            {
                var serializedCachedQuote = await this.cache.GetStringAsync(string.Format(GetQuotesKeyFormat, symbol), cancellationToken);
                if (serializedCachedQuote != null)
                {
                    var quote = JsonConvert.DeserializeObject<BatchResponse>(serializedCachedQuote);
                    result[symbol] = quote;
                }
                else
                {
                    symbolsToFetch.Add(symbol);
                }
            }

            async Task SaveToCache(string symbol, BatchResponse quote)
            {
                var serializedQuote = JsonConvert.SerializeObject(quote);
                await this.cache.SetStringAsync(string.Format(GetQuotesKeyFormat, symbol), serializedQuote, this.cacheOptions, cancellationToken);
            }
        }

        public async Task<List<SymbolDescription>> GetSymbolsAsync(CancellationToken cancellationToken)
        {
            var serializedSymbols = await this.cache.GetStringAsync(GetSymbolsKey, cancellationToken);
            List<SymbolDescription> symbols;
            if (serializedSymbols != null)
            {
                symbols = JsonConvert.DeserializeObject<List<SymbolDescription>>(serializedSymbols);
            }
            else
            {
                symbols = await this.stockProvider.GetSymbolsAsync(cancellationToken);
                serializedSymbols = JsonConvert.SerializeObject(symbols);
                await this.cache.SetStringAsync(GetSymbolsKey, serializedSymbols, this.cacheOptions, cancellationToken);
            }

            return symbols;
        }
    }
}
