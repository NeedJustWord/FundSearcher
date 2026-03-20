namespace Fund.Crawler.Models
{
    /// <summary>
    /// 基金Key
    /// </summary>
    public class FundKey : BaseKey
    {
        /// <summary>
        /// 基金代码
        /// </summary>
        public string FundId { get; set; }

        public FundKey(long index, string fundId) : base(index, $"Fund:{fundId}")
        {
            FundId = fundId;
        }

        public override string GetKey(string url)
        {
            return $"[{FundId},{Index},{url}]";
        }
    }
}
