using Newtonsoft.Json;

namespace Fund.Crawler.Models
{
    /// <summary>
    /// 基金基础信息
    /// </summary>
    public class FundBaseInfo : BaseInfo
    {
        private string fundId;
        /// <summary>
        /// 基金代码
        /// </summary>
        [JsonProperty(Order = -7)]
        public string FundId
        {
            get { return fundId; }
            set { SetProperty(ref fundId, value); }
        }

        private string fundName;
        /// <summary>
        /// 基金简称
        /// </summary>
        [JsonProperty(Order = -6)]
        public string FundName
        {
            get { return fundName; }
            set { SetProperty(ref fundName, value); }
        }
    }
}
