using System;
using System.Threading.Tasks;
using Crawler.SimpleCrawler.Events;

namespace Crawler.SimpleCrawler
{
    /// <summary>
    /// 爬虫基类
    /// </summary>
    public abstract class BaseCrawler
    {
        /// <summary>
        /// 爬虫启动事件
        /// </summary>
        public event EventHandler<OnStartEventArgs> OnStartEvent;

        /// <summary>
        /// 爬虫完成事件
        /// </summary>
        public event EventHandler<OnCompletedEventArgs> OnCompletedEvent;

        /// <summary>
        /// 爬虫出错事件
        /// </summary>
        public event EventHandler<OnErrorEventArgs> OnErrorEvent;

        /// <summary>
        /// 异步爬虫
        /// </summary>
        /// <param name="uri">爬虫URL地址</param>
        /// <param name="proxy">代理服务器</param>
        /// <returns>网页源代码</returns>
        public abstract Task<string> Start(Uri uri, string proxy = null);

        /// <summary>
        /// 引发<see cref="OnStartEvent"/>事件
        /// </summary>
        /// <param name="args"></param>
        public void OnStart(OnStartEventArgs args)
        {
            OnStartEvent?.Invoke(this, args);
        }

        /// <summary>
        /// 引发<see cref="OnErrorEvent"/>事件
        /// </summary>
        /// <param name="args"></param>
        public void OnError(OnErrorEventArgs args)
        {
            OnErrorEvent?.Invoke(this, args);
        }

        /// <summary>
        /// 引发<see cref="OnCompletedEvent"/>事件
        /// </summary>
        /// <param name="args"></param>
        public void OnCompleted(OnCompletedEventArgs args)
        {
            OnCompletedEvent?.Invoke(this, args);
        }
    }
}
