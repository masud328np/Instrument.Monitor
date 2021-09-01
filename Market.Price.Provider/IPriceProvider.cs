//Copyright(c) 2021-2030, Muhammad Rahman
//All rights reserved.

//This source code is licensed under the Apache 2.0 License found in the
//LICENSE file in the root directory of this source tree. 

using System;
using System.Threading;
using System.Threading.Tasks;

namespace Market.Price.Provider
{
    public interface IPriceProvider
    {
        void Subscribe(Guid engineId, string priceSrc, Action<object, PriceEventArgs> handler);

        void Unsubscribe(Guid engineId, string priceSrc = null);

        Task StartAsync(CancellationToken token);
        void Stop();

    }
}