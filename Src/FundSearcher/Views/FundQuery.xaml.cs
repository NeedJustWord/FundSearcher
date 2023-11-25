using System.Windows;
using System.Windows.Controls;
using FundSearcher.Extensions;

namespace FundSearcher.Views
{
    /// <summary>
    /// FundQuery.xaml 的交互逻辑
    /// </summary>
    public partial class FundQuery : UserControl
    {
        public FundQuery()
        {
            InitializeComponent();
        }

        private void BtnAllCollapsed_Click(object sender, RoutedEventArgs e)
        {
            foreach (var item in dgFundInfos.Items)
            {
                dgFundInfos.SetDetailsVisibilityForItem(item, Visibility.Collapsed);

                var row = (DataGridRow)dgFundInfos.ItemContainerGenerator.ContainerFromItem(item);
                if (row != null)
                {
                    row.FindVisualTreeChild<TextBlock>("tag").Text = "+";
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
            }
        }
    }
}
