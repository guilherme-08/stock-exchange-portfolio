using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Stock_Exchange_Portfolio.ViewModels
{
    public class PortfolioCategory : Portfolio, IList<StockPosition>
    {
        public string Name { get; set; }
    }
}
