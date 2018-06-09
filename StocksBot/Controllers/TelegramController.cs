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
            this.logger = logger;
            this.configuration = options.Value;
        }

        [HttpPost]
        [Route("api/telegram/{secret}/update")]
        public ActionResult Update(string secret, [FromBody] Update update, CancellationToken cancellationToken)
        {
            if (secret != this.configuration.WebhookSecret)
            {
                this.logger.LogWarning("Invalid secret: {0}, expected {1}", secret, this.configuration.WebhookSecret);
                return this.BadRequest();
            }

            this.updateParser.ProcessUpdateAsync(update, cancellationToken);
            return this.Ok();
        }
    }
}