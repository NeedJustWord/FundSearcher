using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Fund.Core.Helpers;
using Fund.Crawler;
using Fund.Crawler.Extensions;
using Fund.Crawler.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTest
{
    [TestClass]
    public class RegexTest : BaseTest
    {
        [TestMethod]
        public void PrintCrawlerInfoTest()
        {
            AsyncHelper.RunSync(PrintCrawlerInfoTask);
        }

        private async Task PrintCrawlerInfoTask()
        {
            var fundIds = GetFundIds();
            var crawler = GetCrawler((model) => { });
            var infos = await GetFundInfos(crawler, fundIds);
            var msg = infos.Select(t => t.CrawlerInfo).PrintCrawlerInfo();
            WriteMsg(msg);
        }

        [TestMethod]
        public void BatchRunTest()
        {
            AsyncHelper.RunSync(BatchRunTask);
        }

        private async Task BatchRunTask()
        {
            var fundIds = GetFundIds();
            var crawler = GetCrawler((model) => { });
            var filePath = "BatchRun";

            var infos = await GetFundInfos(crawler, fundIds);
            var msg = infos.Select(t => new CrawlerInfo(t.CrawlerInfo)).PrintCrawlerInfo();
            WriteMsg($"首次处理结果：{Environment.NewLine}{msg}", Path.Combine(filePath, "首次结果.txt"));

            int count = 10;
            var data = new List<List<CrawlerInfo>>(count);
            for (int i = 0; i < count; i++)
            {
                infos = await GetFundInfos(crawler, fundIds);
                data.Add(infos.Where(t => t != null).Select(t => new CrawlerInfo(t.CrawlerInfo)).ToList());
            }

            var result = Handle(data);
            msg = result.PrintCrawlerInfo();
            WriteMsg($"处理{count}次的平均结果：{Environment.NewLine}{msg}", Path.Combine(filePath, "平均结果.txt"));
        }

        private List<CrawlerInfo> Handle(List<List<CrawlerInfo>> data)
        {
            var result = new List<CrawlerInfo>(data.First().Count);
            var group = data.SelectMany(t => t).GroupBy(t => t.Name);
            foreach (var item in group)
            {
                var list = item.ToList();
                var info = new CrawlerInfo(list.First());
                info.CrawlerTime = new TimeSpan((long)list.Average(t => t.CrawlerTimeTicks));
                info.HandleTime = new TimeSpan((long)list.Average(t => t.HandleTimeTicks));
                result.Add(info);
            }
            return result;
        }

        private Task<List<FundInfo>> GetFundInfos(FundCrawler crawler, params string[] fundIds)
        {
            return Task.Run(() =>
            {
                var infos = crawler.StartFund(CancellationToken.None, fundIds).Result;
                return infos;
            });
        }

        private string[] GetFundIds()
        {
            return "021757,004788,021758,004789,310318,007804,200002,022914,167601,006912,017925,005113,010736,010737,166802,005114,014372,020220,020221,003957,022866,003958,018257,022867,015387,012911,018258,012912,019656,015388,022917,005870,009059,016690,002670,003884,003015,519116,009060,022906,002671,003885,110030,003579,019210,007044,010854,016204,023290,100038,673100,007045,005248,023291,006020,013291,022899,008184,016205,673101,010855,021877,006021,000311,021878,004881,015679,022922,022309,022940,165515,010311,022908,006600,022504,022310,008592,022936,050002,022935,022505,013120,008593,020011,021635,160807,481009,022599,007143,015061,002385,000312,022859,021494,021832,005639,022860,022600,008390,022973,021833,008776,006937,007144,005867,000613,015062,022928,110020,000313,022983,000051,008391,003876,021103,008777,005640,022955,000961,000656,007339,160706,022890,165309,007538,450008,008238,000176,519300,005658,022948,460300,005918,007539,015278,007096,005137,022699,007404,501043,008291,022924,015671,160724,008926,006131,005102,016134,202015,160615,005103,020352,008239,008292,501045,007275,023423,015279,006939,007448,161811,022964,004342,008927,270010,011545,010872,162213,021847,660008,002987,005850,007276,021848,010873,003548,011546,005152,165310,022954,009208,320014,015906,010908,004513,002310,004512,002315,001015,015907,010352,010909,022090,022949,001016,022091,020160,004190,005530,004191,020161,010556,012206,000512,022366,002063,012207,022367,022962,163407,007230,026675,026608,025481,025480,024577,024576,025379,025378,024416,024415,024314,024313,024856,024855,024627,024626,023607,023608,026275,025932,025502,025501,025936,025882,025706,025705,025676,025677,025329,024637,024636,014166,014165,023059,023060,024012,024011,024546,023147,023146,009981,009982,022340,001879,015794,021811,006928,022341,161613,021810,020870,015795,020872,012900,004870,020871,160223,022907,110026,022912,002656,007664,012901,015600,010183,007665,004744,009012,022960,001592,004343,001593,022896,003765,009046,009013,003766,019817,009047,050021,022920,006733,022952,161022,005390,160637,006248,013277,005873,015673,006249,012179,005391,013443,005874,012180,163209,010356,018370,018371,025376,025377,025170,025171,025164,025165,024858,024857,025195,025196,026152,026151,025009,025238,026773,026772,024064,024063,024492,000218,008142,000216,022653,022502,004253,008143,020341,000307,000217,009198,002963,021740,008701,002610,008702,002611,001021,001023,003722,016534,012871,016535,018968,019172,018043,016055,019173,160213,016452,019736,016453,018044,016057,270042,019441,019737,019524,022664,006479,019442,161130,000834,019525,016532,023422,019547,012870,539001,008971,016533,019548,012752,015299,018966,021773,040046,015300,014978,018967".Split(',');
        }
    }
}
