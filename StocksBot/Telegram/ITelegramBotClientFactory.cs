using Telegram.Bot;

namespace StocksBot.Telegram
{
    public interface ITelegramBotClientFactory
    {
        ITelegramBotClient Create();
    }
}