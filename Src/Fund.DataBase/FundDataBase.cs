using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
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
        private string fundFileName;
        private string fundFileNameWithExtension;
        private string indexFileName;
        private string indexFileNameWithExtension;

        /// <summary>
        /// 基金列表
        /// </summary>
        public List<FundInfo> FundInfos => fundUpdate.FundInfos;

        public FundDataBase(IEventAggregator eventAggregator, string fundFileName = "FundInfos", string indexFileName = "IndexInfos")
        {
            this.fundFileName = fundFileName;
            fundFileNameWithExtension = $"{fundFileName}.txt";
            this.indexFileName = indexFileName;
            indexFileNameWithExtension = $"{indexFileName}.txt";

            fundUpdate = new FundUpdate(eventAggregator);
            Load();
        }

        /// <summary>
        /// 加载数据库
        /// </summary>
        public void Load()
        {
            var files = Directory.GetFiles(".", $"{fundFileName}*.txt");
            var list = files.SelectMany(t => File.ReadAllText(t).FromJson<List<FundInfo>>())
                .GroupBy(t => new FundKey(t.FundId, t.InfoSource))
                .Select(t => t.OrderByDescending(x => x.UpdateTime).First());
            fundUpdate.Init(list);
            Save(files, fundFileNameWithExtension, fundUpdate.FundInfos.CustomSort().ToJson());

            files = Directory.GetFiles(".", $"{indexFileName}*.txt");
            var group = files.SelectMany(t => File.ReadAllText(t).FromJson<List<IndexInfo>>())
                .GroupBy(t => t.InfoSource);
            var dict = new Dictionary<string, List<IndexInfo>>();
            foreach (var item in group)
            {
                dict[item.Key] = item.GroupBy(t => t.IndexCode).Select(t => t.OrderByDescending(x => x.UpdateTime).First()).ToList();
            }
            fundUpdate.Init(dict);
            Save(files, indexFileNameWithExtension, fundUpdate.IndexInfos.CustomSort().ToJson());
        }

        /// <summary>
        /// 保存数据库
        /// </summary>
        public void Save()
        {
            if (fundUpdate.HasFundUpdate)
            {
                File.WriteAllText(fundFileNameWithExtension, fundUpdate.FundInfos.CustomSort().ToJson());
            }
            if (fundUpdate.HasIndexUpdate)
            {
                File.WriteAllText(indexFileNameWithExtension, fundUpdate.IndexInfos.CustomSort().ToJson());
            }
        }

        private void Save(string[] files, string fileNameWithExtension, string contents)
        {
            if (files.Length == 0) return;
            if (files.Length > 1 || string.Compare(Path.GetFileName(files[0]), fileNameWithExtension, true) != 0)
            {
                foreach (var file in files)
                {
                    File.Delete(file);
                }
                File.WriteAllText(fileNameWithExtension, contents);
            }
        }

        /// <summary>
        /// 删除基金
        /// </summary>
        /// <param name="fundInfos"></param>
        /// <returns></returns>
        public List<FundInfo> Delete(FundInfo[] fundInfos)
        {
            return fundUpdate.Delete(fundInfos);
        }

        /// <summary>
        /// 获取基金信息
        /// </summary>
        /// <param name="fundIds">基金代码</param>
        /// <param name="token">任务取消token</param>
        /// <param name="sourceName">信息来源</param>
        /// <param name="forceUpdate">是否强制更新</param>
        /// <returns></returns>
        public async Task<List<FundInfo>> GetFundInfos(string fundIds, CancellationToken token, string sourceName = EastMoneyCrawler.SourceNameKey, bool forceUpdate = false)
        {
            return await GetFundInfos(sourceName, token, forceUpdate, fundIds.InputSplit().Where(t => int.TryParse(t, out _)).SelectMany(GetFundIds).Distinct().ToArray());
        }

        /// <summary>
        /// 获取基金信息
        /// </summary>
        /// <param name="token">任务取消token</param>
        /// <param name="forceUpdate">是否强制更新</param>
        /// <param name="fundIds">基金代码</param>
        /// <returns></returns>
        public async Task<List<FundInfo>> GetFundInfos(CancellationToken token, bool forceUpdate = false, params string[] fundIds)
        {
            return await GetFundInfos(EastMoneyCrawler.SourceNameKey, token, forceUpdate, fundIds);
        }

        private async Task<List<FundInfo>> GetFundInfos(string sourceName, CancellationToken token, bool forceUpdate = false, params string[] fundIds)
        {
            var keys = fundIds == null ? new FundKey[0] : fundIds.Select(t => new FundKey(t, sourceName));
            return await GetFundInfos(keys, forceUpdate, token);
        }

        private async Task<List<FundInfo>> GetFundInfos(IEnumerable<FundKey> keys, bool forceUpdate, CancellationToken token)
        {
            return await Task.Run(() =>
            {
                List<FundInfo> fundInfos = new List<FundInfo>();
                if (keys != null)
                {
                    fundInfos.AddRange(fundUpdate.Update(keys, forceUpdate, token).Result);
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

        /// <summary>
        /// 获取指数信息
        /// </summary>
        /// <param name="token">任务取消token</param>
        /// <param name="forceUpdate">是否强制更新</param>
        /// <returns></returns>
        public async Task<List<IndexInfo>> GetIndexInfos(CancellationToken token, bool forceUpdate = false)
        {
            return await Task.Run(() =>
            {
                return fundUpdate.Update(EastMoneyCrawler.SourceNameKey, forceUpdate, token);
            });
        }

        /// <summary>
        /// 获取指数相关基金基础信息
        /// </summary>
        /// <param name="info">指数信息</param>
        /// <param name="token">任务取消token</param>
        /// <param name="forceUpdate">是否强制更新</param>
        /// <returns></returns>
        public async Task<List<FundBaseInfo>> GetFundBaseInfos(IndexInfo info, CancellationToken token, bool forceUpdate = false)
        {
            return await Task.Run(() =>
            {
                return fundUpdate.Update(info, forceUpdate, token);
            });
        }

        /// <summary>
        /// 获取指数相关基金基础信息
        /// </summary>
        /// <param name="info">指数信息</param>
        /// <param name="token">任务取消token</param>
        /// <param name="forceUpdate">是否强制更新</param>
        /// <returns></returns>
        public async Task<List<FundBaseInfo>[]> GetFundBaseInfos(IEnumerable<IndexInfo> infos, CancellationToken token, bool forceUpdate = false)
        {
            return await Task.Run(() =>
            {
                return Task.WhenAll(infos.Select(t => fundUpdate.Update(t, forceUpdate, token)));
            });
        }
    }
}
