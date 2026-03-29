using System;
using System.Threading;
using System.Threading.Tasks;
using Fund.Core.Helpers;
using Fund.Crawler.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTest
{
    [TestClass]
    public class EastMoneyCrawlerTest : BaseTest
    {
        [TestMethod]
        public void GetAllIndexInfoTest()
        {
            AsyncHelper.RunSync(GetAllIndexInfoTask);
        }

        private async Task GetAllIndexInfoTask()
        {
            await GetAllIndexInfo(WriteJson);
        }

        [TestMethod]
        public void GetIndexInfoTest()
        {
            AsyncHelper.RunSync(GetIndexInfoTask);
        }

        private async Task GetIndexInfoTask()
        {
            await GetIndexInfo(WriteJson, "NDX100");
        }

        [TestMethod]
        public void GetIndexFundIdTest()
        {
            AsyncHelper.RunSync(GetIndexFundIdTask);
        }

        private async Task GetIndexFundIdTask()
        {
            await GetIndexInfo(WriteFundId, "NDX100", "000300", "399006");
        }

        private Task GetIndexInfo(Action<IndexInfo> action, params string[] indexCodes)
        {
            return Task.Run(() =>
            {
                var crawler = GetCrawler();
                var infos = crawler.StartIndex(CancellationToken.None, indexCodes).Result;
                foreach (var info in infos)
                {
                    action(info);
                }
            });
        }

        private Task GetAllIndexInfo(Action<IndexInfoList> action)
        {
            return Task.Run(() =>
            {
                var crawler = GetCrawler();
                var infos = crawler.StartIndex(CancellationToken.None).Result;
                action(infos);
            });
        }
    }
}
