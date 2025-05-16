using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Crawler.DataHandler.Extensions;
using Fund.Core.Extensions;
using Fund.Crawler.Models;
using Prism.Events;

namespace Fund.Crawler.Webs
{
    /// <summary>
    /// EastMoney网站爬虫
    /// </summary>
    public class EastMoneyCrawler : BaseWebCrawler
    {
        public const string SourceNameKey = "天天基金";

        public override int FundPageCount => 2;

        public override int IndexPageCount => 1;

        public EastMoneyCrawler(IEventAggregator eventAggregator) : base(SourceNameKey, eventAggregator)
        {
        }

        public async override Task<FundInfo> Start(FundKey key)
        {
            return await Task.Run(async () =>
            {
                var fundInfo = CreateFundInfo(key.FundId);

                var task = new Task[]
                {
                    GetBaseInfo(key, fundInfo),
                    GetTransactionInfo(key, fundInfo),
                };

                await Task.WhenAll(task);
                return fundInfo;
            });
        }

        public async override Task<IndexInfo> Start(IndexKey key)
        {
            return await Task.Run(async () =>
            {
                var info = CreateIndexInfo(key.IndexCode);
                await GetIndexInfo(key, info);
                return info;
            });
        }

        public async override Task<List<IndexInfo>> Start()
        {
            return await Task.Run(async () =>
            {
                var infos = new List<IndexInfo>();
                await GetIndexInfo(infos);
                return infos;
            });
        }

        #region 基金信息
        #region 基本概况
        /// <summary>
        /// 基本概况
        /// </summary>
        /// <param name="key"></param>
        /// <param name="fundInfo"></param>
        private async Task GetBaseInfo(FundKey key, FundInfo fundInfo)
        {
            var url = $"http://fundf10.eastmoney.com/jbgk_{key.FundId}.html";
            await StartSimpleCrawler(key, url, fundInfo, HandlerBaseInfoSource);
        }

        private void HandlerBaseInfoSource(string pageSource, FundInfo fundInfo)
        {
            var content = pageSource.GetFirstHtmlTagValueByAttri("div", "class", "bs_jz")
                .GetFirstHtmlTagValueByAttri("div", "class", "col-right")
                .GetHtmlTagValue("p", 1)
                .GetHtmlTagContent();
            fundInfo.Counter = content.Contains("场内交易") ? "场内交易" : "场外交易";

            var keyValues = pageSource.GetFirstHtmlTagValueByAttri("table", "class", "info w790")
            .GetHtmlTagValue("tr")
            .SelectMany(t =>
            {
                var replaceA = Regex.Replace(t, "</?a[^>]*>", "");
                var array = Regex.Replace(replaceA, "(<[^>]*>)+", "|").Trim('|').Split('|');
                var lastIndex = array.Length % 2 == 0 ? array.Length - 2 : array.Length - 3;
                var list = new List<KeyValuePair<string, string>>();
                for (int i = 0; i <= lastIndex; i += 2)
                {
                    list.Add(new KeyValuePair<string, string>(array[i], array[i + 1]));
                }
                return list;
            }).Where(t => t.Key != null);

            foreach (var item in keyValues)
            {
                var key = item.Key;
                var value = item.Value;
                switch (key)
                {
                    case "基金全称":
                        fundInfo.FundFullName = value;
                        break;
                    case "基金简称":
                        fundInfo.FundName = value;
                        break;
                    case "基金类型":
                        fundInfo.FundType = value;
                        break;
                    case "发行日期":
                        fundInfo.IssueDay = DateTime.Parse(value);
                        break;
                    case "成立日期/规模":
                        {
                            var tmp = value.Split('/');
                            fundInfo.BirthDay = tmp[0].IsNullOrWhiteSpace() ? (DateTime?)null : DateTime.Parse(tmp[0].TrimEnd());
                            fundInfo.BirthSize = GetSize(tmp[1].TrimStart());
                        }
                        break;
                    case "资产规模":
                        fundInfo.AssetSize = GetSize(value);
                        fundInfo.AssetDeadline = GetDeadline(value);
                        break;
                    case "份额规模":
                        fundInfo.ShareSize = GetSize(value);
                        fundInfo.ShareDeadline = GetDeadline(value);
                        break;
                    case "跟踪标的":
                        fundInfo.TrackingTarget = value.IsNullOrEmpty() || value == "该基金无跟踪标的" ? "无" : value;
                        break;
                }
            }
        }
        #endregion

        #region 交易信息
        /// <summary>
        /// 交易信息
        /// </summary>
        /// <param name="key"></param>
        /// <param name="fundInfo"></param>
        private async Task GetTransactionInfo(FundKey key, FundInfo fundInfo)
        {
            var url = $"http://fundf10.eastmoney.com/jjfl_{key.FundId}.html";
            await StartSimpleCrawler(key, url, fundInfo, HandleTransactionInfoSource);
        }

        private void HandleTransactionInfoSource(string pageSource, FundInfo fundInfo)
        {
            var info = new TransactionInfo();

            var keyValues = pageSource.GetHtmlTagValueByAttri("div", "class", "boxitem w790")
            .Select(t =>
            {
                var key = t.GetFirstHtmlTagValueByAttri("label", "class", "left")?.GetHtmlTagContent();
                return new KeyValuePair<string, string>(key, t);
            })
            .Where(t => t.Key != null);

            foreach (var item in keyValues)
            {
                switch (item.Key)
                {
                    case "交易确认日":
                        var temp = item.Value.GetHtmlTagValue("td").ToList();
                        if (temp.Count > 3)
                        {
                            info.BuyConfirmDate = temp[1].GetHtmlTagContent();
                            info.SellConfirmDate = temp[3].GetHtmlTagContent();
                        }
                        break;
                    case "运作费用":
                        temp = item.Value.GetHtmlTagValue("td").ToList();
                        if (temp.Count > 5)
                        {
                            info.ManageRate = GetRate(temp[1].GetHtmlTagContent());
                            info.HostingRate = GetRate(temp[3].GetHtmlTagContent());
                            info.SalesServiceRate = GetRate(temp[5].GetHtmlTagContent());
                        }
                        break;
                    case "交易状态":
                        temp = item.Value.GetHtmlTagValue("td").ToList();
                        if (temp.Count > 3)
                        {
                            info.BuyStatus = temp[1].GetHtmlTagContent();
                            info.SellStatus = temp[3].GetHtmlTagContent();
                        }
                        break;
                    case "申购与赎回金额":
                        temp = item.Value.GetHtmlTagValue("td").ToList();
                        if (temp.Count > 5)
                        {
                            info.BuyUpperLimitAmount = GetYuanAmount(temp[5].GetHtmlTagContent());
                        }
                        break;
                    default:
                        if (item.Key.Contains("认购费率"))
                        {
                            info.ApplyRates = GetTransactionRates(item.Value);
                        }
                        else if (item.Key.Contains("申购费率"))
                        {
                            info.BuyRates = GetTransactionRates(item.Value);
                        }
                        else if (item.Key.Contains("赎回费率"))
                        {
                            info.SellRates = GetTransactionRates(item.Value);
                        }
                        break;
                }
            }

            fundInfo.TransactionInfo = info;
        }
        #endregion
        #endregion

        #region 指数相关基金信息
        /// <summary>
        /// 指数相关基金信息
        /// </summary>
        /// <param name="key"></param>
        /// <param name="info"></param>
        private async Task GetIndexInfo(IndexKey key, IndexInfo info)
        {
            var url = $"https://zhishubao.1234567.com.cn/home/detail?code={key.IndexCode}";
            await StartSimpleCrawler(key, url, info, HandleIndexInfoSource);
        }

        private void HandleIndexInfoSource(string pageSource, IndexInfo info)
        {
            info.IndexName = pageSource.GetFirstHtmlTagValueByAttri("div", "class", "subNav").GetHtmlTagValue("span", 1).GetHtmlTagContent().Replace("相关指数基金", "");

            var rows = pageSource.GetFirstHtmlTagValueByAttri("table", "class", "index_concat_table show-all").GetFirstHtmlTagValue("tbody").GetHtmlTagValue("tr").ToList();
            info.TrackingCount = rows.Count;
            info.FundBaseInfos.Capacity = rows.Count;
            var now = DateTime.Now;
            foreach (var row in rows)
            {
                var td = row.GetFirstHtmlTagValue("td");
                info.FundBaseInfos.Add(new FundBaseInfo
                {
                    InfoSource = SourceName,
                    UpdateTime = now,
                    FundId = td.GetFirstHtmlTagValue("p").GetHtmlTagContent(),
                    FundName = td.GetFirstHtmlTagValue("a").GetHtmlTagContent(),
                });
            }
        }
        #endregion

        #region 指数信息
        private async Task GetIndexInfo(List<IndexInfo> result)
        {
            var url = $"https://zhishubao.1234567.com.cn/home/AllIndex";
            await StartSimpleCrawler(new BaseKey(0), url, result, HandleIndexInfoSource);
        }

        private void HandleIndexInfoSource(string pageSource, List<IndexInfo> infos)
        {
            var now = DateTime.Now;
            var rows = pageSource.GetFirstHtmlTagValueByAttri("ul", "class", "clearfix all_index_ul ").GetHtmlTagValueByAttri("div", "class", "link").ToList();
            infos.Capacity = rows.Count;

            int trackingCount;
            foreach (var row in rows)
            {
                if (int.TryParse(row.GetFirstHtmlTagValueByAttri("span", "class", "red").GetHtmlTagContent(), out trackingCount) == false)
                {
                    trackingCount = 0;
                }

                infos.Add(new IndexInfo
                {
                    InfoSource = SourceName,
                    UpdateTime = now,
                    IndexCode = row.GetAttriValue("data-code"),
                    IndexName = row.GetAttriValue("data-name"),
                    TrackingCount = trackingCount,
                    FundBaseInfos = new List<FundBaseInfo>(trackingCount),
                });
            }
        }
        #endregion

        private List<TransactionRate> GetTransactionRates(string input)
        {
            var result = new List<TransactionRate>();
            if (input.Contains("table") == false) return result;

            var rateName = input.GetFirstHtmlTagValue("label").GetHtmlTagContent();
            bool? isFront = null;
            if (rateName.Contains("前端"))
            {
                isFront = true;
            }
            if (rateName.Contains("后端"))
            {
                isFront = false;
            }

            var sgyh = input.GetHtmlTagValueByAttri("div", "class", "sgyh").FirstOrDefault();
            string[] columns;
            if (sgyh == null)
            {
                columns = new string[]
                {
                    input.GetHtmlTagValue("th", 2).GetHtmlTagContent().GetHtmlTagContent(true),
                };
            }
            else
            {
                List<string> list = new List<string>()
                {
                    input.GetFirstHtmlTagValue("span").GetHtmlTagContent(),
                };
                var temp = sgyh.GetHtmlTagContent();
                if (temp.Contains("|"))
                {
                    temp = temp.Substring(temp.IndexOf("<br/>") + 5);
                    list.AddRange(Regex.Replace(temp, "<.*>", "|").Split('|'));
                }
                else
                {
                    list.Add(temp);
                }

                columns = list.ToArray();
            }

            var rows = input.GetFirstHtmlTagValue("tbody").GetHtmlTagValue("tr");
            foreach (var row in rows)
            {
                var rate = new TransactionRate() { IsFront = isFront };

                var tds = row.GetHtmlTagValue("td").ToList();
                rate.ApplicableAmount = tds[0].GetHtmlTagContent();
                rate.ApplicablePeriod = tds[1].GetHtmlTagContent();

                rate.Rate = new Dictionary<string, string>();
                var temp = tds[2].GetHtmlTagContent().Replace("&nbsp;", "").Replace("</strike>", "");
                var index = temp.IndexOf(">");
                if (index > -1)
                {
                    temp = temp.Substring(index + 1);
                    var rates = temp.Split('|');
                    for (int i = 0; i < columns.Length; i++)
                    {
                        rate.Rate.Add(columns[i], rates[i]);
                    }
                }
                else
                {
                    for (int i = 0; i < columns.Length; i++)
                    {
                        rate.Rate.Add(columns[i], temp);
                    }
                }

                result.Add(rate);
            }

            return result;
        }

        private double? GetSize(string str)
        {
            if (str.All(t => t == '-')) return null;
            return double.Parse(str.Substring(0, str.IndexOf("亿")));
        }

        private DateTime? GetDeadline(string str)
        {
            if (str.All(t => t == '-')) return null;
            return DateTime.Parse(str.Substring(str.IndexOf("：") + 1, 11));
        }

        private double GetRate(string str)
        {
            if (str == "---") return 0;
            if (str.IndexOf("每年") == -1) return -1;
            return double.Parse(str.Substring(0, str.IndexOf('%'))) / 100;
        }

        private double? GetYuanAmount(string str)
        {
            if (str == null) return null;

            if (str.EndsWith("元"))
            {
                str = str.TrimEnd('元');
                int factor = 1;
                if (str.EndsWith("万"))
                {
                    str = str.TrimEnd('万');
                    factor = 1_0000;
                }
                else if (str.EndsWith("亿"))
                {
                    str = str.TrimEnd('亿');
                    factor = 1_0000_0000;
                }
                return double.Parse(str) * factor;
            }

            return null;
        }
    }
}
