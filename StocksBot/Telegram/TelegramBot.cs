using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types.InlineQueryResults;

namespace StocksBot.Telegram
{
    public class TelegramBot : ITelegramBot
    {
        private readonly ITelegramBotClientFactory _telegramBotClientFactory;
        private readonly TelegramConfiguration _configuration;

        public TelegramBot(ITelegramBotClientFactory telegramBotClientFactory, IOptionsSnapshot<TelegramConfiguration> options)
        {
            _telegramBotClientFactory = telegramBotClientFactory;
            _configuration = options.Value;
        }

        public async Task RegisterWebhookAsync(CancellationToken cancellationToken)
        {
            var telegramBotClient = _telegramBotClientFactory.Create();
            await telegramBotClient.SetWebhookAsync(_configuration.WebhookUrl, cancellationToken: cancellationToken);
        }

        public async Task ReplyAsync(string inlineQueryId, IEnumerable<InlineQueryResultBase> result, CancellationToken cancellationToken)
        {
            var telegramBotClient = _telegramBotClientFactory.Create();
            await telegramBotClient.AnswerInlineQueryAsync(inlineQueryId, result, cacheTime: 60, cancellationToken: cancellationToken);
        }
    }
}
