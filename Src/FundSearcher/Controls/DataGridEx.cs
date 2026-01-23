using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace FundSearcher.Controls
{
    class DataGridEx : DataGrid
    {
        /// <summary>
        /// 隐藏列名
        /// </summary>
        public List<string> HiddenColumns
        {
            get { return (List<string>)GetValue(HiddenColumnsProperty); }
            set { SetValue(HiddenColumnsProperty, value); }
        }

        public static readonly DependencyProperty HiddenColumnsProperty =
            DependencyProperty.Register("HiddenColumns", typeof(List<string>), typeof(DataGridEx), new PropertyMetadata(HiddenColumnsChanged));

        private static void HiddenColumnsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (!(d is DataGrid dg) || e.NewValue == e.OldValue) return;

            InternalSetHiddenColumns(dg, (List<string>)e.NewValue);
        }

        private static void InternalSetHiddenColumns(DataGrid dg, List<string> hiddenColumns)
        {
            Func<string, Visibility> func;
            if (hiddenColumns?.Count > 0)
            {
                func = t => hiddenColumns.Contains(t) ? Visibility.Collapsed : Visibility.Visible;
            }
            else
            {
                func = t => Visibility.Visible;
            }

            foreach (var column in dg.Columns)
            {
                column.Visibility = func((string)column.Header);
            }
        }

        protected override void OnLoadingRow(DataGridRowEventArgs e)
        {
            base.OnLoadingRow(e);

            if (e.Row.GetIndex() == 0)
            {
                InternalSetHiddenColumns(this, HiddenColumns);
            }
        }
    }
}
