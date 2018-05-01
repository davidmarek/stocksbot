using Newtonsoft.Json;
using System;
using System.Linq;
using Xunit;
using StocksBot.StocksProviders.Models;
using System.Collections.Generic;

namespace StocksBot.UnitTests
{
    public class AlphaVantageModelTests
    {
        private const string TimeSeriesDailyResponse = @"{
            ""Meta Data"": {
                ""1. Information"": ""Daily Prices (open, high, low, close) and Volumes"",
                ""2. Symbol"": ""MSFT"",
                ""3. Last Refreshed"": ""2018-05-01 15:32:55"",
                ""4. Output Size"": ""Compact"",
                ""5. Time Zone"": ""US/Eastern""
            },
            ""Time Series (Daily)"": {
                ""2018-05-01"": {
                    ""1. open"": ""93.2100"",
                    ""2. high"": ""95.2900"",
                    ""3. low"": ""92.7900"",
                    ""4. close"": ""95.0000"",
                    ""5. volume"": ""31057858""
                },
                ""2018-04-30"": {
                    ""1. open"": ""96.3300"",
                    ""2. high"": ""96.3964"",
                    ""3. low"": ""93.1500"",
                    ""4. close"": ""93.5200"",
                    ""5. volume"": ""41092435""
                }
            }
        }";

        [Fact]
        public void AlphaVantageTimeSeriesDailyResponse_Parsed_InformationIsCorrect()
        {
            var parsedData = JsonConvert.DeserializeObject<AlphaVantageTimeSeriesDailyResponse>(TimeSeriesDailyResponse);
            Assert.Equal("Daily Prices (open, high, low, close) and Volumes", parsedData.Metadata.Information);
        }

        [Fact]
        public void AlphaVantageTimeSeriesDailyResponse_Parsed_SymbolIsCorrect()
        {
            var parsedData = JsonConvert.DeserializeObject<AlphaVantageTimeSeriesDailyResponse>(TimeSeriesDailyResponse);
            Assert.Equal("MSFT", parsedData.Metadata.Symbol);
        }

        [Fact]
        public void AlphaVantageTimeSeriesDailyResponse_Parsed_LastRefreshedIsCorrect()
        {
            var parsedData = JsonConvert.DeserializeObject<AlphaVantageTimeSeriesDailyResponse>(TimeSeriesDailyResponse);
            Assert.Equal(new DateTime(2018, 5, 1, 15, 32, 55), parsedData.Metadata.LastRefreshed);
        }

        [Fact]
        public void AlphaVantageTimeSeriesDailyResponse_Parsed_OutputSizedIsCorrect()
        {
            var parsedData = JsonConvert.DeserializeObject<AlphaVantageTimeSeriesDailyResponse>(TimeSeriesDailyResponse);
            Assert.Equal("Compact", parsedData.Metadata.OutputSize);
        }

        [Fact]
        public void AlphaVantageTimeSeriesDailyResponse_Parsed_TimeZoneIsCorrect()
        {
            var parsedData = JsonConvert.DeserializeObject<AlphaVantageTimeSeriesDailyResponse>(TimeSeriesDailyResponse);
            Assert.Equal("US/Eastern", parsedData.Metadata.TimeZone);
        }

        [Fact]
        public void AlphaVantageTimeSeriesDailyResponse_Parsed_DataKeysAreCorrect()
        {
            var parsedData = JsonConvert.DeserializeObject<AlphaVantageTimeSeriesDailyResponse>(TimeSeriesDailyResponse);
            Assert.Equal(parsedData.Data.Keys.ToHashSet(), new HashSet<DateTime> {
                new DateTime(2018, 5, 1),
                new DateTime(2018, 4, 30)
            });
        }
    }
}
