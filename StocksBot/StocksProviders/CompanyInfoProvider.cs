using StocksBot.StocksProviders.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StocksBot.StocksProviders
{
    public class CompanyInfoProvider
    {
        private List<SymbolDescription> _symbolDescriptions;

        public CompanyInfoProvider(List<SymbolDescription> symbolDescriptions)
        {
            _symbolDescriptions = symbolDescriptions ?? throw new ArgumentNullException(nameof(symbolDescriptions));
            _symbolDescriptions.Sort();
        }

        public void UpdateSymbolDescriptions(List<SymbolDescription> symbolDescriptions)
        {
            if (symbolDescriptions == null)
                throw new ArgumentNullException(nameof(symbolDescriptions));

            symbolDescriptions.Sort();
            _symbolDescriptions = symbolDescriptions;
        }

        public List<SymbolDescription> FindPrefix(string symbolPrefix, int count = 50, int offset = 0)
        {
            if (symbolPrefix == null)
                throw new ArgumentNullException(nameof(symbolPrefix));
            if (count <= 0 || count > 50)
                throw new ArgumentOutOfRangeException(nameof(count));
            if (offset < 0)
                throw new ArgumentOutOfRangeException(nameof(offset));

            var index = _symbolDescriptions.BinarySearch(new SymbolDescription { Symbol = symbolPrefix });
            if (index < 0)
            {
                index = ~index;
                if (index == _symbolDescriptions.Count)
                    return new List<SymbolDescription>();
            }

            var n = 0;
            while (index + offset + n < _symbolDescriptions.Count 
                && n < count
                && _symbolDescriptions[index + offset + n].Symbol.StartsWith(symbolPrefix))
            {
                n++;
            }

            return _symbolDescriptions.GetRange(index + offset, n);
        }
    }
}
