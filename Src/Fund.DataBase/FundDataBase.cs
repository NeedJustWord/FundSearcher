using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Fund.Crawler.Models;
using Fund.Crawler.Webs;
using Prism.Events;

namespace Fund.DataBase
{
    /// <summary>
    /// 基金数据库
    /// </summary>
    public class FundDataBase
    {
        private FundUpdate fundUpdate;
        private string dbFileName;
        private string dbFileNameWithExtension => $"{dbFileName}.txt";

        private static char[] fundIdSeparator = new char[] { ' ', ',', '，', '-' };

        /// <summary>
        /// 基金列表
        /// </summary>
        public List<FundInfo> FundInfos => fundUpdate.FundInfos;

        public FundDataBase(IEventAggregator eventAggregator) : this("FundInfos", eventAggregator)
        {
        }

        public FundDataBase(string fileName, IEventAggregator eventAggregator)
        {
            fundUpdate = new FundUpdate(eventAggregator);
            dbFileName = fileName;
            Load();
        }

        /// <summary>
        /// 加载数据库
        /// </summary>
        public void Load()
        {
            var files = Directory.GetFiles(".", $"{dbFileName}*.txt");
            var list = files.SelectMany(t => File.ReadAllText(t).FromJson<List<FundInfo>>())
                .GroupBy(t => new FundKey(t.FundId, t.InfoSource))
                .Select(t => t.OrderByDescending(x => x.UpdateTime).First());
            fundUpdate.Init(list);
            Save(files);
        }

        /// <summary>
        /// 保存数据库
        /// </summary>
        public void Save()
        {
            if (fundUpdate.HasUpdate)
            {
                File.WriteAllText(dbFileNameWithExtension, fundUpdate.FundInfos.CustomSort().ToJson());
            }
        }

        private void Save(string[] files)
        {
            if (files.Length == 0) return;
            if (files.Length > 1 || string.Compare(Path.GetFileName(files[0]), dbFileNameWithExtension, true) != 0)
            {
                foreach (var file in files)
                {
                    File.Delete(file);
                }
                File.WriteAllText(dbFileNameWithExtension, fundUpdate.FundInfos.CustomSort().ToJson());
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
