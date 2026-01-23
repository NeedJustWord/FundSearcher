using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Fund.Crawler.Models;
using Fund.Crawler.Webs;

namespace Fund.Crawler
{
    /// <summary>
    /// 基金爬虫
    /// </summary>
    public class FundCrawler
    {
        /// <summary>
        /// 网站爬虫
        /// </summary>
        public BaseWebCrawler WebCrawler { get; }

        public FundCrawler(BaseWebCrawler webCrawler)
        {
            WebCrawler = webCrawler ?? throw new ArgumentNullException($"{nameof(webCrawler)}为null");
        }

        /// <summary>
        /// 开始爬取基金信息
        /// </summary>
        /// <param name="token">任务取消token</param>
        /// <param name="fundIds"></param>
        /// <returns></returns>
        public async Task<List<FundInfo>> Start(CancellationToken token, params string[] fundIds)
        {
            return await Task.Run(() =>
            {
                List<FundInfo> result = new List<FundInfo>();
                if (fundIds != null)
                {
                    WebCrawler.InitFundTotal(fundIds.Length);
                    Parallel.ForEach(fundIds, (fundId, pls, index) =>
                    {
                        if (token.IsCancellationRequested)
                        {
                            WebCrawler.WriteLog($"爬取基金信息{fundId}任务已取消");
                            return;
                        }
                        result.Add(WebCrawler.Start(new FundKey(index, fundId), token).Result);
                    });

                    if (token.IsCancellationRequested)
                    {
                        WebCrawler.Cancel("爬取基金信息任务已取消");
                    }
                    else
                    {
                        WebCrawler.Finish("爬取基金信息完成");
                    }
                }
                return result;
            }, token);
        }

        /// <summary>
        /// 开始爬取指数信息
        /// </summary>
        /// <param name="token">任务取消token</param>
        /// <returns></returns>
        public async Task<List<IndexInfo>> Start(CancellationToken token)
        {
            return await Task.Run(() =>
            {
                var result = WebCrawler.Start(token).Result;
                WebCrawler.Finish("爬取指数信息完成");
                return result;
            }, token);
        }

        /// <summary>
        /// 开始爬取指数相关基金信息
        /// </summary>
        /// <param name="indexCode"></param>
        /// <param name="token">任务取消token</param>
        /// <returns></returns>
        public async Task<IndexInfo> Start(string indexCode, CancellationToken token)
        {
            return await Task.Run(() =>
            {
                var key = new IndexKey(0, indexCode);
                var result = WebCrawler.Start(key, token).Result;
                WebCrawler.Finish("爬取指数相关基金信息完成");
                return result;
            }, token);
        }
    }
}
