using System.Windows.Controls;
using System.Windows.Media;
using FundSearcher.Extensions;

namespace FundSearcher.Views
{
    /// <summary>
    /// FundCompare.xaml 的交互逻辑
    /// </summary>
    public partial class FundCompare : UserControl
    {
        public FundCompare()
        {
            InitializeComponent();
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
    }
}
