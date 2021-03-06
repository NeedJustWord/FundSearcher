using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fund.Crawler;
using Fund.Crawler.Models;
using Fund.Crawler.Webs;

namespace Fund.DataBase
{
    class FundUpdate
    {
        private Dictionary<string, FundCrawler> crawlerDict = new Dictionary<string, FundCrawler>();
        private Dictionary<FundKey, FundInfo> fundInfoDict = new Dictionary<FundKey, FundInfo>();

        /// <summary>
        /// 基金列表
        /// </summary>
        public List<FundInfo> FundInfos => fundInfoDict.Values.ToList();

        public FundUpdate() : this(null)
        {
        }

        public FundUpdate(IEnumerable<FundInfo> fundInfos)
        {
            crawlerDict.Add(EastMoneyCrawler.SourceNameKey, new FundCrawler(new EastMoneyCrawler()));
            Add(fundInfos);
        }

        /// <summary>
        /// 添加基金
        /// </summary>
        /// <param name="fundInfos"></param>
        /// <param name="isClear">是否先清空</param>
        public void Add(IEnumerable<FundInfo> fundInfos, bool isClear = false)
        {
            if (isClear)
            {
                fundInfoDict.Clear();
            }
            if (fundInfos != null)
            {
                foreach (var fundInfo in fundInfos)
                {
                    if (fundInfo != null)
                    {
                        fundInfoDict[(FundKey)fundInfo] = fundInfo;
                    }
                }
            }
        }

        /// <summary>
        /// 更新基金
        /// </summary>
        /// <param name="fundKeys"></param>
        /// <returns></returns>
        public async Task<List<FundInfo>> Update(IEnumerable<FundKey> fundKeys)
        {
            return await Task.Run(() =>
            {
                Dictionary<string, List<string>> updateDict = new Dictionary<string, List<string>>();
                foreach (var key in fundKeys)
                {
                    if (fundInfoDict.TryGetValue(key, out var value) == false || value.IsNeedUpdate())
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
                    if (crawlerDict.TryGetValue(updateItem.Key, out var crawler))
                    {
                        Add(crawler.Start(updateItem.Value.ToArray()).Result);
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
    }
}
