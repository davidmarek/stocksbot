using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace StocksBot.UnitTests
{
    class MockMessageHandler : HttpMessageHandler
    {
        private readonly HttpResponseMessage response;

        public MockMessageHandler(HttpResponseMessage response)
        {
            this.response = response;
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            return Task.FromResult(this.response);
        }
    }
}
