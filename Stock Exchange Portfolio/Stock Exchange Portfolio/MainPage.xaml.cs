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

            // Set the data context of the listbox control to the sample data
            DataContext = App.ViewModel;
        }

        // Load data for the ViewModel Items
        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (!App.ViewModel.IsDataLoaded)
            {
                App.ViewModel.LoadData();
            }

            Settings.Portfolio.Add(new StockPosition() { Name = "GOOG", NumberOfShares = 10 });

            //var yahooQuote = await API.GetAsync<YahooQuote>(API.Actions.GetQuote, "GOOG");
            // http://ichart.finance.yahoo.com/table.txt?a=9&b=5&c=2013&d=9&e=19&f=2013&g=d&s=GOOG
            var yahooTableGoogleRequest = new YahooTableRequest("GOOG");
            yahooTableGoogleRequest.InitialDay = 5;
            yahooTableGoogleRequest.InitialMonth = 9;
            yahooTableGoogleRequest.InitialYear = 2013;
            yahooTableGoogleRequest.FinalDay = 19;
            yahooTableGoogleRequest.FinalMonth = 9;
            yahooTableGoogleRequest.FinalYear = 2013;

            // http://ichart.finance.yahoo.com/table.txt?a=9&b=5&c=2013&d=9&e=19&f=2013&g=d&s=GOOG
            var yahooTableGoogle = await API.GetAsync<YahooTable>(API.Actions.GetTable, yahooTableGoogleRequest.ToString());
        }

        private async void LongListSelector_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            App.StockInfoViewModel = new ViewModels.StockInfoViewModel();
            App.StockInfoViewModel.YahooQuote = await API.GetAsync<YahooQuote>(API.Actions.GetQuote, "GOOG");
            App.StockInfoViewModel.YahooQuote.ShortName = "GOOG";
            NavigationService.Navigate(new Uri("/StockInfoPage.xaml", UriKind.Relative));
        }
    }
}