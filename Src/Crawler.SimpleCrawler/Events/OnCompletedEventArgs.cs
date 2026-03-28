using System;

namespace Crawler.SimpleCrawler.Events
{
    /// <summary>
    /// 爬虫完成事件数据
    /// </summary>
    public class OnCompletedEventArgs : BaseEventArgs
    {
        /// <summary>
        /// 页面源代码
        /// </summary>
        public string PageSource { get; }
        /// <summary>
        /// 爬虫请求执行时间
        /// </summary>
        public TimeSpan Elapsed { get; }

        /// <summary>
        /// 缓存是否有效
        /// </summary>
        public bool CacheValid { get; }

        public OnCompletedEventArgs(Uri uri, int threadId, TimeSpan elapsed, string pageSource, bool cacheValid) : base(uri, threadId)
        {
            Elapsed = elapsed;
            PageSource = pageSource;
            CacheValid = cacheValid;
        }
    }
}
