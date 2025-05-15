using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Crawler.SimpleCrawler;
using Fund.Crawler.Extensions;
using Fund.Crawler.Models;
using Fund.Crawler.PubSubEvents;
using Prism.Events;

namespace Fund.Crawler.Webs
{
    /// <summary>
    /// 网站爬虫基类
    /// </summary>
    public abstract class BaseWebCrawler
    {
        private static Random random = new Random();
        private static object lockObj = new object();

        /// <summary>
        /// 来源名称
        /// </summary>
        public string SourceName { get; }

        /// <summary>
        /// 爬取基金信息的页面数
        /// </summary>
        public abstract int FundPageCount { get; }

        /// <summary>
        /// 爬取指数相关基金信息的页面数
        /// </summary>
        public abstract int IndexPageCount { get; }

        protected IEventAggregator eventAggregator;
        private int total;
        private int current;

        protected BaseWebCrawler(string sourceName, IEventAggregator eventAggregator)
        {
            SourceName = sourceName;
            this.eventAggregator = eventAggregator;
        }

        /// <summary>
        /// 根据key爬取基金信息
        /// <para>若想知道批量爬取进度，调用此方法前调用<see cref="InitFundTotal(int)"/>方法初始化批量总数</para>
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public abstract Task<FundInfo> Start(FundKey key);

        /// <summary>
        /// 根据key爬取指数相关基金信息
        /// <para>若想知道批量爬取进度，调用此方法前调用<see cref="InitIndexTotal(int)"/>方法初始化批量总数</para>
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public abstract Task<IndexInfo> Start(IndexKey key);

        /// <summary>
        /// 爬取所有指数信息
        /// </summary>
        /// <returns></returns>
        public abstract Task<List<IndexInfo>> Start();

        /// <summary>
        /// 创建基金信息
        /// </summary>
        /// <param name="fundId">基金代码</param>
        /// <returns></returns>
        protected FundInfo CreateFundInfo(string fundId)
        {
            return new FundInfo
            {
                InfoSource = SourceName,
                UpdateTime = DateTime.Now,
                FundId = fundId,
            };
        }

        /// <summary>
        /// 创建指数信息
        /// </summary>
        /// <param name="indexCode"></param>
        /// <returns></returns>
        protected IndexInfo CreateIndexInfo(string indexCode)
        {
            return new IndexInfo
            {
                InfoSource = SourceName,
                UpdateTime = DateTime.Now,
                IndexCode = indexCode,
                FundBaseInfos = new List<FundBaseInfo>(),
            };
        }

        /// <summary>
        /// 使用简单爬虫爬取<paramref name="url"/>
        /// </summary>
        /// <param name="key">爬取信息key</param>
        /// <param name="url">爬虫URL地址</param>
        /// <param name="info">爬取信息</param>
        /// <param name="action">页面源码处理方法</param>
        /// <returns></returns>
        protected async Task<string> StartSimpleCrawler<TInfo>(BaseKey key, string url, TInfo info, Action<string, TInfo> action)
        {
            var crawler = new SimpleCrawler();
            crawler.OnStartEvent += (sender, args) =>
            {
                var keyStr = key.GetKey(url);
                if (key.Index != 0)
                {
                    WriteLog($"{args.ThreadId} {keyStr}开始休眠");
                    RandomSleep(keyStr, 3, 15);
                    WriteLog($"{args.ThreadId} {keyStr}休眠结束");
                }
                WriteLog($"{args.ThreadId} {keyStr}开始爬取");
            };
            crawler.OnCompletedEvent += (sender, args) =>
            {
                var keyStr = key.GetKey(url);
                try
                {
                    WriteLog($"{args.ThreadId} {keyStr}爬取结束，开始处理");
                    action?.Invoke(args.PageSource, info);
                }
                catch (Exception ex)
                {
                    WriteLog($"{args.ThreadId} {keyStr}处理出现异常，{ex.Message}");
                }
                finally
                {
                    IncrementCurrent();
                    WriteLog($"{args.ThreadId} {keyStr}处理结束");
                }
            };
            return await crawler.Start(url);
        }

        /// <summary>
        /// 获取随机休眠时间
        /// </summary>
        /// <param name="minSecond"></param>
        /// <param name="maxSecond"></param>
        /// <returns></returns>
        private static int GetRandomSleepSecond(int minSecond, int maxSecond)
        {
            lock (lockObj)
            {
                return random.Next(minSecond, maxSecond);
            }
        }

        /// <summary>
        /// 随机休眠
        /// </summary>
        /// <param name="key"></param>
        /// <param name="minSecond"></param>
        /// <param name="maxSecond"></param>
        private void RandomSleep(string key, int minSecond, int maxSecond)
        {
            var second = GetRandomSleepSecond(minSecond, maxSecond);
            WriteLog($"{key}随机休眠时间 {second}s");
            Task.Delay(second * 1000).Wait();
        }

        /// <summary>
        /// 初始化总数
        /// </summary>
        /// <param name="total"></param>
        public void InitFundTotal(int total)
        {
            this.total = total * FundPageCount;
            current = 0;
        }

        /// <summary>
        /// 初始化总数
        /// </summary>
        /// <param name="total"></param>
        public void InitIndexTotal(int total)
        {
            this.total = total * IndexPageCount;
            current = 0;
        }

        private void IncrementCurrent()
        {
            Interlocked.Increment(ref current);
        }

        public void WriteLog(string msg)
        {
            eventAggregator.Publish<CrawlingProgressEvent, CrawlingProgressModel>(new CrawlingProgressModel()
            {
                Total = total,
                Current = current,
                Message = $"{DateTime.Now} {msg}",
            });
        }
    }
}
