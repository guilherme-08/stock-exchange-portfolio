using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Common;
using Common.YahooResponses;
using Stock_Exchange_Portfolio.ViewModels;

namespace Stock_Exchange_Portfolio
{
    public partial class PortfolioDetailsPage : PhoneApplicationPage
    {
        public PortfolioDetailsPage()
        {
            InitializeComponent();
        }

        private async void OnStockTap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            var stockPosition = (PortfolioDetailsModel)((FrameworkElement)sender).DataContext;

            App.StockInfoViewModel = new ViewModels.StockInfoViewModel();
            App.StockInfoViewModel.YahooQuote = await API.GetAsync<YahooQuote>(API.Actions.GetQuote, stockPosition.StockName);
            App.StockInfoViewModel.YahooQuote.ShortName = stockPosition.StockName;
            NavigationService.Navigate(new Uri("/StockInfoPage.xaml", UriKind.Relative));
        }
    }
}