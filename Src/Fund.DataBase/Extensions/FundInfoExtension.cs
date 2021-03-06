using System;

namespace Fund.Crawler.Models
{
    /// <summary>
    /// <see cref="FundInfo"/>扩展
    /// </summary>
    static class FundInfoExtension
    {
        /// <summary>
        /// 判断基金信息是否需要更新
        /// </summary>
        /// <param name="fundInfo"></param>
        /// <returns></returns>
        public static bool IsNeedUpdate(this FundInfo fundInfo)
        {
            return fundInfo == null || fundInfo.CreateTime < DateTime.Now.AddDays(-30);
        }
    }
}
