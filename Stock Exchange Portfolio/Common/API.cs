using Common.YahooRequests;
using Common.YahooResponses;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public class API
    {
        public enum Actions
        {
            GetQuote,
            GetTable,
            Search
        }

        private static Uri getUrlFromAction(Actions action, params string[] parameters)
        {
            switch (action)
            {
                case Actions.GetQuote:
                    return new Uri(string.Format("http://finance.yahoo.com/d/quotes?f=nsl1d1t1v&s={0}", parameters));
                case Actions.GetTable:
                    return new Uri(string.Format("http://ichart.finance.yahoo.com/table.txt?{0}", parameters));
                case Actions.Search:
                    return new Uri(string.Format("http://d.yimg.com/autoc.finance.yahoo.com/autoc?query={0}&callback=YAHOO.Finance.SymbolSuggest.ssCallback", parameters));
                default:
                    throw new NotSupportedException("Unkown API.Actions.");
            }
        }

        public static async Task<T> GetAsync<T>(Actions action, params string[] parameters)
        {
            Uri uri = getUrlFromAction(action, parameters);

            HttpWebRequest request = (HttpWebRequest)WebRequest.CreateHttp(uri);
            request.AllowReadStreamBuffering = true;
            
            //var task = Task<WebResponse>.Factory.FromAsync(
            //    new Func<AsyncCallback, object, IAsyncResult>(request.BeginGetResponse),
            //    new Func<IAsyncResult, WebResponse>(request.EndGetResponse), TaskCreationOptions.HideScheduler);
            //var response = await task;

            var response = await request.GetResponseAsync();
            using (Stream streamResponse = response.GetResponseStream())
            using (StreamReader streamRead = new StreamReader(streamResponse, Encoding.UTF8))
            {
                var responseString = streamRead.ReadToEnd();

                var yahooSerializer = new YahooSerializer(typeof(T));
                return (T)yahooSerializer.ReadString(responseString);
            }
        }

        public static async void GetStockVariation(StockPosition stockPosition)
        {
            var yahooQuote = await GetAsync<YahooQuote>(Actions.GetQuote, stockPosition.Name);

            stockPosition.StockValueNow = yahooQuote.Value;
            if (stockPosition.StockCloseYesterday == null)
            {
                var yahooRequest = new YahooTableRequest(stockPosition.Name);
                yahooRequest.InitialDate = DateTime.Now.Subtract(new TimeSpan(6, 0, 0, 0)); // -4 days because weekend, -2 because holidays
                yahooRequest.Periodicity = YahooTableRequest.PeriodicityTypes.Daily;

                var yahooTable = await GetAsync<YahooTable>(Actions.GetTable, yahooRequest.ToString());

                var lastDay = yahooTable.Last();
                if (stockPosition.StockValueNow == 0 || yahooQuote.Date.Equals(lastDay.Date))
                {
                    if (yahooTable.Count >= 2)
                    {
                        stockPosition.StockCloseYesterday = (yahooTable[yahooTable.Count - 2] ?? new YahooTableEntry()).Close;
                    }
                }
                else
                {
                    stockPosition.StockCloseYesterday = (lastDay ?? new YahooTableEntry()).Close;
                }
            }
        }

        public static async Task<YahooTable> GetPortfolioValue(Portfolio portfolio, int numberOfDays)
        {
            if (portfolio.Count == 0 || numberOfDays < 0)
            {
                return null;
            }
            uint totalOfShares = portfolio.Aggregate(0u, (shares, stockPosition) =>
            {
                return shares + stockPosition.NumberOfShares;
            });

            var portfolioYahooTables = new List<YahooTable>();
            foreach (var stockPosition in portfolio)
            {
                var yahooRequest = new YahooTableRequest(stockPosition.Name);
                yahooRequest.InitialDate = DateTime.Now.Subtract(new TimeSpan(numberOfDays, 0, 0, 0)); // todo: consider weekends
                yahooRequest.Periodicity = YahooTableRequest.PeriodicityTypes.Daily;

                portfolioYahooTables.Add(await GetAsync<YahooTable>(Actions.GetTable, yahooRequest.ToString()));
            }
            var portfolioValue = new YahooTable();

            // Add an entry for the number of days
            portfolioValue.AddRange(Enumerable.Repeat(0, portfolioYahooTables[0].Count).Select(_ => new YahooTableEntry()).ToList());
            for (int i = 0; i < portfolioYahooTables[0].Count; ++i)
            {
                portfolioValue[i].DateString = portfolioYahooTables[0][i].DateString;
                for (int j = 0; j < portfolioYahooTables.Count; ++j)
                {
                    var yahooTable = portfolioYahooTables[j];
                    portfolioValue[i].Close += yahooTable[i].Close * portfolio[j].NumberOfShares;
                }
            }
            return portfolioValue;
        }
    }
}
