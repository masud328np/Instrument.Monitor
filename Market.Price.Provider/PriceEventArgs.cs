//Copyright(c) 2021-2030, Muhammad Rahman
//All rights reserved.

//This source code is licensed under the Apache 2.0 License found in the
//LICENSE file in the root directory of this source tree. 

using System.Collections.Generic;

namespace Market.Price.Provider
{
    public class PriceEventArgs
    {
        public IList<ProviderStockPrice> Prices { get; set; }
        public string Source { get; set; }
    }


}
