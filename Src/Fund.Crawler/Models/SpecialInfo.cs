using System.Collections.Generic;
using Prism.Mvvm;

namespace Fund.Crawler.Models
{
    /// <summary>
    /// 特色数据
    /// </summary>
    public class SpecialInfo : BindableBase
    {
        private Dictionary<string, double?> volatility;
        /// <summary>
        /// 波动率
        /// </summary>
        public Dictionary<string, double?> Volatility
        {
            get { return volatility; }
            set { SetProperty(ref volatility, value); }
        }

        private Dictionary<string, double?> sharpeRatio;
        /// <summary>
        /// 夏普比率
        /// </summary>
        public Dictionary<string, double?> SharpeRatio
        {
            get { return sharpeRatio; }
            set { SetProperty(ref sharpeRatio, value); }
        }

        private Dictionary<string, double?> infoRatio;
        /// <summary>
        /// 信息比率
        /// </summary>
        public Dictionary<string, double?> InfoRatio
        {
            get { return infoRatio; }
            set { SetProperty(ref infoRatio, value); }
        }

        private double? annualizedTrackingError;
        /// <summary>
        /// 年化跟踪误差
        /// </summary>
        public double? AnnualizedTrackingError
        {
            get { return annualizedTrackingError; }
            set { SetProperty(ref annualizedTrackingError, value); }
        }

        private double? averageTrackingErrorOfTheSameType;
        /// <summary>
        /// 同类平均跟踪误差
        /// </summary>
        public double? AverageTrackingErrorOfTheSameType
        {
            get { return averageTrackingErrorOfTheSameType; }
            set { SetProperty(ref averageTrackingErrorOfTheSameType, value); }
        }
    }
}
