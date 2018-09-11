using StocksBot.QuoteRenderer;
using StocksBot.StocksProviders;
using StocksBot.StocksProviders.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using Telegram.Bot.Types.InlineQueryResults;
using TB = Telegram.Bot.Types;

namespace StocksBot.Telegram
{
    public class UpdateParser : IUpdateParser
    {
        private readonly ITelegramBot telegramBot;
        private readonly IStockProvider stockProvider;
        private readonly IQuoteRenderer quoteRenderer;
        private readonly CompanyInfoProvider companyInfoProvider;

        public UpdateParser(ITelegramBot telegramBot, IStockProvider stockProvider, IQuoteRenderer quoteRenderer, CompanyInfoProvider companyInfoProvider)
        {
            this.telegramBot = telegramBot ?? throw new ArgumentNullException(nameof(telegramBot));
            this.stockProvider = stockProvider ?? throw new ArgumentNullException(nameof(stockProvider));
            this.quoteRenderer = quoteRenderer ?? throw new ArgumentNullException(nameof(quoteRenderer));
            this.companyInfoProvider = companyInfoProvider ?? throw new ArgumentNullException(nameof(companyInfoProvider));
        }

        public async Task ProcessUpdateAsync(Update update, CancellationToken cancellationToken)
        {
            if (update.Type == TB.Enums.UpdateType.InlineQuery)
            {
                var companies = GetCompaniesFromQuery(update);
                var quotes = await this.stockProvider.GetQuotesAsync(companies.Select(c => c.Symbol), cancellationToken);
                var results = new List<InlineQueryResultBase>();
                foreach (var company in companies)
                {
                    var quote = quotes[company.Symbol].Quote;
                    var logoUrl = this.stockProvider.GetLogo(company.Symbol);
                    var result = this.quoteRenderer.Render(company, quote, logoUrl);
                    results.Add(result);
                }
                await this.telegramBot.ReplyAsync(update.InlineQuery.Id, results, cancellationToken);
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
                    companies.AddRange(this.companyInfoProvider.FindPrefix(symbol, 1));
                }
                return companies;
            }

            return this.companyInfoProvider.FindPrefix(query.ToUpperInvariant(), 5);
        }
    }
}
