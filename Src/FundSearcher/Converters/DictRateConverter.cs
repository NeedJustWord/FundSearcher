using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Data;

namespace FundSearcher.Converters
{
    /// <summary>
    /// 认购、申购、赎回费率转换器
    /// </summary>
    class DictRateConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var dict = values[0] as Dictionary<string, string>;
            var key = values[1] as string;
            return dict.TryGetValue(key, out var value) ? value : "0.00%";
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
