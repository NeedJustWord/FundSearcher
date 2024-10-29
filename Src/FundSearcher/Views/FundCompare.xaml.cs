using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Fund.Core.Extensions;
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

        private void DataGrid_Loaded(object sender, RoutedEventArgs e)
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

            var maxColumns = new string[]
            {
                "资产规模(亿元)",
                "份额规模(亿份)",
            };
            var minColumns = new string[]
            {
                "购买费率",
                "管理费率(每年)",
                "托管费率(每年)",
                "销售服务费率(每年)",
                "运作费用(每年)",
            };
            var resultCells = new List<DataGridCell>();
            for (int column = 0; column < dg.Columns.Count; column++)
            {
                if (maxColumns.Contains(dg.Columns[column].Header.ToString()))
                {
                    SetColumnHightLight(dg, column, double.MinValue, Math.Max, ref resultCells);
                }
                else if (minColumns.Contains(dg.Columns[column].Header.ToString()))
                {
                    SetColumnHightLight(dg, column, double.MaxValue, Math.Min, ref resultCells);
                }
            }
        }

        private void ListView_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.OriginalSource is DependencyObject obj)
            {
                var item = obj.FindVisualTreeParent<ListViewItem>();
                if (item == null) return;

                var model = (FilterModel)item.DataContext;
                aggregator.Publish<FundCompareFilterModelClickEvent, FilterModel>(model);
            }
        }

        private void SetColumnHightLight(DataGrid dg, int column, double value, Func<double, double, double> compare, ref List<DataGridCell> resultCells)
        {
            DataGridCell cell;
            double cellValue, compareValue;
            for (int row = 0; row < dg.Items.Count; row++)
            {
                cell = dg.GetCell(row, column);
                if (cell == null) continue;

                cellValue = ((TextBlock)cell.Content).Text.AsDouble();
                compareValue = compare(value, cellValue);
                if (value != compareValue)
                {
                    value = compareValue;
                    resultCells.Clear();
                    resultCells.Add(cell);
                }
                else if (cellValue == value)
                {
                    resultCells.Add(cell);
                }
            }

            foreach (var item in resultCells)
            {
                item.Foreground = new SolidColorBrush(Colors.Red);
            }
        }
    }
}
