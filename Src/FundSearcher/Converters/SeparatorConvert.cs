using System;
using System.Globalization;
using System.Windows.Data;

namespace FundSearcher.Converters
{
    internal class SeparatorConvert : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            return values?.Length > 1 ? $"{values[0]}{values[1]}" : "";
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
