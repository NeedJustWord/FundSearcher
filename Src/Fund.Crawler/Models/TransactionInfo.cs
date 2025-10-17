using System.Collections.Generic;
using Fund.Crawler.Extensions;
using Newtonsoft.Json;
using Prism.Mvvm;

namespace Fund.Crawler.Models
{
    /// <summary>
    /// 交易信息
    /// </summary>
    public class TransactionInfo : BindableBase
    {
        private double price;
        /// <summary>
        /// 单位净值
        /// </summary>
        public double Price
        {
            get { return price; }
            set { SetProperty(ref price, value); }
        }

        private double? buyUpperLimitAmount;
        /// <summary>
        /// 日累计申购限额，单位元
        /// </summary>
        public double? BuyUpperLimitAmount
        {
            get { return buyUpperLimitAmount; }
            set { SetProperty(ref buyUpperLimitAmount, value); }
        }

        private string buyStatus;
        /// <summary>
        /// 申购状态
        /// </summary>
        public string BuyStatus
        {
            get { return buyStatus; }
            set { SetProperty(ref buyStatus, value); }
        }

        private string sellStatus;
        /// <summary>
        /// 赎回状态
        /// </summary>
        public string SellStatus
        {
            get { return sellStatus; }
            set { SetProperty(ref sellStatus, value); }
        }

        private string buyConfirmDate;
        /// <summary>
        /// 买入确认日
        /// </summary>
        public string BuyConfirmDate
        {
            get { return buyConfirmDate; }
            set { SetProperty(ref buyConfirmDate, value); }
        }

        private string sellConfirmDate;
        /// <summary>
        /// 卖出确认日
        /// </summary>
        public string SellConfirmDate
        {
            get { return sellConfirmDate; }
            set { SetProperty(ref sellConfirmDate, value); }
        }

        private double manageRate;
        /// <summary>
        /// 管理费率(每年)
        /// </summary>
        public double ManageRate
        {
            get { return manageRate; }
            set { SetProperty(ref manageRate, value); }
        }

        private double hostingRate;
        /// <summary>
        /// 托管费率(每年)
        /// </summary>
        public double HostingRate
        {
            get { return hostingRate; }
            set { SetProperty(ref hostingRate, value); }
        }

        private double salesServiceRate;
        /// <summary>
        /// 销售服务费率(每年)
        /// </summary>
        public double SalesServiceRate
        {
            get { return salesServiceRate; }
            set { SetProperty(ref salesServiceRate, value); }
        }

        /// <summary>
        /// 运作费率(每年)
        /// </summary>
        [JsonIgnore]
        public double RunningRate => ManageRate + HostingRate + SalesServiceRate;

        /// <summary>
        /// 运作费率字符串(每年)
        /// </summary>
        [JsonIgnore]
        public string RunningRateStr => RunningRate.ToString("F4");

        private List<TransactionRate> applyRates;
        /// <summary>
        /// 认购费率
        /// </summary>
        public List<TransactionRate> ApplyRates
        {
            get { return applyRates; }
            set { SetProperty(ref applyRates, value); }
        }

        private List<TransactionRate> buyRates;
        /// <summary>
        /// 申购费率
        /// </summary>
        public List<TransactionRate> BuyRates
        {
            get { return buyRates; }
            set { SetProperty(ref buyRates, value); }
        }

        private List<TransactionRate> sellRates;
        /// <summary>
        /// 赎回费率
        /// </summary>
        public List<TransactionRate> SellRates
        {
            get { return sellRates; }
            set
            {
                SetProperty(ref sellRates, value);
                FreeSellDays = sellRates.GetFreeSellDays();
            }
        }

        /// <summary>
        /// 免费赎回天数
        /// </summary>
        [JsonIgnore]
        public int? FreeSellDays { get; private set; }
    }
}
