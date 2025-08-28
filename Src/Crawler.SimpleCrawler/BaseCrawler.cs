using System;
using System.Threading;
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
        /// 爬虫取消事件
        /// </summary>
        public event EventHandler<OnCancelEventArgs> OnCancelEvent;

        /// <summary>
        /// 异步爬虫
        /// </summary>
        /// <param name="uriString">爬虫URL地址</param>
        /// <param name="token">任务取消token</param>
        /// <param name="proxy">代理服务器</param>
        /// <returns>网页源代码</returns>
        public async Task<string> Start(string uriString, CancellationToken token, string proxy = null)
        {
            return await Start(new Uri(uriString), token, proxy);
        }

        /// <summary>
        /// 异步爬虫
        /// </summary>
        /// <param name="uri">爬虫URL地址</param>
        /// <param name="token">任务取消token</param>
        /// <param name="proxy">代理服务器</param>
        /// <returns>网页源代码</returns>
        public abstract Task<string> Start(Uri uri, CancellationToken token, string proxy = null);

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

        /// <summary>
        /// 引发<see cref="OnCancelEvent"/>事件
        /// </summary>
        /// <param name="args"></param>
        public void OnCancel(OnCancelEventArgs args)
        {
            OnCancelEvent?.Invoke(this, args);
        }
    }
}
