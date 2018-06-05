using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using StocksBot.StocksProviders;
using StocksBot.Telegram;
using Telegram.Bot.Types;

namespace StocksBot.Controllers
{
    [Produces("application/json")]
    public class TelegramController : Controller
    {
        private readonly ITelegramBot _telegramBot;
        private readonly IUpdateParser _updateParser;

        public TelegramController(ITelegramBot telegramBot, IUpdateParser updateParser)
        {
            _telegramBot = telegramBot;
            _updateParser = updateParser;
        }

        [HttpPost]
        [Route("api/telegram/update")]
        public ActionResult Update([FromBody] Update update, CancellationToken cancellationToken)
        {
            _updateParser.ProcessUpdateAsync(update, cancellationToken);
            return Ok();
        }

        [Route("api/telegram/register")]
        public async Task<ActionResult> RegisterWebhook(CancellationToken cancellationToken)
        {
            await _telegramBot.RegisterWebhookAsync(cancellationToken);
            return Ok();
        }
    }
}