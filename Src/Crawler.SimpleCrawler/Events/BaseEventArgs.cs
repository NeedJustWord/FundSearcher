using System;

namespace Crawler.SimpleCrawler.Events
{
    /// <summary>
    /// 事件数据基类
    /// </summary>
    public class BaseEventArgs
    {
        /// <summary>
        /// 爬虫URL地址
        /// </summary>
        public Uri Uri { get; }
        /// <summary>
        /// 任务线程ID
        /// </summary>
        public int ThreadId { get; }

        public BaseEventArgs(Uri uri, int threadId)
        {
            Uri = uri;
            ThreadId = threadId;
        }
    }
}
