using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public class Portfolio : ObservableCollection<StockPosition>
    {
        public new void Add(StockPosition item)
        {
            var existingStockPosition = this.SingleOrDefault(stockPosition => stockPosition.Name.Equals(item.Name));
            if (existingStockPosition == null)
            {
                base.Add(item);
            }
            else
            {
                existingStockPosition.NumberOfShares += item.NumberOfShares;
                base.OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, item));
            }
        }
    }
}
