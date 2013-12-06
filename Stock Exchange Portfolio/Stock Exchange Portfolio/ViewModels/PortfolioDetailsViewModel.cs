using Common;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Stock_Exchange_Portfolio.ViewModels
{
    public class PortfolioDetailsViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<PortfolioDetailsModel> PortfolioDetails { get; set; }

        public PortfolioDetailsViewModel()
        {
            PortfolioDetails = new ObservableCollection<PortfolioDetailsModel>();
        }

        public void Reset()
        {
            NotifyPropertyChanged("GraphVisibility");
            NotifyPropertyChanged("PortfolioCurrentValue");
            NotifyPropertyChanged("PortfolioCurrentSharesCount");

            if (App.ViewModel.Portfolio.Count == PortfolioDetails.Count)
            {
                return;
            }
            // Add missing elements
            foreach (var stockPosition in App.ViewModel.Portfolio)
            {
                if (PortfolioDetails.Any(_ => _.StockName == stockPosition.Name))
                {
                    continue;
                }

                PortfolioDetails.Add(
                    new PortfolioDetailsModel()
                    {
                        StockName = stockPosition.Name,
                        StocksCount = stockPosition.NumberOfShares,
                        StocksValueNow = stockPosition.StockValueNow,
                        Opacity = Math.Max(1.0 - (0.2 * PortfolioDetails.Count), 0)
                    });
            }

            // Remove extra elements
            for (int i = 0; i < PortfolioDetails.Count; ++i)
            {
                if (App.ViewModel.Portfolio.Any(_ => _.Name == PortfolioDetails[i].StockName))
                {
                    continue;
                }
                PortfolioDetails.RemoveAt(i);
            }

            NotifyPropertyChanged("GraphVisibility");
        }

        public String PortfolioCurrentValue
        {
            get
            {
                if (App.ViewModel.Portfolio == null || App.ViewModel.Portfolio.Count == 0)
                {
                    return "0";
                }
                return App.ViewModel.Portfolio.Sum(_ => _.StockValueNow * _.NumberOfShares).ToString(".##", CultureInfo.InvariantCulture);
            }
        }

        public String PortfolioCurrentSharesCount
        {
            get
            {
                if (App.ViewModel.Portfolio == null || App.ViewModel.Portfolio.Count == 0)
                {
                    return "0";
                }
                return App.ViewModel.Portfolio.Sum(_ => _.NumberOfShares).ToString();
            }
        }

        public Visibility GraphVisibility
        {
            get
            {
                return App.ViewModel.Portfolio == null || App.ViewModel.Portfolio.Count == 0 ? Visibility.Collapsed : Visibility.Visible;
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
