//Copyright(c) 2021-2030, Muhammad Rahman
//All rights reserved.

//This source code is licensed under the   Apache 2.0 License  found in the
//LICENSE file in the root directory of this source tree. 
using Market.Price.Provider;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Pricing.Core
{
    public class SymbolInfo
    {
        public string Name { get; set; }
        public string Source { get; set; }
    }

    public class Composite
    {
        public Action<List<ProviderStockPrice>> Handler { get; set; }
        public ConcurrentDictionary<string, SymbolInfo> Symbols { get; set; }
    }
}
