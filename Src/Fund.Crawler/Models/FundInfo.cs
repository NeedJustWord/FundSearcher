using System;
using Newtonsoft.Json;
using Prism.Mvvm;

namespace Fund.Crawler.Models
{
    /// <summary>
    /// 基金信息
    /// </summary>
    public class FundInfo : BindableBase
    {
        private bool isChecked;
        /// <summary>
        /// 是否选中
        /// </summary>
        [JsonIgnore]
        public bool IsChecked
        {
            get { return isChecked; }
            set { SetProperty(ref isChecked, value); }
        }

        private bool isShow;
        /// <summary>
        /// 是否显示
        /// </summary>
        [JsonIgnore]
        public bool IsShow
        {
            get { return isShow; }
            set { SetProperty(ref isShow, value); }
        }

        private int orderNumber;
        /// <summary>
        /// 序号
        /// </summary>
        [JsonIgnore]
        public int OrderNumber
        {
            get { return orderNumber; }
            set { SetProperty(ref orderNumber, value); }
        }

        private DateTime updateTime;
        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime UpdateTime
        {
            get { return updateTime; }
            set { SetProperty(ref updateTime, value); }
        }

        private string fundInfoSource;
        /// <summary>
        /// 基金信息来源
        /// </summary>
        public string FundInfoSource
        {
            get { return fundInfoSource; }
            set { SetProperty(ref fundInfoSource, value); }
        }

        private string fundId;
        /// <summary>
        /// 基金代码
        /// </summary>
        public string FundId
        {
            get { return fundId; }
            set { SetProperty(ref fundId, value); }
        }

        private string fundName;
        /// <summary>
        /// 基金简称
        /// </summary>
        public string FundName
        {
            get { return fundName; }
            set { SetProperty(ref fundName, value); }
        }

        private string fundFullName;
        /// <summary>
        /// 基金全称
        /// </summary>
        public string FundFullName
        {
            get { return fundFullName; }
            set { SetProperty(ref fundFullName, value); }
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

        private DateTime birthDay;
        /// <summary>
        /// 成立日期
        /// </summary>
        public DateTime BirthDay
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

        private double assetSize;
        /// <summary>
        /// 资产规模(亿元)
        /// </summary>
        public double AssetSize
        {
            get { return assetSize; }
            set { SetProperty(ref assetSize, value); }
        }

        private DateTime assetDeadline;
        /// <summary>
        /// 资产规模截止日
        /// </summary>
        public DateTime AssetDeadline
        {
            get { return assetDeadline; }
            set { SetProperty(ref assetDeadline, value); }
        }

        private double shareSize;
        /// <summary>
        /// 份额规模(亿份)
        /// </summary>
        public double ShareSize
        {
            get { return shareSize; }
            set { SetProperty(ref shareSize, value); }
        }

        private DateTime shareDeadline;
        /// <summary>
        /// 份额规模截止日
        /// </summary>
        public DateTime ShareDeadline
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
