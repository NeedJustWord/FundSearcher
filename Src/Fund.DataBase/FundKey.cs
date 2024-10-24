using System;
using Fund.Crawler.Models;

namespace Fund.DataBase
{
    /// <summary>
    /// 基金主键
    /// </summary>
    struct FundKey
    {
        /// <summary>
        /// 基金代码
        /// </summary>
        public string FundId { get; set; }

        /// <summary>
        /// 基金信息来源
        /// </summary>
        public string FundInfoSource { get; set; }

        public FundKey(string fundId, string fundInfoSource)
        {
            FundId = fundId;
            FundInfoSource = fundInfoSource;
        }

        /// <summary>
        /// <see cref="FundInfo"/>强制转换成<see cref="FundKey"/>
        /// </summary>
        /// <param name="fundInfo"></param>
        public static explicit operator FundKey(FundInfo fundInfo)
        {
            if (fundInfo == null) throw new ArgumentNullException($"{nameof(fundInfo)}为null");

            return new FundKey(fundInfo.FundId, fundInfo.InfoSource);
        }
    }
}
