using StocksBot.StocksProviders.Models;
using Telegram.Bot.Types.InlineQueryResults;

namespace StocksBot.QuoteRenderer
{
    public interface IQuoteRenderer
    {
        InlineQueryResultBase Render(SymbolDescription company, Quote quote, string logoUrl);
    }
}