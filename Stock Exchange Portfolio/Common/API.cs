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
            GetTable
        }

        private static Uri getUrlFromAction(Actions action, params string[] parameters)
        {
            switch (action)
            {
                case Actions.GetQuote:
                    return new Uri(string.Format("http://finance.yahoo.com/d/quotes?f=nsl1d1t1v&s={0}", parameters));
                case Actions.GetTable:
                    return new Uri(string.Format("http://ichart.finance.yahoo.com/table.txt?{0}", parameters));
            }
            return null;
        }

        public static async Task<T> GetAsync<T>(Actions action, params string[] parameters)
        {
            Uri uri = getUrlFromAction(action, parameters);

            HttpWebRequest request = (HttpWebRequest)WebRequest.CreateHttp(uri);
            //HttpWebResponse response = (HttpWebResponse)await request.GetResponseAsync();
            request.AllowReadStreamBuffering = true;
            var task = Task<WebResponse>.Factory.FromAsync(
                new Func<AsyncCallback, object, IAsyncResult>(request.BeginGetResponse),
                new Func<IAsyncResult, WebResponse>(request.EndGetResponse), null);
            var response = await task;

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
            if (stockPosition.StockCloseYesterday == null)
            {
                var yahooRequest = new YahooTableRequest(stockPosition.Name);
                yahooRequest.InitialDate = DateTime.Now.Subtract(new TimeSpan(2, 0, 0, 0)); // -2 days because sunday
                yahooRequest.Periodicity = YahooTableRequest.PeriodicityTypes.Daily;

                var yahooTable = await GetAsync<YahooTable>(Actions.GetTable, yahooRequest.ToString());

                stockPosition.StockCloseYesterday = (yahooTable.Last() ?? new YahooTableEntry()).Close;
            }
            var yahooQuote = await GetAsync<YahooQuote>(Actions.GetQuote, stockPosition.Name);

            stockPosition.StockValueNow = yahooQuote.Value;
        }
    }
}
