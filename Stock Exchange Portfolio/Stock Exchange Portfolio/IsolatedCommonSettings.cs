using Common;
using System;
using System.Collections.Generic;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stock_Exchange_Portfolio
{
    public class IsolatedCommonSettings : IStorageSettings
    {
        private IsolatedStorageSettings isolatedStorageSettings = IsolatedStorageSettings.ApplicationSettings;

        public static void AttachToCommonSettings()
        {
            Settings.StorageSettings = new IsolatedCommonSettings();
        }

        public void Add(KeyValuePair<string, object> item)
        {
            isolatedStorageSettings.Add(item.Key, item.Value);
        }

        public void Clear()
        {
            isolatedStorageSettings.Clear();
        }

        public bool Contains(KeyValuePair<string, object> item)
        {
            return isolatedStorageSettings.Contains(item);
        }

        public void CopyTo(KeyValuePair<string, object>[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        public int Count
        {
            get
            {
                return isolatedStorageSettings.Count;
            }
        }

        public bool IsReadOnly
        {
            get { return false; }
        }

        public bool Remove(KeyValuePair<string, object> item)
        {
            return isolatedStorageSettings.Remove(item.Key);
        }

        public IEnumerator<KeyValuePair<string, object>> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }

        public void Add(object key, object value)
        {
            isolatedStorageSettings.Add((string)key, value);
        }

        public bool Contains(object key)
        {
            return isolatedStorageSettings.Contains((string)key);
        }

        System.Collections.IDictionaryEnumerator System.Collections.IDictionary.GetEnumerator()
        {
            throw new NotImplementedException();
        }

        public bool IsFixedSize
        {
            get { return false; }
        }

        public System.Collections.ICollection Keys
        {
            get
            {
                return isolatedStorageSettings.Keys;
            }
        }

        public void Remove(object key)
        {
            isolatedStorageSettings.Remove((string)key);
        }

        public System.Collections.ICollection Values
        {
            get
            {
                return isolatedStorageSettings.Values;
            }
        }

        public object this[object key]
        {
            get
            {
                return isolatedStorageSettings[(string)key];
            }
            set
            {
                isolatedStorageSettings[(string)key] = value;
            }
        }

        public void CopyTo(Array array, int index)
        {
            throw new NotImplementedException();
        }

        public bool IsSynchronized
        {
            get { return false; }
        }

        public object SyncRoot
        {
            get { throw new NotImplementedException(); }
        }

        public void Save()
        {
            isolatedStorageSettings.Save();
        }

        public void Add(string key, object value)
        {
            isolatedStorageSettings.Add(key, value);
        }

        public bool ContainsKey(string key)
        {
            return isolatedStorageSettings.Contains(key);
        }

        ICollection<string> IDictionary<string, object>.Keys
        {
            get
            {
                return (ICollection<string>)isolatedStorageSettings.Keys;
            }
        }

        public bool Remove(string key)
        {
            return isolatedStorageSettings.Remove(key);
        }

        public bool TryGetValue(string key, out object value)
        {
            return isolatedStorageSettings.TryGetValue<object>(key, out value);
        }

        ICollection<object> IDictionary<string, object>.Values
        {
            get
            {
                return (ICollection<object>)isolatedStorageSettings.Values;
            }
        }

        public object this[string key]
        {
            get
            {
                return isolatedStorageSettings[key];
            }
            set
            {
                isolatedStorageSettings[key] = value;
            }
        }
    }
}
