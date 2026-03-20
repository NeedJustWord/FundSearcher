using System;
using System.Globalization;
using System.Windows.Data;

namespace FundSearcher.Converters
{
    internal class SeparatorConvert : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            switch (values?.Length)
            {
                case 2:
                    return $"{values[0]}{values[1]}";
                case 3:
                    return $"{values[0]}({values[2]}){values[1]}";
                default:
                    return "";
            }
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
