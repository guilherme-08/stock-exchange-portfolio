using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using Stock_Exchange_Portfolio.Resources;
using System.Collections.Generic;
using System.Linq;
using Common;
using System.Collections;

namespace Stock_Exchange_Portfolio.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        public MainViewModel()
        {
            this.Items = new ObservableCollection<ItemViewModel>();
            this.Portfolio = Settings.Portfolio;
            this.Portfolio.CollectionChanged += PortfolioChanged;
            portfolioGainers = new PortfolioCategory() { Name = "gainers" };
            portfolioLosers = new PortfolioCategory() { Name = "losers" };
        }

        private void PortfolioChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            App.ViewModel.portfolioLosers.Clear();
            App.ViewModel.portfolioGainers.Clear();

            var portfolios = Portfolio.ToLookup(_ => _.Variation.HasValue && _.Variation.Value >= 0.0d);

            foreach (var gainer in portfolios[true])
            {
                App.ViewModel.portfolioGainers.Add(gainer);
            }
            foreach (var loser in portfolios[false])
            {
                App.ViewModel.portfolioLosers.Add(loser);
            }
        }

        public ObservableCollection<ItemViewModel> Items { get; private set; }

        public bool IsDataLoaded
        {
            get;
            private set;
        }

        public void LoadData()
        {
            this.Items.Add(new ItemViewModel() { LineOne = "runtime one", LineTwo = "Maecenas praesent accumsan bibendum", LineThree = "Facilisi faucibus habitant inceptos interdum lobortis nascetur pharetra placerat pulvinar sagittis senectus sociosqu" });
            this.Items.Add(new ItemViewModel() { LineOne = "runtime five", LineTwo = "Maecenas praesent accumsan bibendum", LineThree = "Maecenas praesent accumsan bibendum dictumst eleifend facilisi faucibus habitant inceptos interdum lobortis nascetur" });
            this.Items.Add(new ItemViewModel() { LineOne = "runtime six", LineTwo = "Dictumst eleifend facilisi faucibus", LineThree = "Pharetra placerat pulvinar sagittis senectus sociosqu suscipit torquent ultrices vehicula volutpat maecenas praesent" });

            portfolioGainers.Add(new StockPosition() { Name = "GOOG" });
            portfolioLosers.Add(new StockPosition() { Name = "AAPL" });

            PortfoliosCategorized = new ObservableCollection<PortfolioCategory>();
            PortfoliosCategorized.Add(portfolioGainers);
            PortfoliosCategorized.Add(portfolioLosers);
            this.IsDataLoaded = true;
        }

        private PortfolioCategory portfolioGainers;

        private PortfolioCategory portfolioLosers;

        public ObservableCollection<PortfolioCategory> PortfoliosCategorized { get; set; }

        public Portfolio Portfolio { get; set; }

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