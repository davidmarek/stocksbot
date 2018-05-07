using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot.Types;

using TB = Telegram.Bot.Types;

namespace StocksBot.Telegram
{
    public class UpdateParser
    {
        public void ProcessUpdate(Update update)
        {
            if (update.Type == TB.Enums.UpdateType.InlineQuery)
            {
                var query = update.InlineQuery.Query;
            }
        }
    }
}
