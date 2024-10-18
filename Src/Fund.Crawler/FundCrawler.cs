using System;
using System.Collections.Generic;
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
            if (webCrawler == null) throw new ArgumentNullException($"{nameof(webCrawler)}为null");

            WebCrawler = webCrawler;
        }

        /// <summary>
        /// 开始爬取
        /// </summary>
        /// <param name="fundIds"></param>
        /// <param name="separator"></param>
        /// <returns></returns>
        public async Task<List<FundInfo>> Start(string fundIds, char separator = ',')
        {
            return await Start(fundIds == null ? new string[0] : fundIds.Split(separator));
        }

        /// <summary>
        /// 开始爬取
        /// </summary>
        /// <param name="fundIds"></param>
        /// <returns></returns>
        public async Task<List<FundInfo>> Start(params string[] fundIds)
        {
            return await Task.Run(() =>
            {
                List<FundInfo> result = new List<FundInfo>();
                if (fundIds != null)
                {
                    Parallel.ForEach(fundIds, (fundId, pls, index) =>
                    {
                        if (string.IsNullOrWhiteSpace(fundId)) return;
                        result.Add(WebCrawler.Start(new CrawlerKey(index, fundId)).Result);
                    });
                }
                return result;
            });
        }
    }
}
