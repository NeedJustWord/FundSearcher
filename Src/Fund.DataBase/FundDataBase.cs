using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Fund.Crawler.Models;
using Fund.Crawler.Webs;

namespace Fund.DataBase
{
    /// <summary>
    /// 基金数据库
    /// </summary>
    public class FundDataBase
    {
        private FundUpdate fundUpdate = new FundUpdate();
        private string dbFileName;

        private static char[] fundIdSeparator = new char[] { ' ', ',', '，', '-' };

        /// <summary>
        /// 基金列表
        /// </summary>
        public List<FundInfo> FundInfos => fundUpdate.FundInfos;

        public FundDataBase() : this("FundInfos.txt")
        {
        }

        public FundDataBase(string fileName)
        {
            dbFileName = fileName;
            Load();
        }

        /// <summary>
        /// 加载数据库
        /// </summary>
        public void Load()
        {
            if (File.Exists(dbFileName))
            {
                try
                {
                    var json = File.ReadAllText(dbFileName);
                    var list = json.FromJson<List<FundInfo>>();
                    fundUpdate.Init(list);
                }
                catch (Exception)
                {
                }
            }
        }

        /// <summary>
        /// 保存数据库
        /// </summary>
        public void Save()
        {
            if (fundUpdate.HasUpdate)
            {
                File.WriteAllText(dbFileName, fundUpdate.FundInfos.ToJson());
            }
        }

        /// <summary>
        /// 获取基金信息
        /// </summary>
        /// <param name="fundIds">基金代码</param>
        /// <param name="sourceName">信息来源</param>
        /// <param name="forceUpdate">是否强制更新</param>
        /// <returns></returns>
        public async Task<List<FundInfo>> GetFundInfos(string fundIds, string sourceName = EastMoneyCrawler.SourceNameKey, bool forceUpdate = false)
        {
            return await GetFundInfos(sourceName, forceUpdate, fundIds == null ? new string[0] : fundIds.Split(fundIdSeparator, StringSplitOptions.RemoveEmptyEntries).Where(t => int.TryParse(t, out _)).SelectMany(GetFundIds).Distinct().ToArray());
        }

        /// <summary>
        /// 获取基金信息
        /// </summary>
        /// <param name="forceUpdate">是否强制更新</param>
        /// <param name="fundIds">基金代码</param>
        /// <returns></returns>
        public async Task<List<FundInfo>> GetFundInfos(bool forceUpdate = false, params string[] fundIds)
        {
            return await GetFundInfos(EastMoneyCrawler.SourceNameKey, forceUpdate, fundIds);
        }

        private async Task<List<FundInfo>> GetFundInfos(string sourceName, bool forceUpdate = false, params string[] fundIds)
        {
            var keys = fundIds == null ? new FundKey[0] : fundIds.Select(t => new FundKey(t, sourceName));
            return await GetFundInfos(keys, forceUpdate);
        }

        private async Task<List<FundInfo>> GetFundInfos(IEnumerable<FundKey> keys, bool forceUpdate)
        {
            return await Task.Run(() =>
            {
                List<FundInfo> fundInfos = new List<FundInfo>();
                if (keys != null)
                {
                    fundInfos.AddRange(fundUpdate.Update(keys, forceUpdate).Result);
                }
                return fundInfos;
            });
        }

        private IEnumerable<string> GetFundIds(string input)
        {
            if (input.Length == 6) yield return input;
            if (input.Length < 6)
            {
                foreach (var item in FundInfos)
                {
                    if (item.FundId.Contains(input)) yield return item.FundId;
                }
            }
        }
    }
}
