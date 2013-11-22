using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common
{
    public interface IStorageSettings : IDictionary<string, object>, ICollection<KeyValuePair<string, object>>, IEnumerable<KeyValuePair<string, object>>, IDictionary, ICollection, IEnumerable
    {
        void Save();
    }
}
