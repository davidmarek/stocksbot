using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace StocksBot.StocksProviders.Models
{
    public class AlphaVantageTimeSeriesDailyResponse
    {
        [JsonProperty("Meta Data")]
        public AlphaVantageTimeSeriesDailyMetadata Metadata { get; set; }

        [JsonProperty("Time Series (Daily)")]
        public Dictionary<DateTime, AlphaVantageDataPoint> Data { get; set; }

        public DailyPrices ToDailyPrices()
        {
            return new DailyPrices
            {
                DataPoints = new Dictionary<DateTime, DataPoint>(
                    this.Data.Select(
                        dataPointPair => new KeyValuePair<DateTime, DataPoint>(
                            dataPointPair.Key, 
                            dataPointPair.Value.ToDataPoint())))
            };
        }
    }
}
