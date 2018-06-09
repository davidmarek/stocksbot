using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using StocksBot.StocksProviders;
using StocksBot.Telegram;
using Telegram.Bot.Types;

namespace StocksBot.Controllers
{
    [Produces("application/json")]
    public class TelegramController : Controller
    {
        private readonly ITelegramBot telegramBot;
        private readonly IUpdateParser updateParser;
        private readonly IOptionsSnapshot<TelegramConfiguration> options;
        private readonly ILogger<TelegramController> logger;
        private readonly TelegramConfiguration configuration;

        public TelegramController(
            ITelegramBot telegramBot, 
            IUpdateParser updateParser, 
            IOptionsSnapshot<TelegramConfiguration> options,
            ILogger<TelegramController> logger)
        {
            this.telegramBot = telegramBot;
            this.updateParser = updateParser;
            this.options = options;
            this.logger = logger;
        }

        [HttpPost]
        [Route("api/telegram/{secret}/update")]
        public ActionResult Update(string secret, [FromBody] Update update, CancellationToken cancellationToken)
        {
            if (secret != this.options.Value.WebhookSecret)
            {
                this.logger.LogWarning("Invalid secret: {0}, expected {1}", secret, this.options.Value.WebhookSecret);
                return this.BadRequest();
            }

            this.updateParser.ProcessUpdateAsync(update, cancellationToken);
            return this.Ok();
        }
    }
}