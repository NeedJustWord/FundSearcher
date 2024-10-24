using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Crawler.SimpleCrawler;
using Fund.Crawler.Models;

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

        protected BaseWebCrawler(string sourceName)
        {
            SourceName = sourceName;
        }

        /// <summary>
        /// 根据key爬取基金信息
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public abstract Task<FundInfo> Start(FundKey key);

        /// <summary>
        /// 根据key爬取指数相关基金信息
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public abstract Task<IndexInfo> Start(IndexKey key);

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
                WriteLog($"{args.ThreadId} {keyStr}爬取结束，开始处理");
                action?.Invoke(args.PageSource, info);
                WriteLog($"{args.ThreadId} {keyStr}处理结束");
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

        protected void WriteLog(string msg)
        {
            Console.WriteLine($"{DateTime.Now} {msg}");
        }
    }
}
