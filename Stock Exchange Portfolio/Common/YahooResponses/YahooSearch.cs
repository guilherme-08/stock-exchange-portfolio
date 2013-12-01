using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.YahooResponses
{
    public class YahooSearch
    {
        public YahooSearchResultSet ResultSet = new YahooSearchResultSet();
    }

    public class YahooSearchResultSet
    {
        public string Query;

        public YahooSearchResult[] Result = new YahooSearchResult[] {};
    }

    public class YahooSearchResult
    {
        public string symbol { get; set; }

        public string name { get; set; }

        public string type { get; set; }

        public string exch { get; set; }

        public string exchDisp { get; set; }

        public string typeDisp { get; set; }
    }
}
