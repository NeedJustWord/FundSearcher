using Fund.Crawler.Models;

namespace Fund.Crawler.Interfaces
{
    /// <summary>
    /// 爬取信息接口
    /// </summary>
    public interface ICrawlerInfo
    {
        /// <summary>
        /// 爬取信息
        /// </summary>
        CrawlerInfo CrawlerInfo { get; set; }
    }
}
