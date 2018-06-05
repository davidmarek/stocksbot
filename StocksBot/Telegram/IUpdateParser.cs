using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace StocksBot.Telegram
{
    public interface IUpdateParser
    {
        Task ProcessUpdateAsync(Update update, CancellationToken cancellationToken);
    }
}