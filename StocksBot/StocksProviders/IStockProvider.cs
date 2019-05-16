using StocksBot.StocksProviders.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace StocksBot.StocksProviders
{
    public interface IStockProvider
    {
        Task<Company> GetCompanyAsync(string symbol, CancellationToken cancellationToken);
        Task<List<SymbolDescription>> GetSymbolsAsync(CancellationToken cancellationToken);
        Task<IDictionary<string, BatchResponse>> GetQuotesAsync(IEnumerable<string> symbols, CancellationToken cancellationToken);
        string GetLogo(string symbol);
    }
}
