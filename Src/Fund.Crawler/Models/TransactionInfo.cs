using System.Collections.Generic;

namespace Fund.Crawler.Models
{
    /// <summary>
    /// 交易信息
    /// </summary>
    public class TransactionInfo
    {
        /// <summary>
        /// 买入确认日
        /// </summary>
        public string BuyConfirmDate { get; set; }
        /// <summary>
        /// 卖出确认日
        /// </summary>
        public string SellConfirmDate { get; set; }
        /// <summary>
        /// 管理费率
        /// </summary>
        public string ManageRate { get; set; }
        /// <summary>
        /// 托管费率
        /// </summary>
        public string HostingRate { get; set; }
        /// <summary>
        /// 销售服务费率
        /// </summary>
        public string SalesServiceRate { get; set; }
        /// <summary>
        /// 认购费率
        /// </summary>
        public List<TransactionRate> ApplyRates { get; set; }
        /// <summary>
        /// 申购费率
        /// </summary>
        public List<TransactionRate> BuyRates { get; set; }
        /// <summary>
        /// 赎回费率
        /// </summary>
        public List<TransactionRate> SellRates { get; set; }
    }
}
