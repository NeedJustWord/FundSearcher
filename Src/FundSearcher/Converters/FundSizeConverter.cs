using System;
using System.Globalization;
using System.Windows.Data;

namespace FundSearcher.Converters
{
    /// <summary>
    /// 基金规模转换器
    /// </summary>
    class FundSizeConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var value = (double)values[0];
            var deadline = (DateTime)values[1];
            var format = (string)values[2];
            return $"{value.ToString(format)}(截止至:{deadline:yyyy-MM-dd})";
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
