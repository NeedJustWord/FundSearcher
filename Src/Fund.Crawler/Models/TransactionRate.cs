using System.Collections.Generic;

namespace Fund.Crawler.Models
{
    /// <summary>
    /// 交易费率
    /// </summary>
    public class TransactionRate
    {
        /// <summary>
        /// 是否是前端
        /// </summary>
        public bool? IsFront { get; set; }
        /// <summary>
        /// 适用金额
        /// </summary>
        public string ApplicableAmount { get; set; }
        /// <summary>
        /// 适用期限
        /// </summary>
        public string ApplicablePeriod { get; set; }
        /// <summary>
        /// 费率
        /// </summary>
        public Dictionary<string, string> Rate { get; set; }
    }
}
