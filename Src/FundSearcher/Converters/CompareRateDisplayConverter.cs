using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Data;
using Fund.Crawler.Models;

namespace FundSearcher.Converters
{
    class CompareRateDisplayConverter : IMultiValueConverter
    {
        private readonly char[] newLineCharArray = Environment.NewLine.ToCharArray();
        private readonly string[] rateNames = new string[]
        {
            "天天基金优惠费率",
            "活期宝购买",
            "银行卡购买",
            "原费率",
            "费率",
        };

        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var str = (string)values[0];
            var obj = str == "赎回费率" || str == "场内交易" ? values[1] : values[2];
            if (obj == DependencyProperty.UnsetValue) return "";

            if (str == "赎回费率")
            {
                var sb = new StringBuilder();
                foreach (var item in (List<TransactionRate>)obj)
                {
                    sb.AppendLine($"{GetApplicablePeriod(item.ApplicablePeriod)}：{item.Rate.Values.FirstOrDefault()}");
                }

                var result = sb.ToString().TrimEnd(newLineCharArray);
                if (result.Length == 0) result = "无";
                return result;
            }

            var rate = ((List<TransactionRate>)obj).FirstOrDefault();
            if (rate == null) return "";

            string value;
            foreach (var rateName in rateNames)
            {
                if (rate.Rate.TryGetValue(rateName, out value))
                {
                    return value;
                }
            }

            return "";
        }

        private string GetApplicablePeriod(string str)
        {
            return str == "---" ? "不限" : str;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
