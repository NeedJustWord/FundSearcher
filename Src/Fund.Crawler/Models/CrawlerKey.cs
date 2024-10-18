namespace Fund.Crawler.Models
{
    public class CrawlerKey
    {
        /// <summary>
        /// 索引
        /// </summary>
        public long Index { get; set; }
        /// <summary>
        /// 基金代码
        /// </summary>
        public string FundId { get; set; }

        public CrawlerKey(long index, string fundId)
        {
            Index = index;
            FundId = fundId;
        }

        public string GetKey(string url)
        {
            return $"[{FundId},{Index},{url}]";
        }
    }
}
