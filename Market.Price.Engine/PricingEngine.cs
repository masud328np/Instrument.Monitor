//Copyright(c) 2021-2030, Muhammad Rahman
//All rights reserved.

//This source code is licensed under the Apache 2.0 License  found in the
//LICENSE file in the root directory of this source tree. 

using Market.Price.Provider;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Pricing.Core
{
    public class PricingEngine : IEngine
    {
        readonly ConcurrentQueue<PriceEventArgs> _priceMsgs = new ConcurrentQueue<PriceEventArgs>();
        IPriceProvider _priceProvider;
        Task _publishTask;
        Guid _engineId = Guid.NewGuid();
        public PricingEngine(IPriceProvider priceProvider)
        {
            _priceProvider = priceProvider;
        }
        public static ConcurrentDictionary<Guid, Composite> _clients = new ConcurrentDictionary<Guid, Composite>();

        public async Task StartAsync(CancellationToken token)
        {
            if (_publishTask == null)
            {
                _publishTask = Task.Run(() => PublishPricing(token), token);
                await _publishTask;
            }
        }

        public void Stop()
        {
            _priceProvider.Unsubscribe(_engineId);
            _publishTask.Wait();
            _publishTask = null;           
        }
        public void ClientStart(Guid clientId, Action<List<ProviderStockPrice>> handler)
        {
            _clients.AddOrUpdate(clientId, new Composite() { Handler = handler, Symbols = new ConcurrentDictionary<string, SymbolInfo>() }, (x, y) => y);

        }

        public void OnPriceUpdate(object sender, PriceEventArgs args)
        {
            _priceMsgs.Enqueue(args);
        }

        public void ClientStop(Guid clientId)
        {
            _clients.TryRemove(clientId, out Composite comp);
        }

        public void Subscribe(Guid clientId, string symbol, string source)
        {
            if (string.IsNullOrWhiteSpace(symbol) || string.IsNullOrWhiteSpace(source)) return;

            if (_clients.TryGetValue(clientId, out Composite comp))
            {
                comp.Symbols.AddOrUpdate(symbol + source, new SymbolInfo() { Name = symbol, Source = source }, (a, b) => b);
                _priceProvider.Subscribe(_engineId, source, OnPriceUpdate);
            }
        }

        public void Unsubscribe(Guid clientId, string symbol, string source)
        {
            if (string.IsNullOrWhiteSpace(symbol) || string.IsNullOrWhiteSpace(source)) return;

            if (_clients.TryGetValue(clientId, out Composite val))
            {
                val.Symbols.TryRemove(symbol + source, out SymbolInfo info);
            }
        }

        void PublishPricing(CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                if (_priceMsgs.TryDequeue(out PriceEventArgs args))
                {
                    foreach (var client in _clients)
                    {
                        var prices = args.Prices.Where(x => client.Value.Symbols.Any(y => y.Key == x.Symbol + args.Source)).ToList();
                        if (prices.Count() > 0)
                        {
                            client.Value.Handler(prices);
                        }
                    }
                }

                Thread.Sleep(250);
            }
        }

    }
}
