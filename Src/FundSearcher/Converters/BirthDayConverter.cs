using System;
using System.Globalization;
using System.Windows.Data;
using Fund.Core.Extensions;

namespace FundSearcher.Converters
{
    class BirthDayConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var date = (DateTime?)value;
            return date.HasValue ? $"{date.Value:yyyy-MM-dd}({date.Value.GetCreateYearAndMonth(DateTime.Now)})" : "---";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
