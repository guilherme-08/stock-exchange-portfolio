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
            GetGoogleImageSearch
        }

        private static Uri getUrlFromAction(Actions action, params object[] parameters)
        {
            switch (action)
            {
                case Actions.GetQuote:
                    return new Uri(string.Format("http://finance.yahoo.com/d/quotes?f=sl1d1t1v&s={0}", parameters));
                case Actions.GetGoogleImageSearch:
                    return new Uri(string.Format("http://ichart.finance.yahoo.com/table.txt?{0}&g=d&s={1}", parameters));
            }
            return null;
        }

        public static async Task<T> GetAsync<T>(Actions action, params object[] parameters)
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

    }
}
