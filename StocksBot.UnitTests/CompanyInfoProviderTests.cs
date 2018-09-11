using StocksBot.StocksProviders;
using StocksBot.StocksProviders.Models;
using System;
using System.Collections.Generic;
using Xunit;

namespace StocksBot.UnitTests
{
    public class CompanyInfoProviderTests
    {
        private readonly List<SymbolDescription> testData = new List<SymbolDescription>
        {
            new SymbolDescription { Symbol = "A" },
            new SymbolDescription { Symbol = "AA" },
            new SymbolDescription { Symbol = "AAA" },
            new SymbolDescription { Symbol = "AAZ" },
            new SymbolDescription { Symbol = "AB" },
            new SymbolDescription { Symbol = "B" }
        };

        [Fact]
        public void Constructor_SymbolDescriptionsIsNull_Throws()
        {
            Assert.Throws<ArgumentNullException>(() => new CompanyInfoProvider(null));
        }

        [Fact]
        public void FindPrefix_NoMatchingSymbols_ReturnsEmptyList()
        {
            var companyInfoProvider = new CompanyInfoProvider(this.testData);
            var result = companyInfoProvider.FindPrefix("Z");
            Assert.Empty(result);
        }

        [Fact]
        public void FindPrefix_OnlyOneSymbolMatchesExactly_ReturnsMatchedSymbol()
        {
            var companyInfoProvider = new CompanyInfoProvider(this.testData);
            var result = companyInfoProvider.FindPrefix("B");
            var expectedResult = new List<SymbolDescription> { new SymbolDescription { Symbol = "B" } };
            Assert.Equal(expectedResult, result);
        }

        [Fact]
        public void FindPrefix_EmptyPrefix_ReturnsAll()
        {
            var companyInfoProvider = new CompanyInfoProvider(this.testData);
            var result = companyInfoProvider.FindPrefix("");
            Assert.Equal(this.testData, result);
        }

        [Fact]
        public void FindPrefix_PrefixMatchingMultipleSymbols_ReturnsAllMatchingSymbols()
        {
            var companyInfoProvider = new CompanyInfoProvider(this.testData);
            var result = companyInfoProvider.FindPrefix("AA");
            var expectedResult = new List<SymbolDescription>
            {
                new SymbolDescription{Symbol = "AA"},
                new SymbolDescription{Symbol = "AAA"},
                new SymbolDescription{Symbol = "AAZ"},
            };
            Assert.Equal(expectedResult, result);
        }

        [Fact]
        public void FindPrefix_NonzeroOffset_ReturnsAllMatchingSymbolsAfterOffset()
        {
            var companyInfoProvider = new CompanyInfoProvider(this.testData);
            var result = companyInfoProvider.FindPrefix("AA", offset: 2);
            var expectedResult = new List<SymbolDescription>
            {
                new SymbolDescription{Symbol = "AAZ"},
            };
            Assert.Equal(expectedResult, result);
        }

        [Fact]
        public void FindPrefix_NonzeroCount_ReturnsCountOfMatchingSymbols()
        {
            var companyInfoProvider = new CompanyInfoProvider(this.testData);
            var result = companyInfoProvider.FindPrefix("AA", 2);
            var expectedResult = new List<SymbolDescription>
            {
                new SymbolDescription{Symbol = "AA"},
                new SymbolDescription{Symbol = "AAA"},
            };
            Assert.Equal(expectedResult, result);
        }

        [Fact]
        public void FindPrefix_NonzeroCountAndOffset_ReturnsAllMatchingSymbols()
        {
            var companyInfoProvider = new CompanyInfoProvider(this.testData);
            var result = companyInfoProvider.FindPrefix("AA", 1, 1);
            var expectedResult = new List<SymbolDescription>
            {
                new SymbolDescription{Symbol = "AAA"},
            };
            Assert.Equal(expectedResult, result);
        }

        [Fact]
        public void FindPrefix_PrefixIsNull_Throws()
        {
            var companyInfoProvider = new CompanyInfoProvider(this.testData);
            Assert.Throws<ArgumentNullException>(() => companyInfoProvider.FindPrefix(null));
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(0)]
        [InlineData(51)]
        public void FindPrefix_CountIsOutOfRange_Throws(int count)
        {
            var companyInfoProvider = new CompanyInfoProvider(this.testData);
            Assert.Throws<ArgumentOutOfRangeException>(() => companyInfoProvider.FindPrefix("A", count));
        }

        [Fact]
        public void FindPrefix_OffsetIsOutOfRange_Throws()
        {
            var companyInfoProvider = new CompanyInfoProvider(this.testData);
            Assert.Throws<ArgumentOutOfRangeException>(() => companyInfoProvider.FindPrefix("A", offset: -1));
        }
    }
}
