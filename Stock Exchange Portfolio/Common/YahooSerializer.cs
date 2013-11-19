using Common.YahooResponses;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

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
                    string[] dataValues = data.Split(',');
                    var resultYahooQuote = new YahooQuote();
                    resultYahooQuote.Name = dataValues[0];
                    resultYahooQuote.Value = Double.Parse(dataValues[1], CultureInfo.InvariantCulture);
                    resultYahooQuote.DateString = dataValues[2];
                    resultYahooQuote.Time = dataValues[3];
                    resultYahooQuote.Volume = UInt32.Parse(dataValues[4]);
                    return resultYahooQuote;
                }
                case "YahooTable":
                {
                    string[] dataLines = data.Split('\n');

                    // Ignore first line and the last
                    dataLines = dataLines.Skip(1).Take(dataLines.Length - 2).ToArray();

                    return dataLines.Aggregate(new YahooTable(), (table, line) => {
                        string[] lineValues = line.Split(',');

                        table.Add(new YahooTableEntry()
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
                default:
                    return null;
            }
        }
    }
}
