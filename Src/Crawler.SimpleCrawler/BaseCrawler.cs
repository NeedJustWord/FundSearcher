using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Crawler.SimpleCrawler.Events;
using Fund.Core.Helpers;

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

        #region 页面源代码缓存
        public void WritePageSourceToCache(OnCompletedEventArgs args)
        {
            var filePath = GetPageSourceCacheFilePath(args.Uri);
            if (ConfigHelper.CachePageSource && args.CacheValid == false)
            {
                File.WriteAllText(filePath, args.PageSource);
            }
        }

        protected bool ReadPageSourceFromCache(Uri uri, out string pageSource, out TimeSpan elapsed)
        {
            bool result;
            var watch = Stopwatch.StartNew();
            var filePath = GetPageSourceCacheFilePath(uri);
            if (ConfigHelper.CachePageSource && IsCacheValid(filePath))
            {
                pageSource = File.ReadAllText(filePath);
                result = true;
            }
            else
            {
                pageSource = string.Empty;
                result = false;
            }
            watch.Stop();
            elapsed = watch.Elapsed;
            return result;
        }

        /// <summary>
        /// 缓存是否有效
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        private bool IsCacheValid(string filePath)
        {
            if (File.Exists(filePath) == false) return false;

            var now = DateTime.Now;
            var lastWriteTime = File.GetLastWriteTime(filePath);
            if (lastWriteTime < now.Date.AddDays(-ConfigHelper.CacheOverDay)) return false;
            if (now.DayOfWeek >= DayOfWeek.Monday
                && now.DayOfWeek <= DayOfWeek.Friday
                && now.TimeOfDay >= ConfigHelper.PriceUpdateTime
                && lastWriteTime.TimeOfDay < ConfigHelper.PriceUpdateTime) return false;

            return true;
        }

        /// <summary>
        /// 获取缓存文件路径
        /// </summary>
        /// <param name="uri"></param>
        /// <returns></returns>
        private string GetPageSourceCacheFilePath(Uri uri)
        {
            DirectoryHelper.Ensure(ConfigHelper.CachePath);

            var fileName = new StringBuilder(uri.PathAndQuery.TrimStart('/'));
            var chars = Path.GetInvalidFileNameChars();
            for (int i = 0; i < fileName.Length; i++)
            {
                var c = fileName[i];
                if (chars.Any(t => t == c))
                {
                    fileName[i] = '_';
                }
            }

            if (uri.PathAndQuery.EndsWith(".html") == false)
            {
                fileName.Append(".html");
            }
            return Path.Combine(ConfigHelper.CachePath, fileName.ToString());
        }
        #endregion
    }
}
