using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace StocksBot.StocksProviders.Models
{
    public class AlphaVantageTimeSeriesDailyResponse
    {
        [JsonProperty("Meta Data")]
        public AlphaVantageTimeSeriesDailyMetadata Metadata { get; set; }

        [JsonProperty("Time Series (Daily)")]
        public Dictionary<DateTime, object> Data { get; set; }
    }
}
