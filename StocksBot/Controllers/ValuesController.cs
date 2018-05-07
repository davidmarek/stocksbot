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

        public ValuesController(IStockProvider stockProvider)
        {
            this.stockProvider = stockProvider;
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
        public ActionResult<string> Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
