using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.Globalization;
using Common;

namespace Stock_Exchange_Portfolio
{
    public partial class ClosePositionPage : PhoneApplicationPage
    {
        public ClosePositionPage()
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
            double priceDouble;
            try
            {
                numberOfSharesInt = uint.Parse(numberOfShares.Text, CultureInfo.InvariantCulture);
                priceDouble = double.Parse(price.Text, CultureInfo.InvariantCulture);
            }
            catch
            {
                MessageBox.Show("Invalid number of shares");
                return;
            }
            if (numberOfSharesInt > App.StockInfoViewModel.Stocks)
            {
                MessageBox.Show("You can't have sold more stocks than you have");
                return;
            }
            else if (numberOfSharesInt < 0)
            {
                MessageBox.Show("Number of shares can't be less than 0.");
                return;
            }
            else if (priceDouble < 0)
            {
                MessageBox.Show("Price can't be less than 0.");
                return;
            }


            var stockPosition = new StockPosition()
            {
                Name = App.StockInfoViewModel.YahooQuote.ShortName,
                NumberOfShares = numberOfSharesInt
            };
            Settings.Portfolio.Remove(stockPosition);
            App.StockInfoViewModel.StocksChanged();
            NavigationService.GoBack();
        }
    }
}