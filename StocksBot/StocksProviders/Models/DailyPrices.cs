using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StocksBot.StocksProviders.Models
{
    public class DailyPrices
    {
        public Dictionary<DateTime, DataPoint> DataPoints { get; set; }
    }
}
