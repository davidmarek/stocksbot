using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace StocksBot.StocksProviders.Models
{
    [DebuggerDisplay("Open = {Open}, Close = {Close}, High = {High}, Low = {Low}, Volume = {Volume}")]
    public class DataPoint
    {
        public decimal Open { get; set; }
        public decimal Close { get; set; }
        public decimal High { get; set; }
        public decimal Low { get; set; }
        public long Volume { get; set; }
    }
}
