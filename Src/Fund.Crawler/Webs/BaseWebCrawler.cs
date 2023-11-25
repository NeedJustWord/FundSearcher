using System;
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
        /// 根据基金代码爬取基金信息
        /// </summary>
        /// <param name="fundId">基金代码</param>
        /// <returns></returns>
        public abstract Task<FundInfo> Start(string fundId);

        /// <summary>
        /// 创建基金信息
        /// </summary>
        /// <param name="fundId">基金代码</param>
        /// <returns></returns>
        protected FundInfo CreateFundInfo(string fundId)
        {
            return new FundInfo
            {
                FundInfoSource = SourceName,
                FundId = fundId,
                UpdateTime = DateTime.Now,
            };
        }

        /// <summary>
        /// 使用简单爬虫爬取<paramref name="url"/>
        /// </summary>
        /// <param name="url">爬虫URL地址</param>
        /// <param name="fundInfo">基金信息</param>
        /// <param name="action">页面源码处理方法</param>
        /// <returns></returns>
        protected async Task<string> StartSimpleCrawler(string url, FundInfo fundInfo, Action<string, FundInfo> action)
        {
            var crawler = new SimpleCrawler();
            crawler.OnStartEvent += (sender, args) =>
            {
                WriteLog($"{args.ThreadId} 开始休眠");
                RandomSleep(3, 15);
                WriteLog($"{args.ThreadId} 休眠结束，开始爬取");
            };
            crawler.OnCompletedEvent += (sender, args) =>
            {
                WriteLog($"{args.ThreadId} 爬取结束，开始处理");
                action?.Invoke(args.PageSource, fundInfo);
                WriteLog($"{args.ThreadId} 处理结束");
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
        /// <param name="minSecond"></param>
        /// <param name="maxSecond"></param>
        private void RandomSleep(int minSecond, int maxSecond)
        {
            var second = GetRandomSleepSecond(minSecond, maxSecond);
            WriteLog($"随机时间 {second}");
            Task.Delay(second * 1000).Wait();
        }

        private void WriteLog(string msg)
        {
            Console.WriteLine($"{DateTime.Now} {msg}");
        }
    }
}
