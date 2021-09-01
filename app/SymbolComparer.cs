//Copyright(c) 2021-2030, Muhammad Rahman
//All rights reserved.

//This source code is licensed under the Apache 2.0 License found in the
//LICENSE file in the root directory of this source tree. 

using Market.Price.Provider;
using System.Collections.Generic;
namespace Pricing.Win
{
    public class SymbolComparer : IEqualityComparer<StockPrice>
    {
        public new bool Equals(object x, object y)
        {
            var a = (StockPrice)x;
            var b = (StockPrice)y;
            if (a.Symbol == b.Symbol) return true;
            return false;
        }

        public bool Equals(StockPrice x, StockPrice y)
        {
            return x.Symbol == y.Symbol;
        }



        public int GetHashCode(StockPrice obj)
        {
            return obj.Symbol.GetHashCode();
        }
    }
}
