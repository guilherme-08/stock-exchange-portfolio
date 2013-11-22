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
using System.Globalization;

namespace Stock_Exchange_Portfolio
{
    public partial class AddPositionPage : PhoneApplicationPage
    {
        public AddPositionPage()
        {
            InitializeComponent();

            DataContext = App.StockInfoViewModel;
        }

        private void OnCancelClick(object sender, EventArgs e)
        {
            NavigationService.GoBack();
        }

        private void OnConfirmClick(object sender, EventArgs e)
        {
            uint numberOfSharesInt;
            try
            {
                numberOfSharesInt = uint.Parse(numberOfShares.Text, CultureInfo.InvariantCulture);
            }
            catch {
                MessageBox.Show("Invalid number of shares");
                return;
            }
            var stockPosition = new StockPosition() {
                Name = App.StockInfoViewModel.YahooQuote.ShortName,
                NumberOfShares = numberOfSharesInt
            };
            
            Settings.Portfolio.Add(stockPosition);
            App.StockInfoViewModel.StocksChanged();
            NavigationService.GoBack();
        }
    }
}