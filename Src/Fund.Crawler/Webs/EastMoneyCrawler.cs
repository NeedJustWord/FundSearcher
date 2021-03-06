using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Crawler.DataHandler.Extensions;
using Fund.Crawler.Models;

namespace Fund.Crawler.Webs
{
    /// <summary>
    /// EastMoney网站爬虫
    /// </summary>
    public class EastMoneyCrawler : BaseWebCrawler
    {
        public EastMoneyCrawler() : base("天天基金")
        {
        }

        public async override Task<FundInfo> Start(string fundId)
        {
            return await Task.Run(async () =>
            {
                var fundInfo = CreateFundInfo(fundId);

                var task = new Task[]
                {
                    GetBaseInfo(fundId, fundInfo),
                    GetTransactionInfo(fundId, fundInfo),
                };

                await Task.WhenAll(task);
                return fundInfo;
            });
        }

        /// <summary>
        /// 基本概况
        /// </summary>
        /// <param name="fundId"></param>
        /// <param name="fundInfo"></param>
        private async Task GetBaseInfo(string fundId, FundInfo fundInfo)
        {
            var url = $"http://fundf10.eastmoney.com/jbgk_{fundId}.html";
            await StartSimpleCrawler(url, pageSource =>
            {
                HandlerBaseInfoSource(pageSource, fundInfo);
            });
        }

        private void HandlerBaseInfoSource(string pageSource, FundInfo fundInfo)
        {
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
                        fundInfo.IssueDay = value;
                        break;
                    case "成立日期/规模":
                        fundInfo.BirthStatus = value;
                        break;
                    case "资产规模":
                        fundInfo.FundAmount = value;
                        break;
                    case "份额规模":
                        fundInfo.FundCount = value;
                        break;
                    case "跟踪标的":
                        fundInfo.TrackingTarget = value;
                        break;
                }
            }
        }

        /// <summary>
        /// 交易信息
        /// </summary>
        /// <param name="fundId"></param>
        /// <param name="fundInfo"></param>
        private async Task GetTransactionInfo(string fundId, FundInfo fundInfo)
        {
            var url = $"http://fundf10.eastmoney.com/jjfl_{fundId}.html";
            await StartSimpleCrawler(url, pageSource =>
            {
                HandleTransactionInfoSource(pageSource, fundInfo);
            });
        }

        private void HandleTransactionInfoSource(string pageSource, FundInfo fundInfo)
        {
            var info = new TransactionInfo();

            var keyValues = pageSource.GetHtmlTagValueByAttri("div", "class", "boxitem w790")
            .Select(t =>
            {
                var key = t.GetFirstHtmlTagValueByAttri("label", "class", "left").GetHtmlTagContent();
                return new KeyValuePair<string, string>(key, t);
            })
            .Where(t => string.IsNullOrEmpty(t.Key) == false);

            foreach (var item in keyValues)
            {
                switch (item.Key)
                {
                    case "交易确认日":
                        var temp = item.Value.GetHtmlTagValue("td").ToList();
                        info.BuyConfirmDate = temp[1].GetHtmlTagContent();
                        info.SellConfirmDate = temp[3].GetHtmlTagContent();
                        break;
                    case "运作费用":
                        temp = item.Value.GetHtmlTagValue("td").ToList();
                        info.ManageRate = temp[1].GetHtmlTagContent();
                        info.HostingRate = temp[3].GetHtmlTagContent();
                        info.SalesServiceRate = temp[5].GetHtmlTagContent();
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

        private List<TransactionRate> GetTransactionRates(string input)
        {
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
                    input.GetHtmlTagValue("th", 2).GetHtmlTagContent(),
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

            var result = new List<TransactionRate>();
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
    }
}
