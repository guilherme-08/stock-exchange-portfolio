using Common;
using Common.YahooRequests;
using Common.YahooResponses;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace Stock_Exchange_Portfolio
{
    public partial class StockInfoPage : PhoneApplicationPage
    {
        private static readonly int[] stocksGraphsDates = new int[] { 5, 15, 30, 60, 120, 360 };

        public StockInfoPage()
        {
            InitializeComponent();

            DataContext = App.StockInfoViewModel;

            Loaded += StockInfoPage_Loaded;
        }

        private async void StockInfoPage_Loaded(object sender, RoutedEventArgs e)
        {
            var yahooGoogRequest = new YahooTableRequest("GOOG");
            yahooGoogRequest.InitialDate = DateTime.Now.Subtract(new TimeSpan(stocksGraphsDates[0], 0, 0, 0));
            yahooGoogRequest.FinalDate = DateTime.Now;

            App.StockInfoViewModel.YahooTables[0] = await API.GetAsync<YahooTable>(API.Actions.GetTable, yahooGoogRequest.ToString());
            progressBar.IsIndeterminate = false;
            progressBar.Visibility = System.Windows.Visibility.Collapsed;
        }

        private async void OnStocksGraphsSelectionChanged(object sender, SelectionChangedEventArgs args)
        {
            var index = pStocksGraphs.SelectedIndex;
            if (App.StockInfoViewModel.YahooTables[index] == null)
            {
                progressBar.IsIndeterminate = true;
                progressBar.Visibility = System.Windows.Visibility.Visible;
                var yahooGoogRequest = new YahooTableRequest("GOOG");
                yahooGoogRequest.InitialDate = DateTime.Now.Subtract(new TimeSpan(stocksGraphsDates[index], 0, 0, 0));
                yahooGoogRequest.FinalDate = DateTime.Now;

                App.StockInfoViewModel.YahooTables[index] = await API.GetAsync<YahooTable>(API.Actions.GetTable, yahooGoogRequest.ToString());
                progressBar.IsIndeterminate = false;
                progressBar.Visibility = System.Windows.Visibility.Collapsed;
            }
        }
    }
}