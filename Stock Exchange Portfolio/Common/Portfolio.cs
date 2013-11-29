using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public class Portfolio : ObservableCollection<StockPosition>, INotifyPropertyChanged
    {
        public delegate void NumberOfSharesChangedHandler(uint numberOfShares);

        public Portfolio(IEnumerable<StockPosition> collection)
        {
            foreach(var stockPostion in collection)
            {
                this.Add(stockPostion);
            }
            base.PropertyChanged += OnPropertyChanged;
        }

        public Portfolio()
        {
            base.PropertyChanged += OnPropertyChanged;
        }

        public event PropertyChangedEventHandler PortfolioPropertyChanged;

        public event NumberOfSharesChangedHandler NumberOfSharesChanged;

        private void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (PortfolioPropertyChanged != null && string.Equals(e.PropertyName, "NumberOfShares"))
            {
                NumberOfSharesChanged.Invoke(((StockPosition)sender).NumberOfShares);
            }
            base.OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, sender));
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
                item.PropertyChanged += OnPropertyChanged;
                base.Add(item);
                if (NumberOfSharesChanged != null)
                {
                    NumberOfSharesChanged.Invoke(item.NumberOfShares);
                }
            }
            else
            {
                existingStockPosition.NumberOfShares += item.NumberOfShares;
                if (NumberOfSharesChanged != null)
                {
                    NumberOfSharesChanged.Invoke(item.NumberOfShares);
                }
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
                    existingStockPosition.PropertyChanged -= OnPropertyChanged;
                    base.Remove(existingStockPosition);
                    if (NumberOfSharesChanged != null)
                    {
                        NumberOfSharesChanged.Invoke(existingStockPosition.NumberOfShares);
                    }
                    return;
                }
                existingStockPosition.NumberOfShares -= item.NumberOfShares;
                if (NumberOfSharesChanged != null)
                {
                    NumberOfSharesChanged.Invoke(item.NumberOfShares);
                }
                base.OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, item));
            }
        }

        public StockPosition GetStockPosition(string shortName)
        {
            return this.SingleOrDefault(stockPosition => stockPosition.Name.Equals(shortName));
        }
    }
}
