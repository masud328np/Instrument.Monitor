//Copyright(c) 2021-2030, Muhammad Rahman
//All rights reserved.

//This source code is licensed under the Apache 2.0 License found in the
//LICENSE file in the root directory of this source tree. 

using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Market.Price.Provider
{
    public class StockPrice : INotifyPropertyChanged
    {
        private string _symbol = String.Empty;
        private double _price = 0;
        public string Symbol
        {
            get
            {
                return _symbol;
            }
            set
            {
                if (value != _symbol)
                {
                    _symbol = value;
                    NotifyPropertyChanged();
                }
            }
        }
        public double Price
        {
            get
            {
                return _price;
            }
            set
            {
                if (value != _price)
                {
                    _price = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }



    public class ProviderStockPrice
    {
        public string Symbol { get; set; }
        public double Price { get; set; }
    }
}