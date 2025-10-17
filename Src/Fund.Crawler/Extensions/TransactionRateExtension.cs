using System.Collections.Generic;
using System.Linq;
using Fund.Core.Extensions;
using Fund.Crawler.Models;

namespace Fund.Crawler.Extensions
{
    static class TransactionRateExtension
    {
        /// <summary>
        /// 获取免费赎回天数
        /// </summary>
        /// <param name="rates"></param>
        /// <returns></returns>
        public static int? GetFreeSellDays(this List<TransactionRate> rates)
        {
            var info = rates.Select(t => new
            {
                t.ApplicablePeriod,
                Rate = t.Rate.Values.First().AsDouble(1),
            }).FirstOrDefault(t => t.Rate == 0);

            if (info == null) return null;

            return GetDays(info.ApplicablePeriod);
        }

        private static int GetDays(string str)
        {
            str = str.Replace("大于", "").Replace("等于", "");
            int factor = 1;
            if (str.EndsWith("天"))
            {
                str = str.TrimEnd('天');
            }
            else if (str.EndsWith("年"))
            {
                str = str.TrimEnd('年');
                factor = 365;
            }
            return int.Parse(str) * factor;
        }
    }
}
