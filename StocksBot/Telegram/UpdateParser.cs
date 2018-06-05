using StocksBot.StocksProviders;
using StocksBot.StocksProviders.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using Telegram.Bot.Types.InlineQueryResults;
using TB = Telegram.Bot.Types;

namespace StocksBot.Telegram
{
    public class UpdateParser : IUpdateParser
    {
        private readonly ITelegramBot _telegramBot;
        private readonly IStockProvider _stockProvider;
        private readonly CompanyInfoProvider _companyInfoProvider;

        public UpdateParser(ITelegramBot telegramBot, IStockProvider stockProvider, CompanyInfoProvider companyInfoProvider)
        {
            _telegramBot = telegramBot ?? throw new ArgumentNullException(nameof(telegramBot));
            _stockProvider = stockProvider ?? throw new ArgumentNullException(nameof(stockProvider));
            _companyInfoProvider = companyInfoProvider ?? throw new ArgumentNullException(nameof(companyInfoProvider));
        }

        public async Task ProcessUpdateAsync(Update update, CancellationToken cancellationToken)
        {
            if (update.Type == TB.Enums.UpdateType.InlineQuery)
            {
                var companies = GetCompaniesFromQuery(update);
                var quotes = await _stockProvider.GetQuotesAsync(companies.Select(c => c.Symbol), cancellationToken);
                var results = new List<InlineQueryResultArticle>();
                foreach (var company in companies)
                {
                    var quote = quotes[company.Symbol].Quote;
                    var logoUrl = _stockProvider.GetLogo(company.Symbol);
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
                    results.Add(result);
                }
                await _telegramBot.ReplyAsync(update.InlineQuery.Id, results, cancellationToken);
            }
        }

        private List<SymbolDescription> GetCompaniesFromQuery(Update update)
        {
            var query = update.InlineQuery.Query;

            if (string.IsNullOrWhiteSpace(query))
            {
                var companies = new List<SymbolDescription>();
                foreach (var symbol in new[] { "GOOG", "MSFT", "FB", "AMZN" })
                {
                    companies.AddRange(_companyInfoProvider.FindPrefix(symbol, 1));
                }
                return companies;
            }

            return _companyInfoProvider.FindPrefix(query.ToUpperInvariant(), 5);
        }
    }
}
