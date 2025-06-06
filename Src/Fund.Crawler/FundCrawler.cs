﻿using System;
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
        /// 开始爬取基金信息
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
                    WebCrawler.InitFundTotal(fundIds.Length);
                    Parallel.ForEach(fundIds, (fundId, pls, index) =>
                    {
                        result.Add(WebCrawler.Start(new FundKey(index, fundId)).Result);
                    });
                    WebCrawler.WriteLog("爬取基金信息完成");
                }
                return result;
            });
        }

        /// <summary>
        /// 开始爬取指数信息
        /// </summary>
        /// <returns></returns>
        public async Task<List<IndexInfo>> Start()
        {
            return await Task.Run(() =>
            {
                var result = WebCrawler.Start().Result;
                WebCrawler.WriteLog("爬取指数信息完成");
                return result;
            });
        }

        /// <summary>
        /// 开始爬取指数相关基金信息
        /// </summary>
        /// <param name="indexCode"></param>
        /// <returns></returns>
        public async Task<IndexInfo> Start(string indexCode)
        {
            return await Task.Run(() =>
            {
                var key = new IndexKey(0, indexCode);
                var result = WebCrawler.Start(key).Result;
                WebCrawler.WriteLog("爬取指数相关基金信息完成");
                return result;
            });
        }
    }
}
