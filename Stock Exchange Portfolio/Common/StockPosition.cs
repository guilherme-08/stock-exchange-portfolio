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

        public bool Equals(object x, object y)
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
