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
using System.Threading.Tasks;

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
            if (NavigationService.CanGoBack)
            {
                // Probably came from HowToUsePage ...
                NavigationService.RemoveBackEntry();
            }
            if (!App.ViewModel.IsDataLoaded)
            {
                App.ViewModel.LoadData();
            }
            else if (App.ViewModel.IsDataUpdated == false)
            {
                Task.Factory.StartNew(delegate
                {
                    App.ViewModel.UpdateData();
                });
                //Deployment.Current.Dispatcher.BeginInvoke(delegate
                //{
                //    App.ViewModel.UpdateData();
                //});
            }

            Deployment.Current.Dispatcher.BeginInvoke(delegate
            {
                foreach (var stockPosition in App.ViewModel.Portfolio)
                {
                    API.GetStockVariation(stockPosition);
                }
                foreach (var stockPosition in App.ViewModel.WatchList)
                {
                    API.GetStockVariation(stockPosition);
                }
            });
        }

        private async void OnStockPositionTap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            var stockPosition = (StockPosition)((FrameworkElement)sender).DataContext;
            
            App.StockInfoViewModel = new ViewModels.StockInfoViewModel();
            App.StockInfoViewModel.YahooQuote = await API.GetAsync<YahooQuote>(API.Actions.GetQuote, stockPosition.Name);
            App.StockInfoViewModel.YahooQuote.ShortName = stockPosition.Name;
            NavigationService.Navigate(new Uri("/StockInfoPage.xaml", UriKind.Relative));
        }

        private void ApplicationBarIconButton_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/SearchPage.xaml", UriKind.Relative));
        }

        private void OnPortfolioTap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            NavigationService.Navigate(new Uri("/PortfolioDetailsPage.xaml", UriKind.Relative));
        }

        private void OnHowToUseClick(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/HowToUsePage.xaml", UriKind.Relative));
        }
    }

}