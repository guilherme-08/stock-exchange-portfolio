using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.YahooResponses
{
    public class YahooTableEntry
    {
        public String DateString { get; set; }

        public Double Open { get; set; }

        public Double High { get; set; }

        public Double Low { get; set; }

        public Double Close { get; set; }

        public UInt32 Volume { get; set; }

        public Double AdjustedClose { get; set; }
    }
}
