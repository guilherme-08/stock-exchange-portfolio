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
            string[] dataValues = data.Split(',');
            switch (type.Name)
            {
                case "YahooQuote":
                    var resultObject = new YahooQuote();
                    resultObject.Name = dataValues[0];
                    resultObject.Value = Double.Parse(dataValues[1], CultureInfo.InvariantCulture);
                    resultObject.DateString = dataValues[2];
                    resultObject.Time = dataValues[3];
                    resultObject.Volume = UInt32.Parse(dataValues[4]);
                    return resultObject;
                default:
                    return null;
            }
        }
    }
}
