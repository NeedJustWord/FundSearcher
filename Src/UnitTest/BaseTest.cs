using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Fund.Core.Extensions;
using Fund.Core.Helpers;
using Fund.Crawler;
using Fund.Crawler.Extensions;
using Fund.Crawler.Models;
using Fund.Crawler.PubSubEvents;
using Fund.Crawler.Webs;
using Prism.Events;

namespace UnitTest
{
    public class BaseTest
    {
        public BaseTest()
        {
            Config.ConfigPath = "FundSearcher.exe.config";
        }

        protected FundCrawler GetCrawler()
        {
            return GetCrawler(HandleCrawleProgress);
        }

        protected FundCrawler GetCrawler(Action<CrawlingProgressModel> action)
        {
            var aggregator = new EventAggregator();
            aggregator.Subscribe<CrawlingProgressEvent, CrawlingProgressModel>(action);
            return new FundCrawler(new EastMoneyCrawler(aggregator));
        }

        private void HandleCrawleProgress(CrawlingProgressModel model)
        {
            Console.WriteLine($"{model.Message} 进度：{(double)model.Current / model.Total:P2}");
        }

        #region 打印
        protected void WriteMsg(string msg, string fileName = null)
        {
            Debug.WriteLine(msg);
            if (fileName.IsNotNullAndEmpty())
            {
                var dir = Path.GetDirectoryName(fileName);
                DirectoryHelper.Ensure(dir);
                File.WriteAllText(fileName, msg);
            }
        }

        protected void WriteJson<T>(T t)
        {
            Debug.WriteLine(t.ToJson());
        }

        protected void WriteFundId(IndexInfo info)
        {
            Debug.WriteLine(string.Join(",", info.FundBaseInfos.Select(t => t.FundId)));
        }
        #endregion
    }
}
