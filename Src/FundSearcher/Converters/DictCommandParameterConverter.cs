using System;
using System.Globalization;
using System.Windows.Data;
using FundSearcher.Models;

namespace FundSearcher.Converters
{
    /// <summary>
    /// 字典命令参数转换器
    /// </summary>
    class DictCommandParameterConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            return new CommandParameter(values);
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
