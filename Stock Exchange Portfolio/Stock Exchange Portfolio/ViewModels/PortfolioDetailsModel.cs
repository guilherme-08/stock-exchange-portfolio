using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Stock_Exchange_Portfolio.ViewModels
{
    public class PortfolioDetailsModel : INotifyPropertyChanged
    {
        private string stockName;

        public string StockName
        {
            get
            {
                return stockName;
            }
            set
            {
                if (value != stockName)
                {
                    stockName = value;
                    NotifyPropertyChanged("StockName");
                }
            }
        }

        private double stocksCount;

        public double StocksCount
        {
            get
            {
                return stocksCount;
            }
            set
            {
                if (value != stocksCount)
                {
                    stocksCount = value;
                    NotifyPropertyChanged("StocksCount");
                }
            }
        }

        public double Opacity { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(String propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (null != handler)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
