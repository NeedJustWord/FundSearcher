using System;
using System.Collections.Generic;
using System.Linq;

namespace Fund.Crawler.Models
{
    /// <summary>
    /// <see cref="FundInfo"/>扩展
    /// </summary>
    public static class FundInfoExtension
    {
        /// <summary>
        /// 判断基金信息是否需要更新
        /// </summary>
        /// <param name="fundInfo"></param>
        /// <returns></returns>
        public static bool IsNeedUpdate(this FundInfo fundInfo)
        {
            return fundInfo == null || fundInfo.UpdateTime < DateTime.Now.AddDays(-30);
        }

        /// <summary>
        /// 自定义排序
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public static IEnumerable<FundInfo> CustomSort(this IEnumerable<FundInfo> list)
        {
            return list.OrderBy(t => t.Counter).ThenBy(t => t.TrackingTarget).ThenBy(t => t.TransactionInfo.RunningRate).ThenByDescending(t => t.AssetSize).ThenBy(t => t.BirthDay).ThenBy(t => t.FundId);
        }
    }
}
