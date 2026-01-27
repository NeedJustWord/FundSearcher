using System.Windows;
using System.Windows.Controls;
using Fund.Core.Consts;
using Fund.Crawler.Models;
using FundSearcher.Controls;

namespace FundSearcher.Views
{
    /// <summary>
    /// FundDetail.xaml 的交互逻辑
    /// </summary>
    public partial class FundDetail : UserControl
    {
        public FundDetail()
        {
            InitializeComponent();
        }

        private void DataGridEx_Loaded(object sender, RoutedEventArgs e)
        {
            var dg = (DataGridEx)sender;
            var header = ((GroupBox)dg.Parent).Header.ToString();
            var vm = (FundDetailViewModel)dg.DataContext;
            RefreshDetail(dg, header, vm.FundInfo);
        }

        private void RefreshDetail(DataGridEx item, string header, FundInfo info)
        {
            item.ItemsSource = null;
            switch (header)
            {
                case TransactionColumnName.ApplyRates:
                    item.HiddenColumns = info.ApplyRatesHiddenColumns;
                    item.ItemsSource = info.TransactionInfo?.ApplyRates;
                    break;
                case TransactionColumnName.BuyRates:
                    item.HiddenColumns = info.BuyRatesHiddenColumns;
                    item.ItemsSource = info.TransactionInfo?.BuyRates;
                    break;
                case TransactionColumnName.SellRates:
                    item.HiddenColumns = info.SellRatesHiddenColumns;
                    item.ItemsSource = info.TransactionInfo?.SellRates;
                    break;
            }
        }
    }
}
