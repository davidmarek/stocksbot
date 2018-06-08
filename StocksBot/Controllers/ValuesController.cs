using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using StocksBot.StocksProviders;
using StocksBot.StocksProviders.Models;

namespace StocksBot.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private readonly IStockProvider stockProvider;
        private readonly CompanyInfoProvider companyInfoProvider;

        public ValuesController(IStockProvider stockProvider, CompanyInfoProvider companyInfoProvider)
        {
            this.stockProvider = stockProvider;
            this.companyInfoProvider = companyInfoProvider;
        }

        // GET api/values
        [HttpGet]
        public async Task<ActionResult<Company>> Get(CancellationToken cancellationToken)
        {
            var company = await this.stockProvider.GetCompanyAsync("MSFT", cancellationToken);

            return company;
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<List<SymbolDescription>> Get(string id)
        {
            return this.companyInfoProvider.FindPrefix(id);
        }
    }
}
