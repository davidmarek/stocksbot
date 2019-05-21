using Microsoft.Extensions.Options;
using System.Net.Http;
using Telegram.Bot;

namespace StocksBot.Telegram
{
    public class TelegramBotClientFactory : ITelegramBotClientFactory
    {
        private readonly IHttpClientFactory httpClientFactory;
        private readonly TelegramConfiguration configuration;

        public TelegramBotClientFactory(IHttpClientFactory httpClientFactory, IOptionsSnapshot<TelegramConfiguration> options)
        {
            this.configuration = options.Value;
            this.httpClientFactory = httpClientFactory;
        }

        public ITelegramBotClient Create()
        {
            return new TelegramBotClient(this.configuration.Token, this.httpClientFactory.CreateClient());
        }
    }
}
