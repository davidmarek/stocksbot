using Newtonsoft.Json;

namespace StocksBot.StocksProviders.Models
{
    public class Quote
    { 
        [JsonProperty("symbol")]
        public string Symbol { get; set; }

        [JsonProperty("companyName")]
        public string CompanyName { get; set; }

        [JsonProperty("primaryExchange")]
        public string PrimaryExchange { get; set; }

        [JsonProperty("sector")]
        public string Sector { get; set; }

        [JsonProperty("calculationPrice")]
        public string CalculationPrice { get; set; }

        [JsonProperty("open")]
        public decimal? Open { get; set; }

        [JsonProperty("openTime")]
        public long? OpenTime { get; set; }

        [JsonProperty("close")]
        public decimal? Close { get; set; }

        [JsonProperty("closeTime")]
        public long? CloseTime { get; set; }

        [JsonProperty("high")]
        public decimal? High { get; set; }

        [JsonProperty("low")]
        public decimal? Low { get; set; }

        [JsonProperty("latestPrice")]
        public decimal? LatestPrice { get; set; }

        [JsonProperty("latestSource")]
        public string LatestSource { get; set; }

        [JsonProperty("latestTime")]
        public string LatestTime { get; set; }

        [JsonProperty("latestUpdate")]
        public long? LatestUpdate { get; set; }

        [JsonProperty("latestVolume")]
        public int? LatestVolume { get; set; }

        [JsonProperty("iexRealtimePrice")]
        public decimal? IexRealtimePrice { get; set; }

        [JsonProperty("iexRealtimeSize")]
        public int? IexRealtimeSize { get; set; }

        [JsonProperty("iexLastUpdated")]
        public long? IexLastUpdated { get; set; }

        [JsonProperty("delayedPrice")]
        public decimal? DelayedPrice { get; set; }

        [JsonProperty("delayedPriceTime")]
        public long? DelayedPriceTime { get; set; }

        [JsonProperty("previousClose")]
        public decimal? PreviousClose { get; set; }

        [JsonProperty("change")]
        public decimal? Change { get; set; }

        [JsonProperty("changePercent")]
        public decimal? ChangePercent { get; set; }

        [JsonProperty("iexMarketPercent")]
        public decimal? IexMarketPercent { get; set; }

        [JsonProperty("iexVolume")]
        public int? IexVolume { get; set; }

        [JsonProperty("avgTotalVolume")]
        public int? AvgTotalVolume { get; set; }

        [JsonProperty("iexBidPrice")]
        public decimal? IexBidPrice { get; set; }

        [JsonProperty("iexBidSize")]
        public int? IexBidSize { get; set; }

        [JsonProperty("iexAskPrice")]
        public decimal? IexAskPrice { get; set; }

        [JsonProperty("iexAskSize")]
        public int? IexAskSize { get; set; }

        [JsonProperty("marketCap")]
        public long? MarketCap { get; set; }

        [JsonProperty("peRatio")]
        public decimal? PeRatio { get; set; }

        [JsonProperty("week52High")]
        public decimal? Week52High { get; set; }

        [JsonProperty("week52Low")]
        public decimal? Week52Low { get; set; }

        [JsonProperty("ytdChange")]
        public decimal? YtdChange { get; set; }
    }

}
