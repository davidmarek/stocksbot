using Newtonsoft.Json;

namespace StocksBot.StocksProviders.Models
{
    public class AlphaVantageTimeSeriesDailyResponse
    {
        [JsonProperty("Meta Data")]
        public AlphaVantageTimeSeriesDailyMetadata Metadata { get; set; }
    }
}
