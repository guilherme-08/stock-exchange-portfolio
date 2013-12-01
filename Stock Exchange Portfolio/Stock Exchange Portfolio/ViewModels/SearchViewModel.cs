using Common;
using Common.YahooResponses;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stock_Exchange_Portfolio.ViewModels
{
    public class SearchViewModel : INotifyPropertyChanged
    {
        private string searchString;
        public string SearchString
        {
            get
            {
                return searchString;
            }
            set
            {
                if (value != searchString)
                {
                    searchString = value;
                    NotifyPropertyChanged("SearchString");
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(String propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (null != handler)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
