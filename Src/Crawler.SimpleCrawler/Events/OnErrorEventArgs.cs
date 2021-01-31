using System;

namespace Crawler.SimpleCrawler.Events
{
    /// <summary>
    /// 爬虫出错事件数据
    /// </summary>
    public class OnErrorEventArgs : BaseEventArgs
    {
        /// <summary>
        /// 异常信息
        /// </summary>
        public Exception Exception { get; }

        public OnErrorEventArgs(Uri uri, int threadId, Exception exception) : base(uri, threadId)
        {
            Exception = exception;
        }
    }
}
