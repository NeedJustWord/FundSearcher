using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Fund.Crawler.Models
{
    /// <summary>
    /// 基金信息
    /// </summary>
    public class FundInfo : FundBaseInfo
    {
        private List<string> applyRatesHiddenColumns;
        /// <summary>
        /// 认购费率隐藏列
        /// </summary>
        [JsonIgnore]
        public List<string> ApplyRatesHiddenColumns
        {
            get { return applyRatesHiddenColumns; }
            set { SetProperty(ref applyRatesHiddenColumns, value); }
        }

        private List<string> buyRatesHiddenColumns;
        /// <summary>
        /// 申购费率隐藏列
        /// </summary>
        [JsonIgnore]
        public List<string> BuyRatesHiddenColumns
        {
            get { return buyRatesHiddenColumns; }
            set { SetProperty(ref buyRatesHiddenColumns, value); }
        }

        private List<string> sellRatesHiddenColumns;
        /// <summary>
        /// 赎回费率隐藏列
        /// </summary>
        [JsonIgnore]
        public List<string> SellRatesHiddenColumns
        {
            get { return sellRatesHiddenColumns; }
            set { SetProperty(ref sellRatesHiddenColumns, value); }
        }

        /// <summary>
        /// 基金类别
        /// </summary>
        [JsonIgnore]
        public string FundClass => FundName == null ? "" : (FundName.Contains("C") ? "C类" : "A类");

        private string fundFullName;
        /// <summary>
        /// 基金全称
        /// </summary>
        public string FundFullName
        {
            get { return fundFullName; }
            set { SetProperty(ref fundFullName, value); }
        }

        private string counter;
        /// <summary>
        /// 交易场所
        /// </summary>
        public string Counter
        {
            get { return counter; }
            set { SetProperty(ref counter, value); }
        }

        private string fundType;
        /// <summary>
        /// 基金类型
        /// </summary>
        public string FundType
        {
            get { return fundType; }
            set { SetProperty(ref fundType, value); }
        }

        private DateTime issueDay;
        /// <summary>
        /// 发行日期
        /// </summary>
        public DateTime IssueDay
        {
            get { return issueDay; }
            set { SetProperty(ref issueDay, value); }
        }

        private DateTime? birthDay;
        /// <summary>
        /// 成立日期
        /// </summary>
        public DateTime? BirthDay
        {
            get { return birthDay; }
            set { SetProperty(ref birthDay, value); }
        }

        private double? birthSize;
        /// <summary>
        /// 成立规模(亿份)
        /// </summary>
        public double? BirthSize
        {
            get { return birthSize; }
            set { SetProperty(ref birthSize, value); }
        }

        private double? assetSize;
        /// <summary>
        /// 资产规模(亿元)
        /// </summary>
        public double? AssetSize
        {
            get { return assetSize; }
            set { SetProperty(ref assetSize, value); }
        }

        private DateTime? assetDeadline;
        /// <summary>
        /// 资产规模截止日
        /// </summary>
        public DateTime? AssetDeadline
        {
            get { return assetDeadline; }
            set { SetProperty(ref assetDeadline, value); }
        }

        private double? shareSize;
        /// <summary>
        /// 份额规模(亿份)
        /// </summary>
        public double? ShareSize
        {
            get { return shareSize; }
            set { SetProperty(ref shareSize, value); }
        }

        private DateTime? shareDeadline;
        /// <summary>
        /// 份额规模截止日
        /// </summary>
        public DateTime? ShareDeadline
        {
            get { return shareDeadline; }
            set { SetProperty(ref shareDeadline, value); }
        }

        private string trackingTarget;
        /// <summary>
        /// 跟踪标的
        /// </summary>
        public string TrackingTarget
        {
            get { return trackingTarget; }
            set { SetProperty(ref trackingTarget, value); }
        }

        private TransactionInfo transactionInfo;
        /// <summary>
        /// 交易信息
        /// </summary>
        public TransactionInfo TransactionInfo
        {
            get { return transactionInfo; }
            set { SetProperty(ref transactionInfo, value); }
        }
    }
}
