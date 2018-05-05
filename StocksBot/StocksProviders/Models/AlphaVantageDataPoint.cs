using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StocksBot.StocksProviders.Models
{
    public class AlphaVantageDataPoint
    {
        [JsonProperty("1. open")]
        public decimal Open { get; set; }

        [JsonProperty("2. high")]
        public decimal High { get; set; }

        [JsonProperty("3. low")]
        public decimal Low { get; set; }

        [JsonProperty("4. close")]
        public decimal Close { get; set; }

        [JsonProperty("5. volume")]
        public long Volume { get; set; }

        public DataPoint ToDataPoint()
        {
            return new DataPoint
            {
                Close = this.Close,
                High = this.High,
                Low = this.Low,
                Open = this.Open,
                Volume = this.Volume
            };
        }
    }
}
