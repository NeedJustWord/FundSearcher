using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using FundSearcher.Extensions;
using FundSearcher.Models;
using FundSearcher.PubSubEvents;
using Prism.Events;

namespace FundSearcher.Views
{
    /// <summary>
    /// FundCompare.xaml 的交互逻辑
    /// </summary>
    public partial class FundCompare : UserControl
    {
        private readonly IEventAggregator aggregator;

        public FundCompare(IEventAggregator aggregator)
        {
            InitializeComponent();
            this.aggregator = aggregator;
        }

        private void DataGrid_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            var brush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#EBF1DD"));
            var dg = (DataGrid)sender;
            DataGridCell cell;
            for (int row = 0; row < dg.Items.Count; row++)
            {
                for (int column = 1; column < dg.Columns.Count; column += 2)
                {
                    cell = dg.GetCell(row, column);
                    if (cell != null) cell.Background = brush;
                }
            }
        }

        private void ListView_PreviewMouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (e.OriginalSource is DependencyObject obj)
            {
                var item = obj.FindVisualTreeParent<ListViewItem>();
                var model = (FilterModel)item.DataContext;
                aggregator.Publish<FundCompareFilterModelClickEvent, FilterModel>(model);
            }
        }
    }
}
