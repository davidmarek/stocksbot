using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using StocksBot.StocksProviders;
using StocksBot.StocksProviders.Models;

namespace StocksBot.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : Controller
    {
        private readonly IStockProvider stockProvider;
        private readonly CompanyInfoProvider companyInfoProvider;
        private readonly IDistributedCache cache;

        public ValuesController(IStockProvider stockProvider, CompanyInfoProvider companyInfoProvider, IDistributedCache cache)
        {
            this.stockProvider = stockProvider ?? throw new System.ArgumentNullException(nameof(stockProvider));
            this.companyInfoProvider = companyInfoProvider ?? throw new System.ArgumentNullException(nameof(companyInfoProvider));
            this.cache = cache ?? throw new System.ArgumentNullException(nameof(cache));
        }
        
        [HttpGet("{id}")]
        public async Task<IActionResult> GetAsync([StringLength(10)] string id, CancellationToken cancellationToken)
        {
            if (!this.ModelState.IsValid)
            {
                return this.BadRequest();
            }

            var normalizedId = id.ToUpper();
            var company = this.companyInfoProvider.FindPrefix(normalizedId);
            await this.cache.SetStringAsync(normalizedId, JsonConvert.SerializeObject(company), cancellationToken);
            return this.Ok(company);
        }

        [HttpGet("{id}/cached")]
        public async Task<IActionResult> GetCachedAsync([StringLength(10)] string id, CancellationToken cancellationToken)
        {
            if (!this.ModelState.IsValid)
            {
                return this.BadRequest();
            }

            var normalizedId = id.ToUpper();
            var serializedCompany = await this.cache.GetStringAsync(normalizedId, cancellationToken);
            var company = JsonConvert.DeserializeObject<List<SymbolDescription>>(serializedCompany);
            return this.Ok(company);
        }
    }
}
