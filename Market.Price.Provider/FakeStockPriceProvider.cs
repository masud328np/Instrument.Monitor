//Copyright(c) 2021-2030, Muhammad Rahman
//All rights reserved.

//This source code is licensed under the Apache 2.0 License found in the
//LICENSE file in the root directory of this source tree. 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Market.Price.Provider
{
    public class FakeStockPriceProvider : IPriceProvider
    {
        Task _nyseThread;
        Task _nasdaqThread;

        Dictionary<Guid, Dictionary<string, Action<object, PriceEventArgs>>> _priceSubscriberList = new Dictionary<Guid, Dictionary<string, Action<object, PriceEventArgs>>>();
        static event Action<object, PriceEventArgs> OnPriceUpdate;
        public void Subscribe(Guid engineId, string priceSrc, Action<object, PriceEventArgs> handler)
        {
            if (string.IsNullOrWhiteSpace(priceSrc)) return;

            if (!IsClientExist(engineId))
            {
                _priceSubscriberList.Add(engineId, new Dictionary<string, Action<object, PriceEventArgs>>() { { priceSrc, handler } });
                OnPriceUpdate += handler;
            }
            if (!IsHandlerExists(engineId, priceSrc))
            {
                _priceSubscriberList[engineId] = new Dictionary<string, Action<object, PriceEventArgs>>() { { priceSrc, handler } };
                OnPriceUpdate += handler;
            }
        }

        private bool IsClientExist(Guid engineId)
        {
            return _priceSubscriberList.Keys.Any(x => x.Equals(engineId));
        }

        private bool IsHandlerExists(Guid engineId, string priceSrc)
        {
            return _priceSubscriberList[engineId].Any(x => x.Key.Equals(priceSrc));
        }

        public void Unsubscribe(Guid engineId, string priceSrc = null)
        {
            if (string.IsNullOrWhiteSpace(priceSrc))
            {
                if (_priceSubscriberList.Keys.Any(x => x == engineId))
                {
                    var handlerList = _priceSubscriberList[engineId];

                    foreach (var handler in handlerList)
                    {
                        OnPriceUpdate -= handler.Value;
                    }
                    _priceSubscriberList.Remove(engineId);
                }

            }
            else
            {
                OnPriceUpdate -= _priceSubscriberList[engineId].FirstOrDefault(x => x.Key.Equals(priceSrc)).Value;
            }

        }
        void TriggerNYSEPricing(CancellationToken token)
        {
            var random = new Random();
            var prices = new List<ProviderStockPrice>() {
                    new ProviderStockPrice(){ Symbol="WAL",Price =Math.Round(random.NextDouble(),2)},
                    new ProviderStockPrice(){ Symbol="TAR",Price = Math.Round(random.NextDouble(),3)},
                    new ProviderStockPrice(){ Symbol="FB",Price = Math.Round(random.NextDouble(),4)},
                };
            while (!token.IsCancellationRequested)
            {

                foreach (var pr in prices)
                {
                    pr.Price = Math.Round(random.NextDouble(), random.Next(2, 6));
                }

                OnPriceUpdate?.Invoke(this, new PriceEventArgs() { Prices = prices, Source = "NYSE" });

                Thread.Sleep(1000);
            }

        }

        void TriggerNASDAQPricing(CancellationToken token)
        {
            var random = new Random();
            var prices = new List<ProviderStockPrice>() {
                    new ProviderStockPrice(){ Symbol="AAPL",Price = random.Next(100,105)},
                    new ProviderStockPrice(){ Symbol="GOOG",Price = random.Next(200,300)},
                    new ProviderStockPrice(){ Symbol="FB",Price = random.Next(500,600)},
                };

            while (!token.IsCancellationRequested)
            {
                foreach (var pr in prices)
                {
                    pr.Price = random.Next(random.Next(1000));
                }
                OnPriceUpdate?.Invoke(this, new PriceEventArgs() { Prices = prices, Source = "NASDAQ" });
                Thread.Sleep(1000);
            }

        }

        public async Task StartAsync(CancellationToken token)
        {
            _nyseThread = Task.Run(() => TriggerNYSEPricing(token),token);
            _nasdaqThread = Task.Run(() => TriggerNASDAQPricing(token),token);
            await _nyseThread;
            await _nasdaqThread;
        }

        public void Stop()
        {
            _nyseThread.Wait();
            _nyseThread.Wait();
        }
    }
}
