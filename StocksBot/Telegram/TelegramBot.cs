using Flurl;
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
        private readonly ITelegramBotClientFactory telegramBotClientFactory;
        private readonly TelegramConfiguration configuration;

        public TelegramBot(ITelegramBotClientFactory telegramBotClientFactory, IOptionsSnapshot<TelegramConfiguration> options)
        {
            this.telegramBotClientFactory = telegramBotClientFactory;
            this.configuration = options.Value;
        }

        public async Task ReplyAsync(string inlineQueryId, IEnumerable<InlineQueryResultBase> result, CancellationToken cancellationToken)
        {
            var telegramBotClient = this.telegramBotClientFactory.Create();
            await telegramBotClient.AnswerInlineQueryAsync(inlineQueryId, result, cacheTime: 60, cancellationToken: cancellationToken);
        }
    }
}
