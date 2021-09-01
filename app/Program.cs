//Copyright(c) 2021-2030, Muhammad Rahman
//All rights reserved.

//This source code is licensed under the Apache 2.0 License found in the
//LICENSE file in the root directory of this source tree. 

using Market.Price.Provider;
using Pricing.Core;
using System;
using System.Threading;
using System.Windows.Forms;

namespace Pricing.Win
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            var priceProvider = new FakeStockPriceProvider();
            var tokenSrc = new CancellationTokenSource();
            priceProvider.StartAsync(tokenSrc.Token).GetAwaiter();
            var engine = new PricingEngine(new FakeStockPriceProvider());
            engine.StartAsync(tokenSrc.Token).GetAwaiter();
            Application.Run(new FrmMain(engine));
            tokenSrc.Cancel();
            priceProvider.Stop();
            engine.Stop();
            Application.Exit();
        }
    }
}
