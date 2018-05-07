using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;

namespace StocksBot.Telegram
{
    public class TelegramBot : ITelegramBot
    {
        private readonly ITelegramBotClientFactory telegramBotClientFactory;
        private readonly TelegramConfiguration configuration;

        public TelegramBot(ITelegramBotClientFactory telegramBotClientFactory, IOptionsSnapshot<TelegramConfiguration> options)
        {
            this.telegramBotClientFactory = telegramBotClientFactory;
            this.configuration = options.Value;
        }

        public async Task RegisterWebhookAsync(CancellationToken cancellationToken)
        {
            var telegramBotClient = this.telegramBotClientFactory.Create();
            await telegramBotClient.SetWebhookAsync(this.configuration.WebhookUrl, cancellationToken: cancellationToken);
        }
    }
}
