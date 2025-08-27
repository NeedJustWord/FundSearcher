using System.Windows;
using System.Windows.Controls;
using Fund.Crawler.Extensions;
using FundSearcher.PubSubEvents;
using Prism.Events;

namespace FundSearcher.Views
{
    /// <summary>
    /// IndexQuery.xaml 的交互逻辑
    /// </summary>
    public partial class IndexQuery : UserControl
    {
        private readonly IEventAggregator aggregator;

        public IndexQuery(IEventAggregator aggregator)
        {
            InitializeComponent();
            this.aggregator = aggregator;
        }

        private void BtnCheckAll_Click(object sender, RoutedEventArgs e)
        {
            aggregator.Publish<IndexQueryCheckAllEvent>();
        }
    }
}
