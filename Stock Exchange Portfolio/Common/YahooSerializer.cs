using Common.YahooResponses;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Text.RegularExpressions;

namespace Common
{
    public class YahooSerializer
    {
        private Type type;

        public YahooSerializer(Type type)
        {
            this.type = type;
        }

        public object ReadString(string data)
        {
            switch (type.Name)
            {
                case "YahooQuote":
                {
                    var dataValues = Csv.ReadValues(data);
                    var resultYahooQuote = new YahooQuote();
                    resultYahooQuote.Name = dataValues[0].Trim('"');
                    resultYahooQuote.ShortName = dataValues[1].Trim('"');
                    resultYahooQuote.Value = Double.Parse(dataValues[2], CultureInfo.InvariantCulture);
                    resultYahooQuote.DateString = dataValues[3];
                    resultYahooQuote.Time = dataValues[4].Trim('"');
                    resultYahooQuote.Volume = UInt32.Parse(dataValues[5]);
                    return resultYahooQuote;
                }
                case "YahooTable":
                {
                    string[] dataLines = data.Split('\n');

                    // Ignore first line and the last
                    dataLines = dataLines.Skip(1).Take(dataLines.Length - 2).ToArray();

                    return dataLines.Aggregate(new YahooTable(), (table, line) =>
                    {
                        var lineValues = Csv.ReadValues(line);

                        table.Insert(0, new YahooTableEntry()
                        {
                            DateString = lineValues[0],
                            Open = Double.Parse(lineValues[1], CultureInfo.InvariantCulture),
                            High = Double.Parse(lineValues[2], CultureInfo.InvariantCulture),
                            Low = Double.Parse(lineValues[3], CultureInfo.InvariantCulture),
                            Close = Double.Parse(lineValues[4], CultureInfo.InvariantCulture),
                            Volume = UInt32.Parse(lineValues[5]),
                            AdjustedClose = Double.Parse(lineValues[6], CultureInfo.InvariantCulture)
                        });
                        return table;
                    });
                }
                case "YahooSearch":
                {
                    int jsonStart = data.IndexOf('(') + 1;
                    int jsonEnd = data.Length - 1;
                    string json = data.Substring(jsonStart, jsonEnd - jsonStart);

                    DataContractJsonSerializer jsonSerializer = new DataContractJsonSerializer(type);
                    var searchResult = (YahooSearch) jsonSerializer.ReadObject(new MemoryStream(Encoding.Unicode.GetBytes(json)));

                    searchResult.ResultSet.Result = searchResult.ResultSet.Result.Where(_ => _.typeDisp == "Equity").ToArray();
                    return searchResult;
                }
                default:
                    return null;
            }
        }
    }
}
