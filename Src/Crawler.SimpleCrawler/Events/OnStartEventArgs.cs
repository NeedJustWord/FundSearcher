using System;

namespace Crawler.SimpleCrawler.Events
{
    /// <summary>
    /// 爬虫启动事件数据
    /// </summary>
    public class OnStartEventArgs : BaseEventArgs
    {
        public OnStartEventArgs(Uri uri, int threadId) : base(uri, threadId)
        {
        }
    }
}
