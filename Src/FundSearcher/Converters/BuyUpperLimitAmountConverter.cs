using System;
using System.Globalization;
using System.Windows.Data;

namespace FundSearcher.Converters
{
    /// <summary>
    /// 申购上限转换器
    /// </summary>
    class BuyUpperLimitAmountConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value == null ? "不限" : value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
