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
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (!App.ViewModel.IsDataLoaded)
            {
                App.ViewModel.LoadData();
            }
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