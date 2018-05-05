using StocksBot.StocksProviders.Models;
using System.Threading;
using System.Threading.Tasks;

namespace StocksBot.StocksProviders
{
    public interface IStockProvider
    {
        Task<DailyPrices> GetDailyPrices(string symbol, CancellationToken cancellationToken);
    }
}
