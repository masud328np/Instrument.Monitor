//Copyright(c) 2021-2030, Muhammad Rahman
//All rights reserved.

//This source code is licensed under the Apache 2.0 License found in the
//LICENSE file in the root directory of this source tree. 
using Market.Price.Provider;
using Pricing.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace Pricing.Win
{
    public partial class FrmMain : Form
    {
        readonly Guid _clientId = Guid.NewGuid();
        readonly IEngine _engine;
        readonly BindingSource _bindSrc = new BindingSource();
        readonly BindingSource _cmbBindSrc = new BindingSource();
        delegate void UpdatePriceDelegate(IList<ProviderStockPrice> prices);

        public FrmMain(IEngine engine)
        {
            InitializeComponent();
            _engine = engine;
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            _engine.ClientStart(_clientId, x =>
            {
                txtSymbolSub.Invoke(new UpdatePriceDelegate(Refresh), x);
            });

            ChangeButton();
        }

        private void ChangeButton()
        {
            btnStop.Enabled = !btnStop.Enabled;
            btnStart.Enabled = !btnStart.Enabled;
            cmbSymbol.Enabled = !cmbSymbol.Enabled;
            cmbSource.Enabled = !cmbSource.Enabled;
            btnSubscribe.Enabled = !btnSubscribe.Enabled;
            btnUnsubscribe.Enabled = !btnUnsubscribe.Enabled;
        }

        private void BtnSubscribe_Click(object sender, EventArgs e)
        {
            _engine.Subscribe(_clientId, cmbSymbol.SelectedItem as string, cmbSource.SelectedItem as string);
        }

        private void BtnUnsubscribe_Click(object sender, EventArgs e)
        {
            _engine.Unsubscribe(_clientId, cmbSymbol.SelectedItem as string, cmbSource.SelectedItem as string);
        }

        void Refresh(IList<ProviderStockPrice> prices)
        {
            if (!(_bindSrc.DataSource is BindingList<StockPrice> curr))
            {
                curr = new BindingList<StockPrice>(prices.Select(x => new StockPrice() { Symbol = x.Symbol, Price = x.Price }).ToList());
                _bindSrc.DataSource = curr;
                dataGridView1.DataSource = _bindSrc.DataSource;
                return;
            }

            foreach (var item in prices)
            {
                var tmpRow = curr.FirstOrDefault(x => x.Symbol == item.Symbol);
                if (tmpRow != null)
                {
                    tmpRow.Price = item.Price;
                }
                else
                {
                    curr.Add(new StockPrice() { Symbol = item.Symbol, Price = item.Price });
                }
            }
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            _engine.ClientStop(_clientId);
            ChangeButton();
        }


        private void CmbSource_SelectedIndexChanged(object sender, EventArgs e)
        {
            var src = (sender as ComboBox).SelectedItem as string;
            if (src == "NASDAQ")
            {
                _cmbBindSrc.DataSource = new List<string> { "AAPL", "GOOG", "FB" };
            }
            else
            {
                _cmbBindSrc.DataSource = new List<string> { "WAL", "TAR", "FB" };
            }
            cmbSymbol.DataSource = _cmbBindSrc.DataSource;
            cmbSymbol.Refresh();
        }


    }
}
