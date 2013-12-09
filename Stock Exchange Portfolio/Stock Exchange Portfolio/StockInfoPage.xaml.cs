using Common;
using Common.YahooRequests;
using Common.YahooResponses;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        private static readonly Uri AssetAddFavorite = new Uri("Assets/AppBar/favs.addto.png", UriKind.Relative);

        private static readonly Uri AssetRemoveFavorite = new Uri("Assets/AppBar/favs.png", UriKind.Relative);

        private const string AddText = "add";

        private const string RemoveText = "remove";

        private static readonly int[] stocksGraphsDates = new int[] { 5, 15, 30, 60, 120, 360, 720 };

        ApplicationBarIconButton AppBarFavoriteIconButton;

        public StockInfoPage()
        {
            InitializeComponent();

            DataContext = App.StockInfoViewModel;
        }

        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            // Binding related code
            bool performRequest = false;
            if (NavigationContext.QueryString.ContainsKey("yahooQuote.ShortName"))
            {
                string shortName = NavigationContext.QueryString["yahooQuote.ShortName"];

                if (App.StockInfoViewModel.YahooQuote.ShortName != shortName)
                {
                    App.StockInfoViewModel.ResetYahooTables();
                    App.StockInfoViewModel.YahooQuote = new YahooQuote { ShortName = shortName };
                    performRequest = true;
                }
            }

            // Buttons related code
            if (App.StockInfoViewModel.Stocks > 0 && ApplicationBar.Buttons.Count == 2)
            {
                var sellButton = new ApplicationBarIconButton(new Uri("Assets/AppBar/minus.png", UriKind.Relative));
                sellButton.Text = "sell";
                sellButton.Click += OnSellButtonClick;
                ApplicationBar.Buttons.Insert(1, sellButton);
            }
            else if (App.StockInfoViewModel.Stocks == 0 && ApplicationBar.Buttons.Count == 4)
            {
                ApplicationBar.Buttons.RemoveAt(1);
            }

            AppBarFavoriteIconButton = (ApplicationBarIconButton)ApplicationBar.Buttons[ApplicationBar.Buttons.Count - 1];
            if (App.ViewModel.WatchList.Any(stock => stock.Name == App.StockInfoViewModel.YahooQuote.ShortName))
            {
                AppBarFavoriteIconButton.IconUri = AssetRemoveFavorite;
            }
            else
            {
                AppBarFavoriteIconButton.IconUri = AssetAddFavorite;
            }
            if (performRequest)
            {
                App.StockInfoViewModel.YahooQuote = await API.GetAsync<YahooQuote>(API.Actions.GetQuote, App.StockInfoViewModel.YahooQuote.ShortName);
            }
        }

        private async void OnStocksGraphsSelectionChanged(object sender, SelectionChangedEventArgs args)
        {
            var index = pStocksGraphs.SelectedIndex;
            if (App.StockInfoViewModel.YahooTables[index] == null)
            {
                progressBar.IsIndeterminate = true;
                progressBar.Visibility = System.Windows.Visibility.Visible;
                var yahooGoogRequest = new YahooTableRequest(App.StockInfoViewModel.YahooQuote.ShortName);
                yahooGoogRequest.InitialDate = DateTime.Now.Subtract(new TimeSpan(stocksGraphsDates[index], 0, 0, 0));
                yahooGoogRequest.FinalDate = DateTime.Now;

                var taskResult = API.GetAsync<YahooTable>(API.Actions.GetTable, yahooGoogRequest.ToString());
                App.StockInfoViewModel.YahooTables[index] = await taskResult;
                progressBar.IsIndeterminate = false;
                progressBar.Visibility = System.Windows.Visibility.Collapsed;
            }
            else
            {
                progressBar.IsIndeterminate = false;
                progressBar.Visibility = System.Windows.Visibility.Collapsed;
            }
        }

        private void AddToPortfolio(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/AddPositionPage.xaml", UriKind.Relative));
        }

        private void OnWatchListClick(object sender, EventArgs e)
        {
            var stock = new StockPosition()
            {
                Name = App.StockInfoViewModel.YahooQuote.ShortName,
                NumberOfShares = 0,
                StockValueNow = App.StockInfoViewModel.YahooQuote.Value
            };
            if (AppBarFavoriteIconButton.IconUri == AssetRemoveFavorite)
            {
                Settings.WatchList.Remove(stock);
                AppBarFavoriteIconButton.IconUri = AssetAddFavorite;
                AppBarFavoriteIconButton.Text = AddText;
            }
            else
            {
                Settings.WatchList.Add(stock);
                AppBarFavoriteIconButton.IconUri = AssetRemoveFavorite;
                AppBarFavoriteIconButton.Text = RemoveText;
            }
        }

        private void OnSellButtonClick(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/ClosePositionPage.xaml", UriKind.Relative));
        }
    }
}