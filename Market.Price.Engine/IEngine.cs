//Copyright(c) 2021-2030, Muhammad Rahman
//All rights reserved.

//This source code is licensed under the   Apache 2.0 License  found in the
//LICENSE file in the root directory of this source tree. 
using Market.Price.Provider;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Pricing.Core
{
    public interface IEngine
    {
        Task StartAsync(CancellationToken token);
        void Stop();

        void ClientStart(Guid clientId, Action<List<ProviderStockPrice>> handler);
        void ClientStop(Guid clientId);
        void Subscribe(Guid clientId, string symbol, string source);
        void Unsubscribe(Guid clientId, string symbol, string source);
    }
}