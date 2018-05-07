using Newtonsoft.Json;
using System;

namespace StocksBot.StocksProviders.Models
{
    public class SymbolDescription : IComparable<SymbolDescription>
    {
        [JsonProperty("symbol")]
        public string Symbol { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("date")]
        public DateTime Date { get; set; }

        [JsonProperty("isEnabled")]
        public bool IsEnabled { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("iexId")]
        public int IexId { get; set; }

        public int CompareTo(SymbolDescription other)
        {
            return this.Symbol.CompareTo(other.Symbol);
        }
    }
}
