using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Fund.Core.Helpers;
using Fund.Crawler.Models;
using Fund.Crawler.Webs;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTest
{
    [TestClass]
    public class EastMoneyCrawlerTest
    {
        [TestMethod]
        public void GetIndexInfoTest()
        {
            AsyncHelper.RunSync(GetIndexInfoTask);
        }

        private async Task GetIndexInfoTask()
        {
            await Handle(WriteJson, "NDX100");
        }

        [TestMethod]
        public void GetIndexFundIdTest()
        {
            AsyncHelper.RunSync(GetIndexFundIdTask);
        }

        private async Task GetIndexFundIdTask()
        {
            await Handle(WriteFundId, "NDX100", "000300", "399006");
        }

        private Task Handle(Action<IndexInfo> action, params string[] indexCodes)
        {
            return Task.Run(() =>
            {
                var crawler = GetCrawler();
                foreach (var item in indexCodes)
                {
                    var info = GetIndexInfo(crawler, item).Result;
                    action(info);
                }
            });
        }

        private void WriteJson(IndexInfo info)
        {
            Debug.WriteLine(info.ToJson());
        }

        private void WriteFundId(IndexInfo info)
        {
            Debug.WriteLine(string.Join(",", info.FundBaseInfos.Select(t => t.FundId)));
        }

        private Task<IndexInfo> GetIndexInfo(BaseWebCrawler crawler, string indexCode)
        {
            var key = new IndexKey(0, indexCode);
            return crawler.Start(key);
        }

        private EastMoneyCrawler GetCrawler()
        {
            return new EastMoneyCrawler();
        }
    }
}
