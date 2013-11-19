using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.YahooRequests
{
    public class YahooTableRequest
    {
        public enum PeriodicityTypes
        {
            Daily,
            Weekly,
            Monthly
        }

        public string Name;

        public int? InitialDay;
        public int? InitialMonth; // (0 denotes January, and so on)
        public int? InitialYear;
        public int? FinalDay;
        public int? FinalMonth;
        public int? FinalYear;

        public PeriodicityTypes Periodicity = PeriodicityTypes.Daily;

        public YahooTableRequest(string Name)
        {
            this.Name = Name;
        }

        public override string ToString()
        {
            string request = "s=" + Name;

            if (InitialDay.HasValue)
                request = "b=" + InitialDay.Value + "&" + request;

            if (InitialMonth.HasValue)
                request = "a=" + InitialMonth.Value + "&" + request;

            if (InitialYear.HasValue)
                request = "c=" + InitialYear.Value + "&" + request;

            if (FinalDay.HasValue)
                request = "e=" + FinalDay.Value + "&" + request;

            if (FinalMonth.HasValue)
                request = "d=" + FinalMonth.Value + "&" + request;

            if (FinalYear.HasValue)
                request = "f=" + FinalYear.Value + "&" + request;

            switch (Periodicity)
            {
                case PeriodicityTypes.Daily:
                    request = "g=d&" + request;
                    break;
                case PeriodicityTypes.Weekly:
                    request = "g=w&" + request;
                    break;
                case PeriodicityTypes.Monthly:
                    request = "g=m&" + request;
                    break;
            }

            return request;
        }
    }
}
