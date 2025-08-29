using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace FundSearcher.Converters
{
    class SpecialDictDisplayConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var nullValue = "--";
            if (values[0] == DependencyProperty.UnsetValue) return nullValue;

            var dict = (Dictionary<string, double?>)values[0];
            var year = (int)values[1];
            var format = (string)values[2];
            return dict.TryGetValue($"近{year}年", out var value) && value.HasValue ? value.Value.ToString(format) : nullValue;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
