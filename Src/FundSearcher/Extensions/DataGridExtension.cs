using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace FundSearcher.Extensions
{
    static class DataGridExtension
    {
        /// <summary>
        /// 获取DataGrid的指定单元格
        /// </summary>
        /// <param name="dataGrid">DataGrid</param>
        /// <param name="rowIndex">行号</param>
        /// <param name="columnIndex">列号</param>
        /// <param name="isHiddenRow">是否隐藏行</param>
        /// <returns>指定单元格</returns>
        public static DataGridCell GetCell(this DataGrid dataGrid, int rowIndex, int columnIndex, out bool isHiddenRow)
        {
            var row = dataGrid.GetRow(rowIndex);
            DataGridCell cell = null;
            if (row != null)
            {
                isHiddenRow = row.Visibility != Visibility.Visible;
                var presenter = row.FindVisualTreeChild<DataGridCellsPresenter>();
                if (presenter != null)
                {
                    cell = (DataGridCell)presenter.ItemContainerGenerator.ContainerFromIndex(columnIndex);
                    if (cell == null)
                    {
                        dataGrid.ScrollIntoView(row, dataGrid.Columns[columnIndex]);
                        cell = (DataGridCell)presenter.ItemContainerGenerator.ContainerFromIndex(columnIndex);
                    }
                }
            }
            else
            {
                isHiddenRow = true;
            }

            return cell;
        }

        /// <summary>
        /// 获取DataGrid的指定行
        /// </summary>
        /// <param name="dataGrid">DataGrid</param>
        /// <param name="rowIndex">行号</param>
        /// <returns>指定行</returns>
        public static DataGridRow GetRow(this DataGrid dataGrid, int rowIndex)
        {
            DataGridRow row = (DataGridRow)dataGrid.ItemContainerGenerator.ContainerFromIndex(rowIndex);
            if (row == null)
            {
                dataGrid.UpdateLayout();
                dataGrid.ScrollIntoView(dataGrid.Items[rowIndex]);
                row = (DataGridRow)dataGrid.ItemContainerGenerator.ContainerFromIndex(rowIndex);
            }
            return row;
        }
    }
}