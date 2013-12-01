using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Collections.ObjectModel;

namespace Common
{
    public class Settings
    {
        public static IStorageSettings StorageSettings;

        private static bool? isFirstTime = null;

        public static bool IsFirstTime
        {
            get
            {
                if (isFirstTime.HasValue == false)
                {
                    if (StorageSettings.Contains("IsFirstTime"))
                    {
                        isFirstTime = false;
                    }
                    else
                    {
                        StorageSettings.Add("IsFirstTime", false);
                        isFirstTime = true;
                    }
                }
                return isFirstTime.Value;
            }
        }

        private static Portfolio portfolio = null;

        public static Portfolio Portfolio
        {
            get
            {
                if (portfolio == null)
                {
                    if (StorageSettings.Contains("Portfolio"))
                    {
                        portfolio = (Portfolio)StorageSettings["Portfolio"];
                        portfolio.CollectionChanged += (sender, args) =>
                        {
                            StorageSettings.Save();
                        };
                    }
                    else
                    {
                        portfolio = new Portfolio();
                        StorageSettings.Add("Portfolio", portfolio);
                        portfolio.CollectionChanged += (sender, args) =>
                        {
                            StorageSettings.Save();
                        };
                    }
                }
                return portfolio;
            }
        }

        private static Portfolio watchList = null;

        public static Portfolio WatchList
        {
            get
            {
                if (watchList == null)
                {
                    if (StorageSettings.Contains("WatchList"))
                    {
                        watchList = (Portfolio)StorageSettings["WatchList"];
                        watchList.CollectionChanged += (sender, args) =>
                        {
                            StorageSettings.Save();
                        };
                    }
                    else
                    {
                        watchList = new Portfolio();
                        StorageSettings.Add("WatchList", watchList);
                        watchList.CollectionChanged += (sender, args) =>
                        {
                            StorageSettings.Save();
                        };
                    }
                }
                return watchList;
            }
        }
    }
}
