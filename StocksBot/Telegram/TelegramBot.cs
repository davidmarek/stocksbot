using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot.Types.InlineQueryResults;

namespace StocksBot.Telegram
{
    public class TelegramBot : ITelegramBot
    {
        private readonly ITelegramBotClientFactory telegramBotClientFactory;

        public TelegramBot(ITelegramBotClientFactory telegramBotClientFactory)
        {
            this.telegramBotClientFactory = telegramBotClientFactory;
        }

        public async Task ReplyAsync(string inlineQueryId, IEnumerable<InlineQueryResultBase> result, CancellationToken cancellationToken)
        {
            var telegramBotClient = this.telegramBotClientFactory.Create();
            await telegramBotClient.AnswerInlineQueryAsync(inlineQueryId, result, cacheTime: 60, cancellationToken: cancellationToken);
        }
    }
}
