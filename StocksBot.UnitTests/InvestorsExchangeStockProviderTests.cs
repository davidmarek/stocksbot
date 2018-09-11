using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using StocksBot.StocksProviders;
using Xunit;

namespace StocksBot.UnitTests
{
    public class InvestorsExchangeStockProviderTests
    {
        [Fact]
        public async Task GetCompanyAsync_404_Throws()
        {
            var response = new HttpResponseMessage(HttpStatusCode.NotFound);
            var httpClientFactoryMock = new Mock<IHttpClientFactory>();
            httpClientFactoryMock.Setup(x => x.CreateClient(It.IsAny<string>())).Returns(new HttpClient(new MockMessageHandler(response)));

            var iexStockProvider = new InvestorsExchangeStockProvider(httpClientFactoryMock.Object);

            await Assert.ThrowsAsync<HttpRequestException>(async () => await iexStockProvider.GetCompanyAsync("MSFT", CancellationToken.None));
        }
    }
}
