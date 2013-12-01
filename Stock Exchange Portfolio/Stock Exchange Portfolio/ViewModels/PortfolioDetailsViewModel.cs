using Common;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
                        Opacity = Math.Max(1.0 - (0.2 * PortfolioDetails.Count), 0)
                    });
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
