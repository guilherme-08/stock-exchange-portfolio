using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.YahooResponses
{
    public class YahooQuote
    {
        public String Name { get; set; }

        public String ShortName { get; set; }

        public Double Value { get; set; }

        public String DateString { get; set; }

        public DateTime Date
        {
            get
            {
                return DateTime.ParseExact(DateString, "M/d/yyyy", CultureInfo.InvariantCulture);
            }
        }

        public String Time { get; set; }

        public UInt32 Volume { get; set; }
    }
}
