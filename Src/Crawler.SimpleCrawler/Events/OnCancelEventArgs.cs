using System;

namespace Crawler.SimpleCrawler.Events
{
    /// <summary>
    /// 爬虫取消事件数据
    /// </summary>
    public class OnCancelEventArgs : BaseEventArgs
    {
        /// <summary>
        /// 消息
        /// </summary>
        public string Message { get; }

        public OnCancelEventArgs(Uri uri, int threadId, string message) : base(uri, threadId)
        {
            Message = message;
        }
    }
}
