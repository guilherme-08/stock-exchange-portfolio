using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    [DataContractAttribute]
    public class StockPosition : IEqualityComparer, INotifyPropertyChanged
    {
        private string name;

        [DataMember]
        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                if (value != name)
                {
                    name = value;
                    NotifyPropertyChanged("Name");
                }
            }
        }
        private uint numberOfShares;

        [DataMember]
        public uint NumberOfShares
        {
            get
            {
                return numberOfShares;
            }
            set
            {
                if (value != numberOfShares)
                {
                    numberOfShares = value;
                    NotifyPropertyChanged("NumberOfShares");
                }
            }
        }

        private double? stockCloseYesterday;

        public double? StockCloseYesterday
        {
            get
            {
                return stockCloseYesterday;
            }
            set
            {
                if (value != stockCloseYesterday)
                {
                    stockCloseYesterday = value;
                    NotifyPropertyChanged("StockCloseYesterday");
                }
            }
        }

        private double stockValueNow;

        public double StockValueNow
        {
            get
            {
                return stockValueNow;
            }
            set
            {
                if (value != stockValueNow)
                {
                    stockValueNow = value;
                    NotifyPropertyChanged("StockValueNow");
                    NotifyPropertyChanged("Variation");
                }
            }
        }

        public double? Variation
        {
            get
            {
                if (stockCloseYesterday.HasValue == false)
                {
                    return null;
                }

                return (stockValueNow / stockCloseYesterday.Value - 1.0d) * 100.0d;
            }
        }

        public new bool Equals(object x, object y)
        {
            if (x is StockPosition == false || y is StockPosition == false)
            {
                return false;
            }
            return ((StockPosition)x).Name == ((StockPosition)y).Name;
        }

        public int GetHashCode(object obj)
        {
            return Name.GetHashCode();
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
