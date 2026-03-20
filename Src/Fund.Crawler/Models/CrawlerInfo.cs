using System;

namespace Fund.Crawler.Models
{
    /// <summary>
    /// 爬取信息
    /// </summary>
    public class CrawlerInfo
    {
        /// <summary>
        /// 名字
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 爬取耗时
        /// </summary>
        public TimeSpan CrawlerTime { get; set; }

        /// <summary>
        /// 爬取耗时
        /// </summary>
        public long CrawlerTimeTicks => CrawlerTime.Ticks;

        /// <summary>
        /// 处理耗时
        /// </summary>
        public TimeSpan HandleTime { get; set; }

        /// <summary>
        /// 处理耗时
        /// </summary>
        public long HandleTimeTicks => HandleTime.Ticks;

        /// <summary>
        /// 是否使用缓存
        /// </summary>
        public bool UseCache { get; set; }

        /// <summary>
        /// 处理是否成功
        /// </summary>
        public bool HandleSuccess { get; set; }
    }
}
