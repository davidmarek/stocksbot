using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot.Types.InlineQueryResults;

namespace StocksBot.Telegram
{
    public interface ITelegramBot
    {
        Task ReplyAsync(string inlineQueryId, IEnumerable<InlineQueryResultBase> result, CancellationToken cancellationToken);
    }
}