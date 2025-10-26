using System.Windows.Controls;
using Fund.Crawler.Extensions;
using FundSearcher.PubSubEvents;
using Prism.Events;

namespace FundSearcher.Views
{
    /// <summary>
    /// FundBlack.xaml 的交互逻辑
    /// </summary>
    public partial class FundBlack : UserControl
    {
        private readonly IEventAggregator aggregator;

        public FundBlack(IEventAggregator aggregator)
        {
            InitializeComponent();
            this.aggregator = aggregator;
        }

        private void BtnCheckAll_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            aggregator.Publish<FundBlackCheckAllEvent>();
        }
    }
}
