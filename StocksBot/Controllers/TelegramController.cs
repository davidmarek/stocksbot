using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using StocksBot.Telegram;
using Telegram.Bot.Types;

namespace StocksBot.Controllers
{
    [Produces("application/json")]
    public class TelegramController : Controller
    {
        private readonly ITelegramBot telegramBot;

        public TelegramController(ITelegramBot telegramBot)
        {
            this.telegramBot = telegramBot;
        }

        [HttpPost]
        [Route("api/telegram/update")]
        public ActionResult Update([FromBody] Update update, CancellationToken cancellationToken)
        {
            return this.Ok();
        }

        [Route("api/telegram/register")]
        public async Task<ActionResult> RegisterWebhook(CancellationToken cancellationToken)
        {
            await this.telegramBot.RegisterWebhookAsync(cancellationToken);
            return this.Ok();
        }
    }
}