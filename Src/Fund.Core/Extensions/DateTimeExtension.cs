using System;

namespace Fund.Core.Extensions
{
    public static class DateTimeExtension
    {
        /// <summary>
        /// 获取指定时间相差多少年多少个月
        /// </summary>
        /// <param name="date"></param>
        /// <param name="date"></param>
        /// <returns></returns>
        public static string GetCreateYearAndMonth(this DateTime date, DateTime other)
        {
            DateTime min, max;
            if (date > other)
            {
                max = date;
                min = other;
            }
            else
            {
                max = other;
                min = date;
            }

            int year = max.Year, month = max.Month;
            if (max.Month < min.Month)
            {
                year--;
                month += 12;
            }

            year -= min.Year;
            month -= min.Month;

            return year == 0 ? $"{month}个月" : (month == 0 ? $"{year}年" : $"{year}年{month}个月");
        }
    }
}
