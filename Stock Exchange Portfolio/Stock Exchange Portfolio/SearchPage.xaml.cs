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

            Loaded += PageLoaded;
        }

        protected void PageLoaded(object sender, RoutedEventArgs args)
        {
            tbSearch.Focus();
        }

        private async void OnSearchTextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(tbSearch.Text))
            {
                llsResults.ItemsSource = null;
                gridNoResults.Visibility = Visibility.Collapsed;
                return;
            }
            progressBar.IsIndeterminate = true;
            progressBar.Visibility = Visibility.Visible;

            var yahooSearch = await API.GetAsync<YahooSearch>(API.Actions.Search, tbSearch.Text);
            llsResults.ItemsSource = yahooSearch.ResultSet.Result.ToList();

            if (llsResults.ItemsSource.Count == 0)
            {
                labelSearchString.Text = "                      " + yahooSearch.ResultSet.Query;
                gridNoResults.Visibility = Visibility.Visible;
            }
            else
            {
                gridNoResults.Visibility = Visibility.Collapsed;
            }
            progressBar.IsIndeterminate = false;
            progressBar.Visibility = Visibility.Collapsed;
        }

        private void OnResultTap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            var control = sender as FrameworkElement;

            var yahooSearchResult = ((YahooSearchResult) control.DataContext);

            NavigationService.Navigate(new Uri("/StockInfoPage.xaml?yahooQuote.ShortName=" + yahooSearchResult.symbol, UriKind.Relative));
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            NavigationService.RemoveBackEntry();
            App.SearchViewModel.SearchString = string.Empty;
        }
    }
}