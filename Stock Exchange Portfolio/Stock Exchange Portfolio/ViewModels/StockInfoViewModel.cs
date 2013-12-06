using Common;
using Common.YahooResponses;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stock_Exchange_Portfolio.ViewModels
{
    public class StockInfoViewModel : INotifyPropertyChanged
    {
        private YahooQuote yahooQuote;

        public YahooQuote YahooQuote
        {
            get
            {
                return yahooQuote;
            }
            set
            {
                if (value != yahooQuote)
                {
                    yahooQuote = value;
                    NotifyPropertyChanged("YahooQuote");
                }
            }
        }

        public uint Stocks
        {
            get
            {
                var stockPosition = App.ViewModel.Portfolio.GetStockPosition(YahooQuote.ShortName);
                return stockPosition == null ? 0 : stockPosition.NumberOfShares;
            }
        }
        public ObservableCollection<YahooTable> YahooTables { get; private set; }

        public void ResetYahooTables()
        {
            YahooTables = new ObservableCollection<YahooTable>() { null, null, null, null, null, null, null };
        }

        private YahooTable yahooTable;

        public YahooTable YahooTable
        {
            get
            {
                return yahooTable;
            }
            set
            {
                if (value != yahooTable)
                {
                    yahooTable = value;
                    NotifyPropertyChanged("YahooTable");
                }
            }
        }

        public StockInfoViewModel()
        {
            ResetYahooTables();
            yahooQuote = new YahooQuote();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(String propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (null != handler)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        internal void StocksChanged()
        {
            NotifyPropertyChanged("Stocks");
        }
    }
}
