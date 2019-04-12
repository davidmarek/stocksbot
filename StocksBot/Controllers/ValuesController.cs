using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
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

        public ValuesController(IStockProvider stockProvider, CompanyInfoProvider companyInfoProvider)
        {
            this.stockProvider = stockProvider;
            this.companyInfoProvider = companyInfoProvider;
        }

        [HttpGet]
        public async Task<ActionResult<Company>> Get(CancellationToken cancellationToken)
        {
            var company = await this.stockProvider.GetCompanyAsync("MSFT", cancellationToken);

            return company;
        }

        [HttpGet("{id}")]
        public IActionResult Get([StringLength(10)] string id)
        {
            if (!this.ModelState.IsValid)
            {
                return this.BadRequest();
            }

            var normalizedId = id.ToUpper();
            return this.Ok(this.companyInfoProvider.FindPrefix(normalizedId));
        }
    }
}
