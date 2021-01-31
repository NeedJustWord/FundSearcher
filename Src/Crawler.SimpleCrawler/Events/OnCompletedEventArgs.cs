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
        /// 爬虫请求执行时间，单位毫秒
        /// </summary>
        public long Milliseconds { get; }

        public OnCompletedEventArgs(Uri uri, int threadId, long milliseconds, string pageSource) : base(uri, threadId)
        {
            Milliseconds = milliseconds;
            PageSource = pageSource;
        }
    }
}
