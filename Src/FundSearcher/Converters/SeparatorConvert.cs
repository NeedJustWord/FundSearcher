using System;
using System.Globalization;
using System.Windows.Data;

namespace FundSearcher.Converters
{
    internal class SeparatorConvert : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            return values == null ? "" : string.Join("", values);
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
