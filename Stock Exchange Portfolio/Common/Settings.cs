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

    }
}
