using System;
using System.Globalization;
using System.Windows.Data;

namespace FundSearcher.Converters
{
    /// <summary>
    /// 日累计申购限额转换器
    /// </summary>
    class BuyUpperLimitAmountConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return "不限";

            var amount = (double)value;
            if (amount >= 1_0000_0000) return $"{amount / 1_0000_0000:F2}亿";
            else if (amount >= 1_0000) return $"{amount / 1_0000:F2}万";
            return $"{amount:F2}";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
