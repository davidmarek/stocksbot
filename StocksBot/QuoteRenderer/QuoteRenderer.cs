using System.Globalization;
using System.Text;
using StocksBot.StocksProviders.Models;
using Telegram.Bot.Types.InlineQueryResults;
using TB = Telegram.Bot.Types;

namespace StocksBot.QuoteRenderer
{
    public class QuoteRenderer : IQuoteRenderer
    {
        public InlineQueryResultBase Render(SymbolDescription company, Quote quote, string logoUrl)
        {
            var title = $"{company.Name} ({company.Symbol})";

            var us = CultureInfo.GetCultureInfo("en-US");
            var description = new StringBuilder();
            description.AppendLine($"*{quote.LatestPrice?.ToString("C", us)}* {(quote.Change > 0 ? "+" : "") + quote.Change?.ToString("C", us)} ({(quote.ChangePercent > 0 ? "+" : "") + quote.ChangePercent?.ToString("P")})");
            description.AppendLine($"*High:* {quote.High?.ToString("C", us)}");
            description.AppendLine($"*Low:* {quote.Low?.ToString("C", us)}");
            description.AppendLine($"*52w high:* {quote.Week52High?.ToString("C", us)}");
            description.Append($"*52w low:* {quote.Week52Low?.ToString("C", us)}");

            var textContent = $"*{title}*\n{description}";
            var testMessageContent = new InputTextMessageContent(textContent)
            {
                ParseMode = TB.Enums.ParseMode.Markdown
            };
            var result = new InlineQueryResultArticle(company.Symbol, title, testMessageContent)
            {
                Description = description.ToString(),
                ThumbUrl = logoUrl
            };
            return result;
        }
    }
}
