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
            if (values[1] == DependencyProperty.UnsetValue) return "";

            var str = (string)values[0];
            if (str == "赎回费率")
            {
                var sb = new StringBuilder();
                foreach (var item in (List<TransactionRate>)values[1])
                {
                    sb.AppendLine($"{GetApplicablePeriod(item.ApplicablePeriod)}：{item.Rate.Values.FirstOrDefault()}");
                }

                var result = sb.ToString().TrimEnd(newLineCharArray);
                if (result.Length == 0) result = "无";
                return result;
            }

            var rates = str == "场内交易" ? (List<TransactionRate>)values[1] : (List<TransactionRate>)values[2];
            var rate = rates.FirstOrDefault();
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
