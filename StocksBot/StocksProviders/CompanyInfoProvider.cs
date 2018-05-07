using StocksBot.StocksProviders.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StocksBot.StocksProviders
{
    public class CompanyInfoProvider
    {
        private readonly List<SymbolDescription> symbolDescriptions;

        public CompanyInfoProvider(List<SymbolDescription> symbolDescriptions)
        {
            this.symbolDescriptions = symbolDescriptions ?? throw new ArgumentNullException(nameof(symbolDescriptions));
            this.symbolDescriptions.Sort();
        }

        public List<SymbolDescription> FindPrefix(string symbolPrefix, int count = 50, int offset = 0)
        {
            if (symbolPrefix == null)
                throw new ArgumentNullException(nameof(symbolPrefix));
            if (count <= 0 || count > 50)
                throw new ArgumentOutOfRangeException(nameof(count));
            if (offset < 0)
                throw new ArgumentOutOfRangeException(nameof(offset));

            var index = this.symbolDescriptions.BinarySearch(new SymbolDescription { Symbol = symbolPrefix });
            if (index < 0)
            {
                index = ~index;
                if (index == this.symbolDescriptions.Count)
                    return new List<SymbolDescription>();
            }

            var n = 0;
            while (index + offset + n < this.symbolDescriptions.Count 
                && n < count
                && this.symbolDescriptions[index + offset + n].Symbol.StartsWith(symbolPrefix))
            {
                n++;
            }

            return this.symbolDescriptions.GetRange(index + offset, n);
        }
    }
}
