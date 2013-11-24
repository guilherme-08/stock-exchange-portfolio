using System;
using System.Net;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Common;
using Common.YahooResponses;
using Common.YahooRequests;
using System.Collections.ObjectModel;
using AmCharts.Windows.QuickCharts;

namespace Stock_Exchange_Portfolio
{
    public partial class MainPage : PhoneApplicationPage
    {
        public MainPage()
        {
            InitializeComponent();

            DataContext = App.ViewModel;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (!App.ViewModel.IsDataLoaded)
            {
                App.ViewModel.LoadData();
            }
            App.ViewModel.Portfolio.ToList().ForEach(API.GetStockVariation);
        }

        private async void LongListSelector_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            var stockPosition = (sender as LongListSelector).SelectedItem as StockPosition;

            App.StockInfoViewModel = new ViewModels.StockInfoViewModel();
            App.StockInfoViewModel.YahooQuote = await API.GetAsync<YahooQuote>(API.Actions.GetQuote, stockPosition.Name);
            App.StockInfoViewModel.YahooQuote.ShortName = stockPosition.Name;
            NavigationService.Navigate(new Uri("/StockInfoPage.xaml", UriKind.Relative));
        }

        private async void ApplicationBarIconButton_Click(object sender, EventArgs e)
        {
            //App.StockInfoViewModel = new ViewModels.StockInfoViewModel();
            //App.StockInfoViewModel.YahooQuote = await API.GetAsync<YahooQuote>(API.Actions.GetQuote, "AAPL");
            //App.StockInfoViewModel.YahooQuote.ShortName = "AAPL";
            NavigationService.Navigate(new Uri("/SearchPage.xaml", UriKind.Relative));
        }
    }
}