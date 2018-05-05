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
            var expectedKeys = new HashSet<DateTime> {
                new DateTime(2018, 5, 1),
                new DateTime(2018, 4, 30)
            };
            Assert.Equal(expectedKeys,parsedData.Data.Keys.ToHashSet());
        }

        [Theory]
        [InlineData("2018-05-01", "93.21")]
        [InlineData("2018-04-30", "96.33")]
        public void AlphaVantageTimeSeriesDailyResponse_Parsed_OpenPriceIsCorrect(string date, string value)
        {
            var key = DateTime.Parse(date);
            var expectedValue = decimal.Parse(value);
            var parsedData = JsonConvert.DeserializeObject<AlphaVantageTimeSeriesDailyResponse>(TimeSeriesDailyResponse);
            Assert.Equal(expectedValue, parsedData.Data[key].Open);
        }

        [Theory]
        [InlineData("2018-05-01", "95")]
        [InlineData("2018-04-30", "93.52")]
        public void AlphaVantageTimeSeriesDailyResponse_Parsed_ClosePriceIsCorrect(string date, string value)
        {
            var key = DateTime.Parse(date);
            var expectedValue = decimal.Parse(value);
            var parsedData = JsonConvert.DeserializeObject<AlphaVantageTimeSeriesDailyResponse>(TimeSeriesDailyResponse);
            Assert.Equal(expectedValue, parsedData.Data[key].Close);
        }

        [Theory]
        [InlineData("2018-05-01", "95.29")]
        [InlineData("2018-04-30", "96.3964")]
        public void AlphaVantageTimeSeriesDailyResponse_Parsed_HighPriceIsCorrect(string date, string value)
        {
            var key = DateTime.Parse(date);
            var expectedValue = decimal.Parse(value);
            var parsedData = JsonConvert.DeserializeObject<AlphaVantageTimeSeriesDailyResponse>(TimeSeriesDailyResponse);
            Assert.Equal(expectedValue, parsedData.Data[key].High);
        }

        [Theory]
        [InlineData("2018-05-01", "92.7900")]
        [InlineData("2018-04-30", "93.1500")]
        public void AlphaVantageTimeSeriesDailyResponse_Parsed_LowPriceIsCorrect(string date, string value)
        {
            var key = DateTime.Parse(date);
            var expectedValue = decimal.Parse(value);
            var parsedData = JsonConvert.DeserializeObject<AlphaVantageTimeSeriesDailyResponse>(TimeSeriesDailyResponse);
            Assert.Equal(expectedValue, parsedData.Data[key].Low);
        }

        [Theory]
        [InlineData("2018-05-01", 31057858)]
        [InlineData("2018-04-30", 41092435)]
        public void AlphaVantageTimeSeriesDailyResponse_Parsed_VolumeIsCorrect(string date, long expectedValue)
        {
            var key = DateTime.Parse(date);
            var parsedData = JsonConvert.DeserializeObject<AlphaVantageTimeSeriesDailyResponse>(TimeSeriesDailyResponse);
            Assert.Equal(expectedValue, parsedData.Data[key].Volume);
        }

        [Fact]
        public void AlphaVantageDataPoint_ToDataPoint_ReturnsDataPointWithCorrectOpenPrice()
        {
            var inputDataPoint = new AlphaVantageDataPoint
            {
                Open = 1.23m,
                Close = 2.34m,
                High = 3.45m,
                Low = 0.12m,
                Volume = 123456
            };
            var output = inputDataPoint.ToDataPoint();
            Assert.Equal(inputDataPoint.Open, output.Open);
        }

        [Fact]
        public void AlphaVantageDataPoint_ToDataPoint_ReturnsDataPointWithCorrectClosePrice()
        {
            var inputDataPoint = new AlphaVantageDataPoint
            {
                Open = 1.23m,
                Close = 2.34m,
                High = 3.45m,
                Low = 0.12m,
                Volume = 123456
            };
            var output = inputDataPoint.ToDataPoint();
            Assert.Equal(inputDataPoint.Open, output.Open);
            Assert.Equal(inputDataPoint.Close, output.Close);
            Assert.Equal(inputDataPoint.High, output.High);
            Assert.Equal(inputDataPoint.Low, output.Low);
            Assert.Equal(inputDataPoint.Volume, output.Volume);
        }

        [Fact]
        public void AlphaVantageDataPoint_ToDataPoint_ReturnsDataPointWithCorrectHighPrice()
        {
            var inputDataPoint = new AlphaVantageDataPoint
            {
                Open = 1.23m,
                Close = 2.34m,
                High = 3.45m,
                Low = 0.12m,
                Volume = 123456
            };
            var output = inputDataPoint.ToDataPoint();
            Assert.Equal(inputDataPoint.High, output.High);
        }

        [Fact]
        public void AlphaVantageDataPoint_ToDataPoint_ReturnsDataPointWithCorrectLowPrice()
        {
            var inputDataPoint = new AlphaVantageDataPoint
            {
                Open = 1.23m,
                Close = 2.34m,
                High = 3.45m,
                Low = 0.12m,
                Volume = 123456
            };
            var output = inputDataPoint.ToDataPoint();
            Assert.Equal(inputDataPoint.Low, output.Low);
        }

        [Fact]
        public void AlphaVantageDataPoint_ToDataPoint_ReturnsDataPointWithCorrectVolume()
        {
            var inputDataPoint = new AlphaVantageDataPoint
            {
                Open = 1.23m,
                Close = 2.34m,
                High = 3.45m,
                Low = 0.12m,
                Volume = 123456
            };
            var output = inputDataPoint.ToDataPoint();
            Assert.Equal(inputDataPoint.Volume, output.Volume);
        }
    }
}
