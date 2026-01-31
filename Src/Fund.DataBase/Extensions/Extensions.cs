using System;
using System.Collections.Generic;
using System.Linq;

namespace Fund.Crawler.Models
{
    public static class Extensions
    {
        private static readonly char[] inputSeparator = new char[] { ' ', ',', '，', '-' };

        /// <summary>
        /// 输入分割
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string[] InputSplit(this string str)
        {
            return str == null ? new string[0] : str.Split(inputSeparator, StringSplitOptions.RemoveEmptyEntries);
        }

        /// <summary>
        /// 分割
        /// </summary>
        /// <param name="str"></param>
        /// <param name="separator"></param>
        /// <returns></returns>
        public static string[] SplitRemoveEmpty(this string str, params char[] separator)
        {
            return str == null ? new string[0] : str.Split(separator, StringSplitOptions.RemoveEmptyEntries);
        }

        /// <summary>
        /// 判断是否需要更新
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public static bool IsNeedUpdate(this DateTime time)
        {
            return time < DateTime.Now.AddDays(-30);
        }

        /// <summary>
        /// 判断基金信息是否需要更新
        /// </summary>
        /// <param name="fundInfo"></param>
        /// <returns></returns>
        public static bool IsNeedUpdate(this FundInfo fundInfo)
        {
            return fundInfo == null || fundInfo.UpdateTime.IsNeedUpdate();
        }

        /// <summary>
        /// 自定义排序
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public static IEnumerable<FundInfo> CustomSort(this IEnumerable<FundInfo> list)
        {
            return list.OrderByDescending(t => t.IsHolding).ThenBy(t => t.Counter).ThenBy(t => t.TrackingTarget).ThenBy(t => t.TransactionInfo?.RunningRate).ThenByDescending(t => t.AssetSize).ThenBy(t => t.BirthDay).ThenBy(t => t.FundId);
        }

        /// <summary>
        /// 自定义排序
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public static IEnumerable<IndexInfo> CustomSort(this IEnumerable<IndexInfo> list)
        {
            return list.OrderByDescending(t => t.TrackingCount > 0).ThenBy(t => t.IndexCode);
        }

        /// <summary>
        /// 自定义排序
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public static IEnumerable<FundBaseInfo> CustomSort(this IEnumerable<FundBaseInfo> list)
        {
            return list.OrderBy(t => t.FundId);
        }
    }
}
