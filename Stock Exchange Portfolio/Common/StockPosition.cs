using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    [DataContractAttribute]
    public class StockPosition : IEqualityComparer
    {
        [DataMember]
        public string Name;

        [DataMember]
        public uint NumberOfShares;

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
    }
}
