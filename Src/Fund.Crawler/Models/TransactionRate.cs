using System.Collections.Generic;
using Prism.Mvvm;

namespace Fund.Crawler.Models
{
    /// <summary>
    /// 交易费率
    /// </summary>
    public class TransactionRate : BindableBase
    {
        private bool? isFront;
        /// <summary>
        /// 是否是前端
        /// </summary>
        public bool? IsFront
        {
            get { return isFront; }
            set { SetProperty(ref isFront, value); }
        }

        private string applicableAmount;
        /// <summary>
        /// 适用金额
        /// </summary>
        public string ApplicableAmount
        {
            get { return applicableAmount; }
            set { SetProperty(ref applicableAmount, value); }
        }

        private string applicablePeriod;
        /// <summary>
        /// 适用期限
        /// </summary>
        public string ApplicablePeriod
        {
            get { return applicablePeriod; }
            set { SetProperty(ref applicablePeriod, value); }
        }

        private Dictionary<string, string> rate;
        /// <summary>
        /// 费率
        /// </summary>
        public Dictionary<string, string> Rate
        {
            get { return rate; }
            set { SetProperty(ref rate, value); }
        }
    }
}
