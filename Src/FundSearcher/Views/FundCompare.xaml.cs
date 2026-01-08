using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Fund.Core.Consts;
using Fund.Core.Extensions;
using Fund.Crawler.Extensions;
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
            DataGridCell cell;
            for (int row = 0; row < dgCompare.Items.Count; row++)
            {
                for (int column = 1; column < dgCompare.Columns.Count; column += 2)
                {
                    cell = dgCompare.GetCell(row, column, out _);
                    if (cell != null) cell.Background = brush;
                }
            }

            SetColumnHightLight();
        }

        private void ListView_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.OriginalSource is DependencyObject obj)
            {
                var item = obj.FindVisualTreeParent<ListViewItem>();
                if (item == null) return;

                var model = (FilterModel)item.DataContext;
                aggregator.Publish<FundCompareFilterModelClickEvent, FilterModel>(model);
                SetColumnHightLight();
            }
        }

        private void SetColumnHightLight()
        {
            //取最大值的列，null当作最大值
            var maxColumnsNullAsMaxValue = new string[]
            {
                TransactionColumnName.BuyUpperLimitAmountWithUnit,
                FundColumnName.AssetWithUnit,
                FundColumnName.ShareWithUnit,
            };
            //取最大值的列，null当作最小值
            var maxColumnsNullAsMinValue = new string[]
            {
                SpecialColumnName.SharpeRatioInThePastYear1,
                SpecialColumnName.SharpeRatioInThePastYear2,
                SpecialColumnName.SharpeRatioInThePastYear3,
                SpecialColumnName.InfoRatioInThePastYear1,
                SpecialColumnName.InfoRatioInThePastYear2,
                SpecialColumnName.InfoRatioInThePastYear3,
            };
            //取最小值的列，null当作最大值
            var minColumnsNullAsMaxValue = new string[]
            {
                SpecialColumnName.VolatilityInThePastYear1,
                SpecialColumnName.VolatilityInThePastYear2,
                SpecialColumnName.VolatilityInThePastYear3,
                SpecialColumnName.AnnualizedTrackingError,
                SpecialColumnName.AverageTrackingErrorOfTheSameType,
            };
            //取最小值的列，null当作最小值
            var minColumnsNullAsMinValue = new string[]
            {
                TransactionColumnName.Price,
                TransactionColumnName.NormalBuyRates,
                TransactionColumnName.ManageRateWithUnit,
                TransactionColumnName.HostingRateWithUnit,
                TransactionColumnName.SalesServiceRateWithUnit,
                TransactionColumnName.RunningRateWithUnit,
            };
            string header;
            var allCells = new List<DataGridCell>(dgCompare.Items.Count);
            var resultCells = new List<DataGridCell>(dgCompare.Items.Count);
            for (int column = 0; column < dgCompare.Columns.Count; column++)
            {
                header = dgCompare.Columns[column].Header.ToString();
                if (maxColumnsNullAsMaxValue.Contains(header))
                {
                    SetColumnHightLight(dgCompare, column, double.MinValue, double.MaxValue, Math.Max, ref allCells, ref resultCells);
                }
                else if (maxColumnsNullAsMinValue.Contains(header))
                {
                    SetColumnHightLight(dgCompare, column, double.MinValue, double.MinValue, Math.Max, ref allCells, ref resultCells);
                }
                else if (minColumnsNullAsMaxValue.Contains(header))
                {
                    SetColumnHightLight(dgCompare, column, double.MaxValue, double.MaxValue, Math.Min, ref allCells, ref resultCells);
                }
                else if (minColumnsNullAsMinValue.Contains(header))
                {
                    SetColumnHightLight(dgCompare, column, double.MaxValue, double.MinValue, Math.Min, ref allCells, ref resultCells);
                }
            }
        }

        private void SetColumnHightLight(DataGrid dg, int column, double value, double nullAsValue, Func<double, double, double> compare, ref List<DataGridCell> allCells, ref List<DataGridCell> resultCells)
        {
            DataGridCell cell;
            double cellValue, compareValue;

            allCells.Clear();
            for (int row = 0; row < dg.Items.Count; row++)
            {
                cell = dg.GetCell(row, column, out bool isHiddenRow);
                if (isHiddenRow || cell == null) continue;

                allCells.Add(cell);
                cellValue = ((TextBlock)cell.Content).Text.AsDouble(nullAsValue);
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

            SolidColorBrush normal = new SolidColorBrush(Colors.Black), red = new SolidColorBrush(Colors.Red);
            foreach (var item in allCells)
            {
                item.Foreground = normal;
            }
            foreach (var item in resultCells)
            {
                item.Foreground = red;
            }
        }
    }
}
