using System;

namespace Fund.Crawler.Models
{
    /// <summary>
    /// 基金信息
    /// </summary>
    public class FundInfo
    {
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// 基金信息来源
        /// </summary>
        public string FundInfoSource { get; set; }
        /// <summary>
        /// 基金代码
        /// </summary>
        public string FundId { get; set; }
        /// <summary>
        /// 基金简称
        /// </summary>
        public string FundName { get; set; }
        /// <summary>
        /// 基金全称
        /// </summary>
        public string FundFullName { get; set; }
        /// <summary>
        /// 基金类型
        /// </summary>
        public string FundType { get; set; }
        /// <summary>
        /// 发行日期
        /// </summary>
        public string IssueDay { get; set; }
        /// <summary>
        /// 成立日期/规模
        /// </summary>
        public string BirthStatus { get; set; }
        /// <summary>
        /// 资产规模
        /// </summary>
        public string FundAmount { get; set; }
        /// <summary>
        /// 份额规模
        /// </summary>
        public string FundCount { get; set; }
        /// <summary>
        /// 跟踪标的
        /// </summary>
        public string TrackingTarget { get; set; }
        /// <summary>
        /// 交易信息
        /// </summary>
        public TransactionInfo TransactionInfo { get; set; }
    }
}
