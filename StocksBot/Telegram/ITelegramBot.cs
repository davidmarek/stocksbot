using System.Threading;
using System.Threading.Tasks;

namespace StocksBot.Telegram
{
    public interface ITelegramBot
    {
        Task RegisterWebhookAsync(CancellationToken cancellationToken);
    }
}