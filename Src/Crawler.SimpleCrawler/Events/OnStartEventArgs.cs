using System;

namespace Crawler.SimpleCrawler.Events
{
    /// <summary>
    /// 爬虫启动事件数据
    /// </summary>
    public class OnStartEventArgs : BaseEventArgs
    {
        /// <summary>
        /// 缓存是否有效
        /// </summary>
        public bool CacheValid { get; }

        public OnStartEventArgs(Uri uri, int threadId, bool cacheValid) : base(uri, threadId)
        {
            CacheValid = cacheValid;
        }
    }
}
