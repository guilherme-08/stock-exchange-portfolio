using Common;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stock_Exchange_Portfolio.ViewModels
{
    public class PortfolioDetailsViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<PortfolioDetailsModel> PortfolioDetails { get; set; }

        public PortfolioDetailsViewModel()
        {
            PortfolioDetails = new ObservableCollection<PortfolioDetailsModel>();
            foreach (var stockPosition in Settings.Portfolio)
            {
                PortfolioDetails.Add(
                    new PortfolioDetailsModel() {
                        StockName = stockPosition.Name,
                        StocksCount = stockPosition.NumberOfShares,
                        StocksValueNow = stockPosition.StockValueNow,
                        Opacity = Math.Max(1.0 - (0.2 * PortfolioDetails.Count), 0)
                    });
            }
        }

        public String PortfolioCurrentValue
        {
            get
            {
                if (Settings.Portfolio == null || Settings.Portfolio.Count == 0)
                {
                    return "0";
                }
                return Settings.Portfolio.Sum(_ => _.StockValueNow).ToString(".##", CultureInfo.InvariantCulture);
            }
        }

        public String PortfolioCurrentSharesCount
        {
            get
            {
                if (Settings.Portfolio == null || Settings.Portfolio.Count == 0)
                {
                    return "0";
                }
                return Settings.Portfolio.Sum(_ => _.NumberOfShares).ToString();
            }
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
    }
}
