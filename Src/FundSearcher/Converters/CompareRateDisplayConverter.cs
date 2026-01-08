using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Data;
using Fund.Core.Consts;
using Fund.Crawler.Models;

namespace FundSearcher.Converters
{
    class CompareRateDisplayConverter : IMultiValueConverter
    {
        private readonly char[] newLineCharArray = Environment.NewLine.ToCharArray();
        private readonly string[] rateNames = new string[]
        {
            TransactionColumnName.EastMoneyPreferredRates,
            TransactionColumnName.CurrentBuyRates,
            TransactionColumnName.CardBuyRates,
            TransactionColumnName.OriginalRates,
            TransactionColumnName.Rates,
        };

        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var str = (string)values[0];
            var obj = str == TransactionColumnName.SellRates || str == "场内交易" ? values[1] : values[2];
            if (obj == DependencyProperty.UnsetValue) return "";

            if (str == TransactionColumnName.SellRates)
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

            foreach (var rateName in rateNames)
            {
                if (rate.Rate.TryGetValue(rateName, out string value))
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
