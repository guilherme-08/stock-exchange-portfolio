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

namespace Stock_Exchange_Portfolio
{
    public partial class SearchPage : PhoneApplicationPage
    {
        public SearchPage()
        {
            InitializeComponent();

            DataContext = App.SearchViewModel;
            Loaded += PageLoaded;
        }

        protected void PageLoaded(object sender, RoutedEventArgs args)
        {
            tbSearch.Focus();
        }

        private async void OnSearchTextChanged(object sender, TextChangedEventArgs e)
        {
            progressBar.IsIndeterminate = true;
            progressBar.Visibility = Visibility.Visible;

            var yahooSearch = await API.GetAsync<YahooSearch>(API.Actions.Search, tbSearch.Text);
            llsResults.ItemsSource = yahooSearch.ResultSet.Result.ToList();

            progressBar.IsIndeterminate = false;
            progressBar.Visibility = Visibility.Collapsed;
        }

        private async void OnResultTap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            var control = sender as FrameworkElement;

            var yahooSearchResult = ((YahooSearchResult) control.DataContext);

            App.StockInfoViewModel = new ViewModels.StockInfoViewModel();
            App.StockInfoViewModel.YahooQuote = await API.GetAsync<YahooQuote>(API.Actions.GetQuote, yahooSearchResult.symbol);
            App.StockInfoViewModel.YahooQuote.ShortName = yahooSearchResult.symbol;
            NavigationService.Navigate(new Uri("/StockInfoPage.xaml", UriKind.Relative));
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            NavigationService.RemoveBackEntry();
            App.SearchViewModel.SearchString = string.Empty;
        }
    }
}