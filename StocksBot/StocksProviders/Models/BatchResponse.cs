using Newtonsoft.Json;
using System.Collections.Generic;

namespace StocksBot.StocksProviders.Models
{
    public class BatchResponse
    {
        [JsonProperty("quote")]
        public Quote Quote { get; set; }
    }
}
