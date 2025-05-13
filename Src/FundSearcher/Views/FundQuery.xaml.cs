using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using Fund.Crawler.Extensions;
using Fund.Crawler.Models;
using FundSearcher.Controls;
using FundSearcher.Extensions;
using FundSearcher.PubSubEvents;
using Prism.Events;

namespace FundSearcher.Views
{
    /// <summary>
    /// FundQuery.xaml 的交互逻辑
    /// </summary>
    public partial class FundQuery : UserControl
    {
        private readonly Dictionary<string, FundInfo> dictRefreshDetail = new Dictionary<string, FundInfo>();
        private readonly IEventAggregator aggregator;

        public FundQuery(IEventAggregator aggregator)
        {
            InitializeComponent();
            this.aggregator = aggregator;
            this.aggregator.Subscribe<FundQueryRefreshDetailEvent, FundInfo>(RefreshDetail);
        }

        private void BtnCheckAll_Click(object sender, RoutedEventArgs e)
        {
            aggregator.Publish<FundQueryCheckAllEvent>();
        }

        private void BtnAllCollapsed_Click(object sender, RoutedEventArgs e)
        {
            foreach (var item in dgFundInfos.Items)
            {
                dgFundInfos.SetDetailsVisibilityForItem(item, Visibility.Collapsed);

                var row = (DataGridRow)dgFundInfos.ItemContainerGenerator.ContainerFromItem(item);
                if (row != null)
                {
                    var tag = row.FindVisualTreeChild<TextBlock>("tag");
                    if (tag != null) tag.Text = "+";
                }
            }
        }

        private void BtnRowDetailShowHide_Click(object sender, RoutedEventArgs e)
        {
            var obj = sender as DependencyObject;
            var tb = obj.FindVisualTreeChild<TextBlock>("tag");
            if (tb == null) return;

            var row = obj.FindVisualTreeParent<DataGridRow>();
            if (row == null) return;

            if (tb.Text.IndexOf("-") > -1)
            {
                row.DetailsVisibility = Visibility.Collapsed;
                tb.Text = "+";
            }
            else
            {
                row.DetailsVisibility = Visibility.Visible;
                tb.Text = "-";
                RefreshDetail((FundInfo)row.Item);
            }
        }

        private void DataGridEx_Loaded(object sender, RoutedEventArgs e)
        {
            var dg = (DataGridEx)sender;
            var header = ((GroupBox)dg.Parent).Header.ToString();
            var key = $"{(int)dg.Tag},{header}";
            if (dictRefreshDetail.TryGetValue(key, out var info))
            {
                RefreshDetail(dg, header, info);
                dictRefreshDetail.Remove(key);
            }
        }

        private void RefreshDetail(FundInfo info)
        {
            var list = new List<DataGridEx>();
            dgFundInfos.FindVisualTreeChilds(ref list);

            var notFind = true;
            foreach (var item in list)
            {
                if ((int)item.Tag == info.OrderNumber)
                {
                    notFind = false;
                    RefreshDetail(item, ((GroupBox)item.Parent).Header.ToString(), info);
                }
            }

            if (notFind)
            {
                dictRefreshDetail[$"{info.OrderNumber},认购费率"] = info;
                dictRefreshDetail[$"{info.OrderNumber},申购费率"] = info;
                dictRefreshDetail[$"{info.OrderNumber},赎回费率"] = info;
            }
        }

        private void RefreshDetail(DataGridEx item, string header, FundInfo info)
        {
            item.ItemsSource = null;
            switch (header)
            {
                case "认购费率":
                    item.HiddenColumns = info.ApplyRatesHiddenColumns;
                    item.ItemsSource = info.TransactionInfo.ApplyRates;
                    break;
                case "申购费率":
                    item.HiddenColumns = info.BuyRatesHiddenColumns;
                    item.ItemsSource = info.TransactionInfo.BuyRates;
                    break;
                case "赎回费率":
                    item.HiddenColumns = info.SellRatesHiddenColumns;
                    item.ItemsSource = info.TransactionInfo.SellRates;
                    break;
            }
        }
    }
}
