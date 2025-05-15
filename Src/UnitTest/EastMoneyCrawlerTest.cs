using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Fund.Core.Helpers;
using Fund.Crawler.Extensions;
using Fund.Crawler.Models;
using Fund.Crawler.PubSubEvents;
using Fund.Crawler.Webs;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Prism.Events;

namespace UnitTest
{
    [TestClass]
    public class EastMoneyCrawlerTest
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
                crawler.InitIndexTotal(indexCodes.Length);
                foreach (var item in indexCodes)
                {
                    var info = GetIndexInfo(crawler, item).Result;
                    action(info);
                }
            });
        }

        private Task GetAllIndexInfo(Action<List<IndexInfo>> action)
        {
            return Task.Run(() =>
            {
                var crawler = GetCrawler();
                var infos = crawler.Start().Result;
                action(infos);
            });
        }

        private void WriteJson<T>(T t)
        {
            Debug.WriteLine(t.ToJson());
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
            var aggregator = new EventAggregator();
            aggregator.Subscribe<CrawlingProgressEvent, CrawlingProgressModel>(HandleCrawleProgress);
            return new EastMoneyCrawler(aggregator);
        }

        private void HandleCrawleProgress(CrawlingProgressModel model)
        {
            Console.WriteLine($"{model.Message} 进度：{(double)model.Current / model.Total:P2}");
        }
    }
}
