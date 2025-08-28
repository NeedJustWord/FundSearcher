using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Fund.Crawler;
using Fund.Crawler.Models;
using Fund.Crawler.Webs;
using Prism.Events;

namespace Fund.DataBase
{
    class FundUpdate
    {
        private Dictionary<string, FundCrawler> crawlerDict = new Dictionary<string, FundCrawler>();
        private Dictionary<FundKey, FundInfo> fundInfoDict = new Dictionary<FundKey, FundInfo>();
        private Dictionary<string, Tuple<List<IndexInfo>, DateTime>> indexInfoDict = new Dictionary<string, Tuple<List<IndexInfo>, DateTime>>();
        private List<IndexInfo> emptyIndexInfos = new List<IndexInfo>(0);
        private bool isIniting;

        /// <summary>
        /// 基金列表
        /// </summary>
        public List<FundInfo> FundInfos => fundInfoDict.Values.ToList();

        /// <summary>
        /// 指数列表
        /// </summary>
        public List<IndexInfo> IndexInfos => indexInfoDict.Values.SelectMany(t => t.Item1).ToList();

        /// <summary>
        /// 基金列表是否有更新
        /// </summary>
        public bool HasFundUpdate { get; private set; }

        /// <summary>
        /// 指数列表是否有更新
        /// </summary>
        public bool HasIndexUpdate { get; private set; }

        public FundUpdate(IEventAggregator eventAggregator)
        {
            crawlerDict.Add(EastMoneyCrawler.SourceNameKey, new FundCrawler(new EastMoneyCrawler(eventAggregator)));
        }

        #region 基金
        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="fundInfos"></param>
        public void Init(IEnumerable<FundInfo> fundInfos)
        {
            isIniting = true;
            fundInfoDict.Clear();
            Add(fundInfos);
            isIniting = false;
            HasFundUpdate = false;
        }

        /// <summary>
        /// 添加基金
        /// </summary>
        /// <param name="fundInfos"></param>
        private void Add(IEnumerable<FundInfo> fundInfos)
        {
            if (fundInfos != null)
            {
                foreach (var fundInfo in fundInfos)
                {
                    if (fundInfo != null)
                    {
                        fundInfoDict[(FundKey)fundInfo] = fundInfo;
                        if (HasFundUpdate == false && isIniting == false)
                        {
                            HasFundUpdate = true;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 删除基金
        /// </summary>
        /// <param name="fundInfos"></param>
        /// <returns></returns>
        public List<FundInfo> Delete(FundInfo[] fundInfos)
        {
            List<FundInfo> result = new List<FundInfo>(fundInfos.Length);
            foreach (var fundInfo in fundInfos)
            {
                if (fundInfoDict.Remove((FundKey)fundInfo))
                {
                    result.Add(fundInfo);
                }
            }
            if (result.Count > 0) HasFundUpdate = true;

            return result;
        }

        /// <summary>
        /// 更新基金
        /// </summary>
        /// <param name="fundKeys"></param>
        /// <param name="forceUpdate"></param>
        /// <param name="token">任务取消token</param>
        /// <returns></returns>
        public async Task<List<FundInfo>> Update(IEnumerable<FundKey> fundKeys, bool forceUpdate, CancellationToken token)
        {
            return await Task.Run(() =>
            {
                Dictionary<string, List<string>> updateDict = new Dictionary<string, List<string>>();
                foreach (var key in fundKeys)
                {
                    if (forceUpdate || fundInfoDict.TryGetValue(key, out var value) == false || value.IsNeedUpdate())
                    {
                        if (updateDict.TryGetValue(key.FundInfoSource, out var list) == false)
                        {
                            list = new List<string>();
                            updateDict[key.FundInfoSource] = list;
                        }
                        list.Add(key.FundId);
                    }
                }

                foreach (var updateItem in updateDict)
                {
                    if (crawlerDict.TryGetValue(updateItem.Key, out var crawler) && token.IsCancellationRequested == false)
                    {
                        Add(crawler.Start(token, updateItem.Value.ToArray()).Result);
                    }
                }

                List<FundInfo> result = new List<FundInfo>();
                foreach (var key in fundKeys)
                {
                    if (fundInfoDict.TryGetValue(key, out var value))
                    {
                        result.Add(value);
                    }
                }
                return result;
            });
        }
        #endregion

        #region 指数

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="dict"></param>
        public void Init(Dictionary<string, List<IndexInfo>> dict)
        {
            isIniting = true;
            indexInfoDict.Clear();

            foreach (var item in dict)
            {
                if (dict.Values.Count > 0)
                {
                    indexInfoDict[item.Key] = new Tuple<List<IndexInfo>, DateTime>(item.Value, item.Value.First().UpdateTime);
                }
            }

            isIniting = false;
            HasIndexUpdate = false;
        }

        /// <summary>
        /// 更新指数
        /// </summary>
        /// <param name="sourceName"></param>
        /// <param name="forceUpdate"></param>
        /// <param name="token">任务取消token</param>
        /// <returns></returns>
        public async Task<List<IndexInfo>> Update(string sourceName, bool forceUpdate, CancellationToken token)
        {
            return await Task.Run(() =>
            {
                List<IndexInfo> infos = null;
                bool needUpdate = false;

                if (indexInfoDict.TryGetValue(sourceName, out var tuple))
                {
                    infos = tuple.Item1;
                    needUpdate = forceUpdate || tuple.Item2.IsNeedUpdate();
                }
                else
                {
                    needUpdate = true;
                }

                if (needUpdate)
                {
                    if (crawlerDict.TryGetValue(sourceName, out var crawler))
                    {
                        if (token.IsCancellationRequested)
                        {
                            return infos ?? emptyIndexInfos;
                        }
                        infos = crawler.Start(token).Result;
                        indexInfoDict[sourceName] = new Tuple<List<IndexInfo>, DateTime>(infos, DateTime.Now);
                        HasIndexUpdate = true;
                    }
                }

                return infos ?? emptyIndexInfos;
            });
        }

        /// <summary>
        /// 更新指数相关基金
        /// </summary>
        /// <param name="info"></param>
        /// <param name="forceUpdate"></param>
        /// <param name="token">任务取消token</param>
        /// <returns></returns>
        public async Task<List<FundBaseInfo>> Update(IndexInfo info, bool forceUpdate, CancellationToken token)
        {
            return await Task.Run(() =>
            {
                bool needUpdate = forceUpdate || (info.FundBaseInfos.Count > 0 && info.FundBaseInfos[0].UpdateTime.IsNeedUpdate());
                if (needUpdate)
                {
                    if (crawlerDict.TryGetValue(info.InfoSource, out var crawler))
                    {
                        if (token.IsCancellationRequested)
                        {
                            return info.FundBaseInfos;
                        }
                        var index = crawler.Start(info.IndexCode, token).Result;
                        info.FundBaseInfos = index.FundBaseInfos;
                        info.TrackingCount = index.FundBaseInfos.Count;
                        HasIndexUpdate = true;
                    }
                }
                return info.FundBaseInfos;
            });
        }
        #endregion
    }
}
