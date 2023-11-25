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
        /// <param name="sourceName">基金代码</param>
        /// <param name="separator">分隔符</param>
        /// <returns></returns>
        public async Task<List<FundInfo>> GetFundInfos(string fundIds, string sourceName = EastMoneyCrawler.SourceNameKey, char separator = ',')
        {
            return await GetFundInfos(sourceName, fundIds == null ? new string[0] : fundIds.Split(separator));
        }

        /// <summary>
        /// 获取基金信息
        /// </summary>
        /// <param name="fundIds">基金代码</param>
        /// <returns></returns>
        public async Task<List<FundInfo>> GetFundInfos(params string[] fundIds)
        {
            return await GetFundInfos(EastMoneyCrawler.SourceNameKey, fundIds);
        }

        /// <summary>
        /// 获取基金信息
        /// </summary>
        /// <param name="sourceName">基金信息来源</param>
        /// <param name="fundIds">基金代码</param>
        /// <returns></returns>
        public async Task<List<FundInfo>> GetFundInfos(string sourceName, params string[] fundIds)
        {
            var keys = fundIds == null ? new FundKey[0] : fundIds.Select(t => new FundKey(t, sourceName));
            return await GetFundInfos(keys);
        }

        private async Task<List<FundInfo>> GetFundInfos(IEnumerable<FundKey> keys)
        {
            return await Task.Run(() =>
            {
                List<FundInfo> fundInfos = new List<FundInfo>();
                if (keys != null)
                {
                    fundInfos.AddRange(fundUpdate.Update(keys).Result);
                }
                return fundInfos;
            });
        }
    }
}
