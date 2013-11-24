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
        public Portfolio(IEnumerable<StockPosition> collection)
        {
            foreach(var stockPostion in collection)
            {
                this.Add(stockPostion);
            }
        }

        public Portfolio()
        {
        }

        public new void Add(StockPosition item)
        {
            if (item == null)
            {
                base.Add(null);
                return;
            }
            var existingStockPosition = GetStockPosition(item.Name);
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
        public new void Remove(StockPosition item)
        {
            var existingStockPosition = GetStockPosition(item.Name);
            if (existingStockPosition == null)
            {
                base.Remove(item);
            }
            else
            {
                if (existingStockPosition.NumberOfShares == item.NumberOfShares)
                {
                    base.Remove(existingStockPosition);
                    return;
                }
                existingStockPosition.NumberOfShares -= item.NumberOfShares;
                base.OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, item));
            }
        }

        public StockPosition GetStockPosition(string shortName)
        {
            return this.SingleOrDefault(stockPosition => stockPosition.Name.Equals(shortName));
        }
    }
}
